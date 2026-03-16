using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPool :MonoBehaviour, IPool
{
    public string namePool;
    System.Action actionLife = null;
    public bool IsDestroy()
    {
        return !gameObject.activeSelf;
    }

    public virtual void OnActive()
    {
        gameObject.SetActive(true);
    }

    public void OnCreate()
    {
        
    }
    public virtual void OnUnActive()
    {
        gameObject.SetActive(false);
        actionLife = null;
    }
    public void DoDetroy()
    {
        
        Debug.Log("Return to pooler: " + namePool);
        MyFactory.ReturnToPoolerDefault(this);
    }
    protected virtual void DoUpdate()
    {
        
    }
    void Update()
    {
        DoUpdate();
    }
    void FixedUpdate()
    {
        actionLife?.Invoke();
    }
    public DefaultPool AddTimeLife(float time)
    {
        actionLife = () =>
        {
            time -= Time.fixedDeltaTime;
            if (time <= 0)
            {
                DoDetroy();
            }
        };
        return this;
    }
}
