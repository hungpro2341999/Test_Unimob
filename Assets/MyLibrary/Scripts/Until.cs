using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public static class MyExtensions
{
    public static void UnloadAsset(Sprite spr)
    {
        Resources.UnloadAsset(spr);
    }
    public static void PlayAnim(List<Transform> allTrans, float delay = 0, float space = 0.1f)
    {
        float i = 0;
        foreach (var trans in allTrans)
        {
            float index = i;
            trans.localScale = Vector3.zero;
            MyThread.Instance.AddDelayAction(1, () =>
            {
                trans.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).SetDelay(index * space);
            });
            i++;
        }
    }
    public static T GetRandomEnumValue<T>()
    {
        System.Random random = new System.Random();
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }

    public static void AddOnceEnable(this GameObject gameObject, System.Action action)
    {
        OnEnableMethodOnce onceEnable = gameObject.GetComponent<OnEnableMethodOnce>();
        if (onceEnable == null)
        {
            onceEnable = gameObject.AddComponent<OnEnableMethodOnce>();
        }
        onceEnable.AddEnable(action);
    }
    public static void AddOnceDisable(this GameObject gameObject, System.Action action)
    {
        OnDisableMethodOnce onceDisable = gameObject.GetComponent<OnDisableMethodOnce>();
        if (onceDisable == null)
        {
            onceDisable = gameObject.AddComponent<OnDisableMethodOnce>();
        }
        onceDisable.AddDisable(action);
    }

    public static void AddUpdateMethod(this GameObject gameObject, System.Action action)
    {
        UpdateMethod updateMethod = gameObject.GetComponent<UpdateMethod>();
        if (updateMethod == null)
        {
            updateMethod = gameObject.AddComponent<UpdateMethod>();
        }
        updateMethod.AddUpdate(action);
    }
     public static void AddAdapterUpdateMethod(this GameObject gameObject, System.Action action)
    {
        UpdateMethod updateMethod = gameObject.GetComponent<UpdateMethod>();
        if (updateMethod == null)
        {
            updateMethod = gameObject.AddComponent<UpdateMethod>();
        }
        updateMethod.AddAdapterUpdate(action);
    }
    public static void AddOnDestroyMethod(this GameObject gameObject, System.Action action)
    {
        OnDestroyMethod onDestroyMethod = gameObject.GetComponent<OnDestroyMethod>();
        if (onDestroyMethod == null)
        {
            onDestroyMethod = gameObject.AddComponent<OnDestroyMethod>();
        }
        onDestroyMethod.AddOnDestroy(action);
    }
    public static void ClearUdateMethod(this GameObject gameObject)
    {
        UpdateMethod updateMethod = gameObject.GetComponent<UpdateMethod>();
        if (updateMethod == null)
        {
            updateMethod = gameObject.AddComponent<UpdateMethod>();
        }
        updateMethod.ClearUpdate();

    }
    public static Transform FindChildByName(this Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform result = child.FindChildByName(name);
            if (result != null)
                return result;
        }
        return null;
    }
    public static T FindChildByName<T>(this Transform parent, string name) where T : MonoBehaviour
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child.GetComponent<T>();

            Transform result = child.FindChildByName(name);
            if (result != null)
                return result.GetComponent<T>(); ;
        }
        return null;
    }

    public static void SetPositionX(this Transform t, float x)
    {
        t.position = new Vector3(x, t.position.y, t.position.z);
    }

    public static void SetPositionY(this Transform t, float y)
    {
        t.position = new Vector3(t.position.x, y, t.position.z);
    }

    public static void SetPositionZ(this Transform t, float z)
    {
        t.position = new Vector3(t.position.x, t.position.y, z);
    }
}
#region  CFG METHOD
public class UpdateMethod : MonoBehaviour
{
    public System.Action actionUpdate = null;

    void Update()
    {
        actionUpdate?.Invoke();
    }
    public void AddUpdate(System.Action action)
    {
        actionUpdate = action;
    }
     public void AddAdapterUpdate(System.Action action)
    {
        actionUpdate += action;
    }
    public void ClearUpdate()
    {
        actionUpdate = null;
    }
}
public class OnDisableMethodOnce : MonoBehaviour
{
    public System.Action actionDisable = null;
    void OnDisable()
    {
        actionDisable?.Invoke();
        actionDisable = null;
    }
    public void AddDisable(System.Action action)
    {
        actionDisable += action;
    }
    public void ClearDisable()
    {
        actionDisable = null;
    }
}
public class OnEnableMethodOnce : MonoBehaviour
{
    public System.Action actionEnable = null;
    void OnEnable()
    {
        actionEnable?.Invoke();
        actionEnable = null;
        Destroy(this);
    }
    public void AddEnable(System.Action action)
    {
        actionEnable += action;
    }
    public void ClearEnable()
    {
        actionEnable = null;
    }
}
public class OnDestroyMethod : MonoBehaviour
{
    public System.Action actionDestroy;
    void OnDestroy()
    {
        actionDestroy?.Invoke();
        actionDestroy = null;
    }
    public void AddOnDestroy(System.Action action)
    {
        actionDestroy += action;        
    }
}
#endregion