using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInforTree : MonoBehaviour
{
    public Image iconTree;
    public TMPro.TextMeshProUGUI txtProfit;
    public TMPro.TextMeshProUGUI txtSpeed;
    Tree tree;
    void Start()
    {
        EventBus.Subscribe<Tree.EventUpgradeTree>((ID) =>
        {
            if (ID.IDTree == tree.Index)
            {
                Load(tree);
            }
        });

    }
    public void Load(Tree tree)
    {
        this.tree = tree;
        var defineRs = DataConfigUpgrade.Instance.GetResoucesDefine(tree.Index);
        iconTree.sprite = defineRs.image;
        txtProfit.text =  BigNumberFormatter.Format(tree.profit.Value);
        var devidedPrice = tree.profit.Value / new BigNumber(3, 0);
        txtSpeed.text = tree.SpeedSpawnRs.ToString() + "s" + "/" + BigNumberFormatter.Format(devidedPrice);
    }
}
