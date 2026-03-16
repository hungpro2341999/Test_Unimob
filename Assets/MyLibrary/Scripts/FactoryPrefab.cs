using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
#region MY THREAD
public class MyThread : MonoBehaviour
{
    public static MyThread Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject thread = new GameObject("MyThread");
                _instance = thread.AddComponent<MyThread>();
            }
            return _instance;
        }
    }
    static MyThread _instance;
    System.Action action1 = null;
    System.Action action2 = null;
    System.Action action3 = null;
    System.Action actionUpdate = null;
    System.Action actionPassDay = null;
    public static System.Action actionGlobal;
    void Update()
    {
        actionGlobal?.Invoke();
        action1?.Invoke();
        action2?.Invoke();
        action3?.Invoke();
        actionUpdate?.Invoke();
    }

    public static void RegisterUpdateActionGlobal(System.Action action)
    {
        actionGlobal += action;
    }
    public static void UnRegisterUpdateActionGlobal(System.Action action)
    {
        actionGlobal -= action;
    }

    public void RegisterUpdateAction(System.Action action)
    {
        actionUpdate += action;
    }
    public void UnRegisterUpdateAction(System.Action action)
    {
        actionUpdate -= action;
    }
    public void AddDelayAction(float delay, System.Action actionEnd)
    {
        System.Action action = null;
        action = () =>
        {
            delay -= Time.deltaTime;
            if (delay < 0)
            {
                actionEnd?.Invoke();
                action1 -= action;
                action = null;
            }
        };
        action1 += action;
    }
    public void AddDelayFrame(int delayFarme, System.Action actionEnd)
    {
        System.Action action = null;
        delayFarme += 1;
        action = () =>
        {
            delayFarme--;
            if (delayFarme <= 0)
            {
                actionEnd?.Invoke();
                action1 -= action;
            }
        };
        action1 += action;
    }
}
#endregion

#region  ALL FACTORY
public class MyFactory
{
    public static void Clear()
    {
        allObj.Clear();
        allPooler.Clear();
    }
    static Dictionary<string, GameObject> allObj = new Dictionary<string, GameObject>();
    static string LayerVFX = "VFX";
    static GameObject parentFactory
    {
        get
        {
            if (_parentFactory == null)
            {
                GameObject obj = new GameObject("Factory");
                _parentFactory = obj;
            }
            return _parentFactory;
        }
    }
    static GameObject _parentFactory;
    public static GameObject InstantiatePrebVFX(string name, float delay, System.Action actionDone)
    {
        GameObject vfx = null;
        if (!allObj.ContainsKey(name))
        {
            var obj = Resources.Load<GameObject>("MyPreb/VFX/" + name);
            allObj.Add(name, obj);
        }
        vfx = GameObject.Instantiate(allObj[name], parentFactory.transform);
        MyThread.Instance.AddDelayAction(delay, () =>
        {
            GameObject.Destroy(vfx.gameObject);
            actionDone?.Invoke();
        });
        vfx.gameObject.SetActive(true);
        return vfx;
    }
    public static T InstantiatePreb<T>() where T : MonoBehaviour
    {
        T preb = null;
        string name = typeof(T).ToString();
        if (!allObj.ContainsKey(name))
        {
            var objjjj = Resources.Load<T>("MyPreb/Preb/" + name);
            allObj.Add(name, objjjj.gameObject);
        }
        preb = GameObject.Instantiate(allObj[name]).GetComponent<T>();
        preb.gameObject.SetActive(true);
        return preb;
    }
    public static GameObject InstantiatePreb(string namepreb, float lifeTime)
    {
        GameObject preb = null;
        string name = namepreb;
        if (!allObj.ContainsKey(name))
        {
            var objjjj = Resources.Load<GameObject>("MyPreb/Preb/" + name);
            allObj.Add(name, objjjj.gameObject);
        }
        preb = GameObject.Instantiate(allObj[name]);
        preb.gameObject.SetActive(true);
        MyThread.Instance.AddDelayAction(lifeTime, () =>
        {
            GameObject.Destroy(preb.gameObject);
        });
        return preb;
    }
    public static GameObject InstantiatePrebVFX(string namepreb)
    {
        GameObject preb = null;
        string name = namepreb;
        if (!allObj.ContainsKey(name))
        {
            var objjjj = Resources.Load<GameObject>("MyPreb/VFX/" + name);
            allObj.Add(name, objjjj.gameObject);
        }
        preb = GameObject.Instantiate(allObj[name]);
        preb.gameObject.SetActive(true);
        return preb;
    }
    public static GameObject InstantiatePreb(string namepreb)
    {
        GameObject preb = null;
        string name = namepreb;
        if (!allObj.ContainsKey(name))
        {
            var objjjj = Resources.Load<GameObject>("MyPreb/Preb/" + name);
            allObj.Add(name, objjjj.gameObject);
        }
        preb = GameObject.Instantiate(allObj[name]);
        preb.gameObject.SetActive(true);
        return preb;
    }
    public static void UnloadPreb(string namepreb)
    {
        var preb = allObj[namepreb];
        allObj.Remove(namepreb);
    }


    public static T InstantiateUIPreb<T>(Transform parent) where T : IPrebUI
    {
        T ui = default(T);
        string name = typeof(T).ToString();
        if (!allObj.ContainsKey(name))
        {
            var path = "MyPreb/UI/" + name;
            var obj = Resources.Load<GameObject>(path);
            allObj.Add(name, obj);
        }
        var objjjj = GameObject.Instantiate(allObj[name], parent);
        objjjj.GetComponent<T>().OnCreate();
        objjjj.gameObject.SetActive(true);
        ui = objjjj.gameObject.GetComponent<T>();
        return ui;
    }

    #region POOLER
    static Dictionary<string, Pooler> allPooler = new Dictionary<string, Pooler>();
    static string defaultPath = "MyPreb/Preb/";
    static string vfxPath = "MyPreb/VFX/";
    static void CreatePoolerDefaultWithName(string name, int count = 1)
    {
        string namePool = name;
        Pooler pooler = new PoolerGameObject();
        pooler.pathRs = defaultPath;
        pooler.CreatePool(name, count);
        allPooler.Add(namePool, pooler);
    }
    static void CreatePooler<T>(int count = 1)
    {
        string namePool = typeof(T).ToString();
        Pooler pooler = new PoolerGameObject();
        pooler.pathRs = defaultPath;
        pooler.CreatePool(namePool, count);
        allPooler.Add(namePool, pooler);
    }
    static void CreatePoolerDefaultWithName(string name, string path, int count = 1)
    {
        string namePool = name;
        Pooler pooler = new PoolerGameObject();
        pooler.pathRs = path;
        pooler.CreatePool(name, count);
        allPooler.Add(namePool, pooler);
    }
    public static T GetPoolerDefault<T>() where T : IPool
    {
        var namePool = typeof(T).ToString();
        if (!allPooler.ContainsKey(namePool))
        {
            CreatePooler<T>(1);
        }
        return allPooler[namePool].GetPool<T>();
    }
    public static DefaultPool GetPoolerDefault(string name)
    {
        string namePool = name;
        if (!allPooler.ContainsKey(namePool))
        {
            CreatePoolerDefaultWithName(namePool, 1);
        }
        return allPooler[namePool].GetPool<DefaultPool>();
    }
    public static DefaultPool GetPoolerDefaultVFX(string name, int lifeTime)
    {
        string namePool = name;
        if (!allPooler.ContainsKey(namePool))
        {
            CreatePoolerDefaultWithName(namePool, vfxPath, 3);
        }
        var vfx = allPooler[namePool].GetPool<DefaultPool>();
        MyThread.Instance.AddDelayAction(lifeTime, () =>
        {
            ReturnToPoolerDefault(vfx);
        });
        return vfx;
    }
    public static void ReturnToPoolerDefault(DefaultPool obj)
    {
        if (allPooler.ContainsKey(obj.namePool))
        {
            ((DefaultPool)obj).transform.parent = (((PoolerGameObject)allPooler[obj.namePool]).parent);
            allPooler[obj.namePool].ReturnToPool(obj);
        }

    }
    public static void ReturnToPoolerDefault(IPool obj)
    {
        if (allPooler.ContainsKey(obj.GetType().ToString()))
            allPooler[obj.GetType().ToString()].ReturnToPool(obj);
    }
    #endregion
}

public abstract class Pooler
{
    public string pathRs;
    protected string namePool;
    List<IPool> allObj = new List<IPool>();
    IPool poolObj;
    public void CreatePool(string name, int count = 5)
    {
        namePool = name;
        for (int i = 0; i < count; i++)
        {
            var obj = CreateObj<IPool>();
            allObj.Add(obj);
        }
    }
    public void CreatePool<T>(int count = 5) where T : IPool
    {
        namePool = typeof(T).ToString();
        for (int i = 0; i < count; i++)
        {
            CreateObj<T>();
        }
    }

    public T GetPool<T>() where T : IPool
    {
        if (!allObj[0].IsDestroy())
        {
            CreateObj<T>();
        }
        var obj = allObj[0];
        allObj.RemoveAt(0);
        obj.OnActive();
        allObj.Add(obj);
        return (T)obj;
    }
    public virtual void ReturnToPool(IPool obj)
    {
        obj.OnUnActive();
        allObj.Remove(obj);
        allObj.Insert(0, obj);
    }
    protected virtual T CreateObj<T>() where T : IPool
    {
        T t = OnCreate<T>();
        t.OnCreate();
        t.OnUnActive();
        allObj.Insert(0, t);
        return t;
    }
    public abstract T OnCreate<T>() where T : IPool;
}

public class PoolerGameObject : Pooler
{
    protected GameObject objPool;
    public Transform parent
    {
        get
        {
            if (_parent == null)
            {
                GameObject obj = new GameObject("Pooler_" + namePool);
                _parent = obj.transform;
            }
            return _parent;
        }
    }
    Transform _parent;
    public override T OnCreate<T>()
    {
        if (objPool == null)
        {
            var path = this.pathRs + namePool;
            objPool = Resources.Load<GameObject>(path);
        }
        var objjjj = GameObject.Instantiate(objPool);
        var defaultPool = objjjj.GetComponent<DefaultPool>();
        if (defaultPool == null)
        {
            defaultPool = objjjj.AddComponent<DefaultPool>();
        }
        defaultPool.namePool = namePool;
        objjjj.transform.SetParent(parent);
        return objjjj.GetComponent<T>();
    }
}
public interface IPool
{
    void OnCreate();
    void OnActive();
    void OnUnActive();
    bool IsDestroy();

}
public interface IPrebUI
{
    void OnCreate();
}
#endregion