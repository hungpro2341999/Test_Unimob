using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer : Data<DataPlayer>
{
    public BigNumber money = new BigNumber(0, 0);
    public int ruby = 0;
    public override void Call()
    {
        base.Call();
        money = new BigNumber(1, 5);
        ruby = 9999;
    }
    public void AddMoney(BigNumber amount)
    {
        money += amount;
        EventBus.Run(new EventUpdateMoney { dataPlayer = this });
    }
   
}
public struct EventMoneyChange
{
    public BigNumber amount;
}
