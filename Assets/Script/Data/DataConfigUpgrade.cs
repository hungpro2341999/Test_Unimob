using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "DataConfigUpgrade", menuName = "Data/DataConfigUpgrade")]

public class DataConfigUpgrade : ScriptableObject
{
    static DataConfigUpgrade _data = null;
    public static DataConfigUpgrade Instance
    {
        get
        {
            if (_data == null)
            {
                _data = Resources.Load<DataConfigUpgrade>("MyPreb/Data/DataConfigUpgrade");
            }
            return _data;
        }
    }
    public List<ResoucesDefine> listResoucesDefine;
    public List<DataLevelUpTree> dataLevelUpTrees;
    public List<DataCfgUpgrade> listDataUpgrade;
    public List<DataResouceUnlock> dataTree;
    public ResoucesDefine GetResoucesDefine(int ID)
    {
        for (int i = 0; i < listResoucesDefine.Count; i++)
        {
            if (listResoucesDefine[i].ID == ID)
            {
                return listResoucesDefine[i];
            }
        }
        return null;
    }
    public DataCfgUpgrade GetDataUpgrade(string nameUpgrade)
    {
        for (int i = 0; i < listDataUpgrade.Count; i++)
        {
            if (listDataUpgrade[i].nameUpgrade == nameUpgrade)
            {
                return listDataUpgrade[i];
            }
        }
        return null;
    }
    public DataResouceUnlock GetDataResourceUnlock(int ID)
    {
        for (int i = 0; i < dataTree.Count; i++)
        {
            if (dataTree[i].ID == ID)
            {
                return dataTree[i];
            }
        }
        return null;
    }
    public DataLevelUpTree GetDataLevelUpTrees(int ID)
    {
        for (int i = 0; i < dataLevelUpTrees.Count; i++)
        {
            if (dataLevelUpTrees[i].ID == ID)
            {
                return dataLevelUpTrees[i];
            }
        }
        return null;
    }
    
}
[System.Serializable]
public class DataCfgUpgrade
{   
    public string nameUpgrade;
    public float[] cfgUpgrade;
    public BigNumber[] cfgUpgradePrice;
    public string description;
    public Sprite spr;
}
[System.Serializable]
public class DataResouceUnlock
{
    public int ID;
    public Sprite icon;
    public BigNumber priceUnlock;
    public string des;
    public float timeUnlock = 5;
}
[System.Serializable]
public class DataLevelUpTree
{
    public int ID;
    public int levelMax =0;
    public BigNumber[] income;
    public float[] speed;
    public BigNumber[] priceUpgrade;
}
[System.Serializable]
public class ResoucesDefine
{
    public int ID;
    public string name;
    public Sprite image;
}