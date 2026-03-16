using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ISceneLocalData
{
    
}
public enum TypeAction
{
   UpdateUI,CloseStore,OpenStore,OpenStoreStartDay,TimeCloseStore,EndDay,SkipNextDay,TimeWaitEndDay,UpdatePrice,
   EnterCash,LeaveCash,
   UpdateCash,DonePayClient,
   InteractObj,UnInteractObj,
   ButtonSelect_1,ButtonSelect_2,ButtonSelect_3,ButtonSelect_4
}
public enum TypeActionGlobal { UpdateSound }
public partial class GameAction : MonoBehaviour
{
    public static GameAction Instance
    {
        get
        {
            if(!_gameAciton)
            {
                _gameAciton = new GameObject("GameAction").AddComponent<GameAction>();
                DontDestroyOnLoad(_gameAciton.gameObject);
            }
            return _gameAciton;
        }
    }
    
    void OnDisable()
    {
      
    }
    static GameAction _gameAciton;    
    #region  REGISTER ACTION
    Dictionary<TypeAction, System.Action> ALL_ACTION = new Dictionary<TypeAction, System.Action>();
    public void RegisterAction(TypeAction type,System.Action actionRegister)
    {
        if(!ALL_ACTION.ContainsKey(type))
        {
            System.Action action = null;
            action = ()=>
            {

            };
            Debug.Log("Create Action "+type.ToString());
            ALL_ACTION.Add(type,action);
        }
        ALL_ACTION[type]+=actionRegister;    
    }
    public void UnRegisterAction(TypeAction type,System.Action actionRegister)
    {
        if(!ALL_ACTION.ContainsKey(type))
        {
            System.Action action = null;
            action = ()=>
            {
            };
            Debug.Log("Create Action "+type.ToString());
            ALL_ACTION.Add(type,action);
        }
        ALL_ACTION[type]-=actionRegister;    
    }
    public void Clear(TypeAction type)
    {
        ALL_ACTION[type] = null;
    }
    public void Clear()
    {
        ALL_ACTION.Clear();
    }
    public void InvokeAction(TypeAction type)
    {
        Debug.Log("Invoke Action : "+type.ToString());
        if(ALL_ACTION.ContainsKey(type))
              ALL_ACTION[type]?.Invoke();
    }
    #endregion
    #region  REGISTER ACTION <T>
    #endregion
    #region REGISTER ACTION GLOBAL
    static Dictionary<TypeActionGlobal, System.Action> ALL_ACTION_GLOBAL = new Dictionary<TypeActionGlobal, System.Action>();
    public static void RegisterActionGlobal(TypeActionGlobal type, System.Action actionRegister)
    {
        if (!ALL_ACTION_GLOBAL.ContainsKey(type))
        {
            System.Action action = null;
            action = () =>
            {

            };
            Debug.Log("Create Action " + type.ToString());
            ALL_ACTION_GLOBAL.Add(type, action);
        }
        ALL_ACTION_GLOBAL[type] += actionRegister;
    }
    public static void InvokeActionGlobal(TypeActionGlobal typeAction)
    {
        Debug.Log("Invoke Action Global : " + typeAction.ToString());
        ALL_ACTION_GLOBAL[typeAction]?.Invoke();
    }
    #endregion
}

public class GameAction<T> 
{   
    #region  REGISTER ACTION
    System.Action<T> ALL_ACTION = null;
    public void RegisterAction(System.Action<T> actionRegister)
    {
        ALL_ACTION+=actionRegister;
    }
    public void UnRegisterAction(System.Action<T> actionRegister)
    {
        ALL_ACTION-=actionRegister;
    }
    public void Clear()
    {
        ALL_ACTION = null;
    }
    public void InvokeAction(T obj)
    {
        ALL_ACTION?.Invoke(obj);
    }
    #endregion
}