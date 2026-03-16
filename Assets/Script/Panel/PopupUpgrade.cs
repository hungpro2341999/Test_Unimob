using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PopupUpgrade : Panel
{
    public Transform parentContent;
    protected override void Start()
    {
        base.Start();
        transform.FindChildByName("Close").GetComponent<Button>().onClick.AddListener(() =>
        {
            PopupUI.Instance.Hide<PopupUpgrade>();
        });
        EventBus.Subscribe<EventUpgrade>((evt) =>
        {
            Init(evt.listUpgrade);
        });
    }
    public void Init(List<AbastractUpgrade> listUpgrade)
    {
        foreach(Transform item in parentContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in listUpgrade)
        {
            if(item.IsMaxLevel())
            {
                continue;
            }
            var uiUpgrade = MyFactory.InstantiatePreb("UIUpgrade").GetComponent<UIUpgrade>();
            uiUpgrade.transform.SetParent(parentContent);
            uiUpgrade.ShowInfor(item);
            uiUpgrade.transform.localScale = Vector3.one;
        }
    }    
}
public struct EventUpgrade
{
    public List<AbastractUpgrade> listUpgrade;
}
