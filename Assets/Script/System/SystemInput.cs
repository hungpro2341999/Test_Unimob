using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;

public class SystemInput : SingletonX<SystemInput>
{
    System.Action actionUpgrade;
    void Start()
    {
        EventBus.Subscribe<Box.EventCickBox>((evt) =>
        {
            ShowInforBox(evt.@object);
        });
        EventBus.Subscribe<Tree.EventCickTree>((evt) =>
        {
            ShowViewLevelUpTree(evt.@object);
        });      
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI())
                return;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == 6)
                {
                    var infor = hit.collider.GetComponentInParent<IShowInfor>();
                    infor.ShowInfor();
                }
            }
        }
        actionUpgrade?.Invoke();
    }
    bool IsPointerOverUI()
    {
        if (Input.touchCount > 0)
        {
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }
        return EventSystem.current.IsPointerOverGameObject();
    }
    void ShowInforBox(Box box)
    {
        var dataResouceUnlock = DataConfigUpgrade.Instance.GetDataResourceUnlock(box.Index);
        box.price = dataResouceUnlock.priceUnlock;
        gameObject.ClearUdateMethod();
        UIViewUnlock uiViewUnlock = null;
        ViewUI.Instance.Show<UIViewUnlock>(out uiViewUnlock, null, Menu<ViewUI>.ShowType.NotHide);
        uiViewUnlock.ShowInfor(dataResouceUnlock, () =>
        {
            if (dataResouceUnlock.priceUnlock > GameData.GetData<DataPlayer>().money)
            {
                ToastUI.Instance.ShowToast("Not enough money to unlock Tree " + dataResouceUnlock.ID);
                return;
            }
            EventBus.Run(new EventChangeMoney() { changeMoney = -dataResouceUnlock.priceUnlock });
            box.OpenBox(true);
            AddDelayOpenBox(box);
            ViewUI.Instance.Hide<UIViewUnlock>();
        });
        uiViewUnlock.transform.FindChildByName("Frame").transform.position = Camera.main.WorldToScreenPoint(box.transform.position);

    }
    void ShowViewLevelUpTree(Tree tree)
    {
        UILevelUp uiViewLevelUp = null;
        ViewUI.Instance.Show<UILevelUp>(out uiViewLevelUp, null, Menu<ViewUI>.ShowType.NotHide);
        uiViewLevelUp.ShowInfor(tree);
    }
    void AddDelayOpenBox(Box box)
    {
        box.isInteract = false;
        var UIIShowProcess = MyFactory.InstantiatePreb("UIIShowProcess").GetComponent<UIIShowProcess>();
        UIIShowProcess.transform.SetParent(ScreenUI.Instance.GetPanelByName<ScreenGamePlay>().transform);
        UIIShowProcess.transform.localScale = Vector3.one;
        UIIShowProcess.transform.position = Camera.main.WorldToScreenPoint(box.transform.position)+Vector3.up*100;
        System.Action action = null;
        float tTime = DataConfigUpgrade.Instance.GetDataResourceUnlock(box.Index).timeUnlock;
        float t = tTime;
        float process = 0;
        action = () =>
        {
            t -= Time.deltaTime;
            if (t > 0)
            {
                process = (t / tTime);
                UIIShowProcess.txtTime.text = t.ToString("F1") + "s";
                UIIShowProcess.sliderProcess.value = 1 - (t / tTime);               
                UIIShowProcess.sliderProcess.value = process;               
            }
            else if (t <= 0)
            {
                SpawnTreeAt(box);
                GameObject.Destroy(UIIShowProcess.gameObject);                    
                GameObject.Destroy(box.gameObject);
                actionUpgrade -= action;
            }

        };
        actionUpgrade += action;
    }
    void SpawnTreeAt(Box box)
    {
        var tree = MyFactory.InstantiatePreb("Tree").GetComponent<Tree>();
        tree.Index = box.Index;
        tree.transform.position = box.transform.position;
        tree.Init();
        EventBus.Run(new EventOpenTree() { tree = tree });
    }
}
