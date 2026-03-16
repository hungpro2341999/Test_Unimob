using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Employer : Person
{
    public BigNumber moneyClaim;
    override public void Init()
    {
        base.Init();
        Status = TypeStatus.Ready;
    }
    public void DoTakeRs(Tree tree)
    {
        float delayPerItem = 0.1f;
        var rs = tree.listRs;
        for (int i = 0; i < rs.Count; i++)
        {
            var item = rs[i];
            item.transform.SetParent(posCarry);
            item.transform.DOLocalMove(Vector3.zero + listItemCarry.Count * Vector3.up * 0.2f, 0.25f)
            .SetEase(Ease.OutBack).SetDelay(i * delayPerItem);
            item.transform.localRotation = Quaternion.identity;
            listItemCarry.Add(item);
        }        
        PlayAnim(TypeAnim.CarryIdle);
        MyThread.Instance.AddDelayAction(rs.Count * delayPerItem,() =>
        {
            EventBus.Run(new EventGiveToClient() { employer = this, listItemGive = listItemCarry });
        });
        tree.listRs.Clear();
    }
    public void DoGiveToCient(Client client,System.Action actionDone = null)
    {
        float delayPerItem = 0.1f;
        for (int i = 0; i < listItemCarry.Count; i++)
        {
            var item = listItemCarry[i];
            item.transform.SetParent(client.posCarry);
            item.transform.DOLocalMove(Vector3.zero + client.listItemCarry.Count * Vector3.up * 0.2f, 0.25f)
            .SetEase(Ease.OutBack).SetDelay(i * delayPerItem);
            item.transform.localRotation = Quaternion.identity;
            client.listItemCarry.Add(item);
        }              
        MyThread.Instance.AddDelayAction(client.listItemCarry.Count * delayPerItem, () =>
        {
            MyFactory.InstantiatePreb("EffPay", 3).transform.position = client.posCarry.position ;
            actionDone?.Invoke();
        });
        listItemCarry.Clear();
    }
}       