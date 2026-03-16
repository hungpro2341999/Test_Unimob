using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : SingletonX<GameController>
{
   public Transform target_1;
   void Start()
   {

      Application.targetFrameRate = 60;
      EventBus.Subscribe<EventTreeFull>((cmd) =>
      {
         Debug.Log("CommandTreeFull received");
      });
      ScreenUI.Instance.Show<ScreenGamePlay>(null,Menu<ScreenUI>.ShowType.NotHide);
      
   }
   BigStat money = new BigStat(new BigNumber(100, 0));
   void Update()
   {

      if (Input.GetKeyDown(KeyCode.Space))
      {
        ToastUI.Instance.ShowToast("Money");
      }

   }

}


public class EventBus
{
   static Dictionary<string, System.Action<object>> dictCommand = new Dictionary<string, System.Action<object>>();

   public static void Subscribe<T>(System.Action<T> action) where T : struct
   {
      Debug.Log("Subscribe to event: " + typeof(T).Name);
      if (dictCommand.ContainsKey(typeof(T).Name))
      {
         dictCommand[typeof(T).Name] += (obj => action((T)obj));
      }
      else
      {
         dictCommand.Add(typeof(T).Name, (obj) => action((T)obj));
      }
   }
   public static void SubscribeTypeGeneric<T>(System.Action<T> action) where T : struct
   {
      var typeName = typeof(T).Name;
      Debug.Log("Subscribe to event: " + typeof(T).Name);
      if (dictCommand.ContainsKey(typeof(T).Name))
      {
         dictCommand[typeof(T).Name] += (obj => action((T)obj));
      }
      else
      {
         dictCommand.Add(typeof(T).Name, (obj) => action((T)obj));
      }
   }

   public static void Run<T>(T command) where T : struct
   {
      if (dictCommand.ContainsKey(typeof(T).Name))
      {
         dictCommand[typeof(T).Name]?.Invoke(command);
      }
   }
}


public struct EventTreeFull
{
   public Tree tree;
}
public struct EventGiveToClient
{
   public Employer employer;
   public List<GameObject> listItemGive;
}