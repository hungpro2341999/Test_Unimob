using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : Person
{
    System.Action actionClientGetItemDone = null;
    public void AddActionClientGetItemDone(System.Action action)
    {
        actionClientGetItemDone += action;
    }
    public void DoGetItemDone()
    {
        actionClientGetItemDone?.Invoke();
    }
}
