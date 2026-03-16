using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static DataConfigUpgrade;

public class Tree : MonoBehaviour, IShowInfor
{
    public int Index = 0;
    public BigStat profit;
    public Transform posTarget = null;
    public List<GameObject> listRs;
    public int AmountRs
    {
        get
        {
            return listRs.Count;
        }
    }
    public float SpeedSpawnRs
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }
    public bool IsFullRs
    {
        get
        {
            return listRs.Count >= MAX_RS;
        }
    }
    public UpgradeTree Upgrade_Tree
    {
        get
        {
            return m_upgradeTree;
        }
    }
    [SerializeField]
    private List<Transform> listPosSpawnRs;

    int MAX_RS = 3;
    public float speed = 1;
    UpgradeTree m_upgradeTree;
    System.Action actionUpdate = null;

    public void Init()
    {
        profit = new BigStat(new BigNumber(1, 0));
        listPosSpawnRs = new List<Transform>();
        var rootSpawn = transform.FindChildByName("Tomato");
        for (int i = 0; i < rootSpawn.childCount; i++)
        {
            listPosSpawnRs.Add(rootSpawn.GetChild(i));
        }
        MAX_RS = listPosSpawnRs.Count;
        this.m_upgradeTree =  new UpgradeTree(this,Index);
        DoUpdateSpawnRs();
        
    }
    public void DoUpdate()
    {
        actionUpdate?.Invoke();
    }

    void DoUpdateSpawnRs()
    {
        float t = 0;
        actionUpdate = () =>
        {
            if (IsFullRs)
            {
                return;
            }
            t += Time.deltaTime;
            if (t >= SpeedSpawnRs)
            {
                t = 0;
                var tomato = GetTomato();
                tomato.transform.position = listPosSpawnRs[AmountRs].position;
                tomato.transform.SetParent(listPosSpawnRs[AmountRs]);
                tomato.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBack);
                listRs.Add(tomato);
                if (IsFullRs)
                {
                    MyThread.Instance.AddDelayAction(0.25f, () =>
                    {
                        EventBus.Run(new EventTreeFull() { tree = this });

                    });
                    return;
                }
            }
        };
    }

    GameObject GetTomato()
    {
        var tomato = MyFactory.GetPoolerDefault("Tomato");
        return tomato.gameObject;
    }

    public void ShowInfor()
    {        
        EventBus.Run(new EventCickTree() { @object = this });
    }
    

#region Upgrade Tree
[System.Serializable]
public class UpgradeTree
{
    public UpgradeTree(Tree tree, int ID)
    {
        dataLevelUpTree = DataConfigUpgrade.Instance.GetDataLevelUpTrees(ID);
        level = 0;
        this.tree = tree;
        tree.SpeedSpawnRs = ValueSpeed;
        tree.profit.BaseValue = ValueIncome;
    }
    public Tree tree;
    public DataLevelUpTree dataLevelUpTree;
    public int level;
    public BigNumber ValueIncome
    {
        get
        {
            return dataLevelUpTree.income[level];
        }
    }
    public float ValueSpeed
    {
        get
        {
            return dataLevelUpTree.speed[level];
        }
    }
    public BigNumber ValuePriceUpgrade
    {
        get
        {
            return dataLevelUpTree.priceUpgrade[level];
        }
    }
    public bool IsMaxLevel()
    {
        if (level >= dataLevelUpTree.levelMax-1)
        {
            return true;
        }
        return false;
    }
    public void LevelUp()
    {
        if (IsMaxLevel())
        {
          
            ToastUI.Instance.ShowToast("Tree " + dataLevelUpTree.ID + " is max level ");
            return;
        }
        if(GameData.GetData<DataPlayer>().money < ValuePriceUpgrade)
        {
            ToastUI.Instance.ShowToast("Not enough money to upgrade Tree " + dataLevelUpTree.ID);
            return;
        }
        EventBus.Run(new EventChangeMoney() { changeMoney = -ValuePriceUpgrade });
        level++;
        Debug.Log("Level up Tree " + dataLevelUpTree.ID + " to level " + level);
        tree.SpeedSpawnRs = ValueSpeed;
        tree.profit.BaseValue = ValueIncome;
        EventBus.Run(new EventUpgradeTree() { IDTree = dataLevelUpTree.ID});
       
    }
}
public struct EventUpgradeTree
{
    public int IDTree;
}
public struct EventCickTree
{
    public Tree @object;
}
#endregion
}
