using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SystemActionStore : SingletonX<SystemActionStore>
{
    System.Action actionWait = null;
    Transform posStart;
    Transform posEnd;
    Transform posWait;
    SystemStore systemStore;
    public List<Employer> listEmployerWait = new List<Employer>();

    void Start()
    {
        systemStore = GetComponent<SystemStore>();
        posWait = transform.FindChildByName("posWait");
        posStart = transform.FindChildByName("posStart");
        posEnd = transform.FindChildByName("posEnd");
        InitAction();
    }

    void InitAction()
    {
        EventBus.Subscribe<EventTreeFull>((evt) =>
        {
            var tree = evt.tree;
            var employer = GetEmployerReady();
            System.Action action = null;
            action = () =>
            {
                employer = GetEmployerReady();
                if (employer != null)
                {
                    EmployerTakeRsFromTree(employer, tree);
                    actionWait -= action;
                }
            };
            actionWait += action;

        });
        EventBus.Subscribe<EventGiveToClient>((evt) =>
        {
            var client = GetClientReady();
            System.Action action = null;
            if (client == null)
            {
                AddToWait(evt.employer);
                return;
            }
            action = () =>
            {
                client = GetClientReady();
                if (client != null)
                {
                    RemoveWait(evt.employer);
                    EmployerGiveToClient(evt.employer, client);
                    actionWait -= action;
                }
            };
            actionWait += action;
        });

        EventBus.Subscribe<EventChangeMoney>((evt) =>
        {
            var dataPlayer = GameData.GetData<DataPlayer>();
            dataPlayer.money += evt.changeMoney;
            EventBus.Run<EventUpdateMoney>(new EventUpdateMoney { dataPlayer = dataPlayer });
        });       
    }

    public void AddToWait(Employer employer)
    {
        listEmployerWait.Add(employer);
      
    }
    void RemoveWait(Employer employer)
    {
        listEmployerWait.Remove(employer);
    }
    void EmployerTakeRsFromTree(Employer employer, Tree tree)
    {
        employer.moneyClaim = tree.profit.Value;
        employer.Status = Person.TypeStatus.Waiting;
        employer.DoMove(tree.posTarget.position, () =>
        {
            employer.DoTakeRs(tree);
        });
    }
    void EmployerGiveToClient(Employer employer, Client client)
    {
        client.Status = Person.TypeStatus.Waiting;
        employer.DoMove(client.transform.position - Vector3.forward * 4, () =>
        {
            employer.DoGiveToCient(client, () =>
            {
               
                MyThread.Instance.AddDelayAction(0.25f, () =>
                {
                    // Payment
                    EventBus.Run<EventChangeMoney>(new EventChangeMoney { changeMoney = employer.moneyClaim });
                    client.DoGetItemDone();
                    employer.Status = Person.TypeStatus.Ready;
                    //AddToWait(employer);
                    client.DoMove(posEnd.position, () =>
                    {
                        client.gameObject.SetActive(false);
                        systemStore.All_Client.Remove(client);
                    });
                });
            });
        });
    }
    void Update()
    {
        actionWait?.Invoke();
    }
    Employer GetEmployerReady()
    {
        for (int i = 0; i < systemStore.All_Employer.Count; i++)
        {
            var employer = systemStore.All_Employer[i];
            if (employer.Status == Person.TypeStatus.Ready)
            {
                return employer;
            }
        }
        return null;
    }
    Client GetClientReady()
    {
        for (int i = 0; i < systemStore.All_Client.Count; i++)
        {
            var client = systemStore.All_Client[i];
            if (client.Status == Person.TypeStatus.Ready)
            {
                return client;
            }
        }
        return null;
    }
}
