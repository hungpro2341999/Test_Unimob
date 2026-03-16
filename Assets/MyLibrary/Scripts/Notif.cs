using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Notif : MonoBehaviour
{
    public static Notif Instance
    {
        get
        {
            if(!_instance)
            {
                _instance = Instantiate(Resources.Load<Notif>("Notif"));
                _instance.Init();
            }
            return _instance;
        }
    }
    static Notif _instance;
    Transform notif;
    int index = 0;
    public void Init()
    {
        notif = transform.FindChildByName("ObjNotif");
        notif.gameObject.SetActive(false);
        DontDestroyOnLoad(this.gameObject);
    }
    public void Show(string s)
    {
        if(index>3)
        {
            return;
        }
        index++;
        var obj = Instantiate(notif, transform);
        obj.transform.localScale = Vector3.one;
        obj.gameObject.SetActive(true);
        var text = obj.transform.FindChildByName("txt").GetComponent<Text>();
        text.text = s;
        MyThread.Instance.AddDelayAction(1,
            () 
            =>
            {
                index--;
                GameObject.Destroy(obj.gameObject);
            });
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Show("Please Select Color first");
        }
    }
}
