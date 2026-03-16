using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SystemUpgrade : SingletonX<SystemUpgrade>
{
    public DataConfigUpgrade dataConfigUpgrade;
    public List<AbastractUpgrade> All_Upgrade
    {
        get
        {
            return listUpgrade;
        }
    }
    public List<AbastractUpgrade> listUpgrade;
    void Start()
    {
        dataConfigUpgrade = DataConfigUpgrade.Instance;
        listUpgrade = new List<AbastractUpgrade>();      
        AddUpgrade(new UpgradeAmountClient(1));
        AddUpgrade(new UpgradeAmountClient(2));
        AddUpgrade(new UpgradeAmountClient(3));
        AddUpgrade(new UpgradeAmountEmployer(1));
        AddUpgrade(new UpgradeAmountEmployer(2));
        AddUpgrade(new UpgradeAmountEmployer(3));

        bool first = true;
        EventBus.Subscribe<EventOpenTree>((evt) =>
        {
          
            if(first)
            {
                first = false;
                AddUpgrade(new UpgradeProfitTree());
            }
            var tree = evt.tree;
            AddUpgrade(new UpgradeX2Profit(tree.Index));
            AddUpgrade(new UpgradeX3Profit(tree.Index));

            var uiShow = MyFactory.InstantiatePreb("UIInforTree").GetComponent<UIInforTree>();
            uiShow.transform.SetParent(ScreenUI.Instance.GetPanelByName<ScreenGamePlay>().transform);
            uiShow.transform.localScale = Vector3.one;
            uiShow.transform.position = Camera.main.WorldToScreenPoint(tree.transform.position) + Vector3.up * 250;
            uiShow.Load(tree);
            var vfx = MyFactory.InstantiatePreb("EffBuildDone", 3);
            vfx.transform.position = tree.transform.position+Vector3.up*1;
            
        });
    }
    public void AddUpgrade<T>(T t) where T : AbastractUpgrade
    {
        listUpgrade.Add(t);
    }
}
#region  Upgrade Store

[System.Serializable]
public class AbastractUpgrade
{
    public DataCfgUpgrade dataCfgUpgrade;
    public int level;
    public float ValueCurrent
    {
        get
        {
            if (IsMaxLevel())
            {
                return dataCfgUpgrade.cfgUpgrade[dataCfgUpgrade.cfgUpgrade.Length - 1];
            }
            return dataCfgUpgrade.cfgUpgrade[level];
        }
    }
    public BigNumber PriceCurrent
    {
        get
        {
            if (IsMaxLevel())
            {
                return dataCfgUpgrade.cfgUpgradePrice[dataCfgUpgrade.cfgUpgradePrice.Length - 1];
            }
            return dataCfgUpgrade.cfgUpgradePrice[level];
        }
    }
    public AbastractUpgrade()
    {
        dataCfgUpgrade = DataConfigUpgrade.Instance.GetDataUpgrade(GetKey());
    }
    public bool IsMaxLevel()
    {
        if (level >= dataCfgUpgrade.cfgUpgrade.Length - 1)
        {
            return true;
        }
        return false;
    }
    public void LevelUp()
    {
        if (IsMaxLevel())
        {
            Debug.Log("Upgrade " + dataCfgUpgrade.nameUpgrade + " is max level");
            return;
        }
        if (GameData.GetData<DataPlayer>().money < PriceCurrent)
        {
            ToastUI.Instance.ShowToast("Not enough money to upgrade ");
            return;
        }
        EventBus.Run(new EventChangeMoney() { changeMoney = -PriceCurrent });
        level++;
        Debug.Log("Level up " + dataCfgUpgrade.nameUpgrade + " to level " + level);
        OnLevelUp();
        EventBus.Run<EventUpgrade>(new EventUpgrade { listUpgrade = SystemUpgrade.Instance.All_Upgrade });
    }
    public virtual void OnLevelUp()
    {

    }
    public virtual string GetKey()
    {
        return GetType().Name;
    }
}
public class UpgradeProfitTree : AbastractUpgrade
{
    Modifier modifierProfit;
    public UpgradeProfitTree() : base()
    {
        modifierProfit = new Modifier(ModifierType.Multiply, new BigNumber(1, 0));
        EventBus.Subscribe<EventOpenTree>((evt) =>
        {   
            evt.tree.profit.AddModifier(modifierProfit);
            MyThread.Instance.AddDelayAction(0.1f, () =>
            {
                 EventBus.Run(new Tree.EventUpgradeTree() { IDTree = evt.tree.Index });
            });           
        });
        var allTree = SystemStore.Instance.All_Tree;
        for (int i = 0; i < allTree.Count; i++)
        {
            modifierProfit.Value = new BigNumber(ValueCurrent, 0);
            allTree[i].profit.AddModifier(modifierProfit);
        }
    }

    public override void OnLevelUp()
    {
        var allTree = SystemStore.Instance.All_Tree;
        for (int i = 0; i < allTree.Count; i++)
        {
            modifierProfit.Value = new BigNumber(ValueCurrent, 0);
            int index = i;
            EventBus.Run(new Tree.EventUpgradeTree() { IDTree = allTree[index].Index });
        }

    }
}
public class UpgradeX2Profit : AbastractUpgrade
{
    public int indexTree;
    Modifier modifierProfit;
    public UpgradeX2Profit(int indexTree)
    {
        this.indexTree = indexTree;
        dataCfgUpgrade = DataConfigUpgrade.Instance.GetDataUpgrade(GetKey());
        modifierProfit = new Modifier(ModifierType.Multiply, new BigNumber(1, 0));
        SystemStore.Instance.GetTreeByID(indexTree).profit.AddModifier(modifierProfit);
    }

    public override void OnLevelUp()
    {
        modifierProfit.Value = new BigNumber(ValueCurrent, 0);
        EventBus.Run(new Tree.EventUpgradeTree() { IDTree = indexTree });
    }
    override public string GetKey()
    {
        return GetType().Name + "_" + indexTree;
    }
}
public class UpgradeX3Profit : AbastractUpgrade
{
    public int indexTree;
    Modifier modifierProfit;
    public UpgradeX3Profit(int indexTree)
    {
        this.indexTree = indexTree;
        dataCfgUpgrade = DataConfigUpgrade.Instance.GetDataUpgrade(GetKey());
        modifierProfit = new Modifier(ModifierType.Multiply, new BigNumber(1, 0));
        SystemStore.Instance.GetTreeByID(indexTree).profit.AddModifier(modifierProfit);
    }

    public override void OnLevelUp()
    {
        modifierProfit.Value = new BigNumber(ValueCurrent, 0);
        EventBus.Run(new Tree.EventUpgradeTree() { IDTree = indexTree });
    }
    override public string GetKey()
    {
        return GetType().Name + "_" + indexTree;
    }
}
public class UpgradeAmountClient : AbastractUpgrade
{
    public int amount;
    public UpgradeAmountClient(int amount) : base()
    {
        this.amount = amount;
        dataCfgUpgrade = DataConfigUpgrade.Instance.GetDataUpgrade(GetKey());
    }

    public override void OnLevelUp()
    {
        SystemStore.Instance.MAX_CLIENT += amount;
    }
    override public string GetKey()
    {
        return "Client+" + amount;
    }
}
public class UpgradeAmountEmployer : AbastractUpgrade
{
    public int amount;
    public UpgradeAmountEmployer(int amount) : base()
    {
        this.amount = amount;
        dataCfgUpgrade = DataConfigUpgrade.Instance.GetDataUpgrade(GetKey());
    }

    public override void OnLevelUp()
    {
        for (int i = 0; i < amount; i++)
        SystemStore.Instance.SpawnEmployer();
    }
    override public string GetKey()
    {
        return "Employer+" + amount;
    }
}
#endregion
