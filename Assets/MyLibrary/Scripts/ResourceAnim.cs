using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum TypeResources
{
    Like
    , Undo,
    Hint,
    AddTime,
    Gold
}
public class ResourceAnim : SingletonX<ResourceAnim>
{
    System.Action actionMove = null;
    System.Action actionUpdate;
    public float speed;
    Queue<Transform> poolRs = new Queue<Transform>();
    public Ease easeState1;
    public Ease easeState2;
    System.Action actionDelayAnim = null;
    System.Action actionUpdateSmt = null;
    private void Awake()
    {
    }

    // public void PlayAnimRate(List<EntityPerson> AllPerson, System.Action actionDone)
    // {
    //     bool delayAnim = false;
    //     List<DefaultPool> AllDefaultPool = new List<DefaultPool>();
    //     System.Action actionMoveToUI = null;
    //     float delayEnd = 0.75f;
    //     var amount = AllPerson.Count;
    //     var target = ScreenUI.Instance.GetPanelByName<ScreenGamePlay>().transform.FindChildByName("ic_Like");
    //     var txt = ScreenUI.Instance.GetPanelByName<ScreenGamePlay>().transform.FindChildByName("txt_Like").GetComponent<Text>();
    //     int amountRs = 0;
    //     float i = 0;
    //     foreach (var person in AllPerson)
    //     {
    //         System.Action action = null;
    //         System.Action actionDelay = null;
    //         float delay = i * 0.5f;
    //         actionDelay = () =>
    //         {
    //         };
    //         action = () =>
    //         {
    //             delay -= Time.deltaTime;
    //             if (delay < 0)
    //             {
    //                 if (person.IsFullHappy)
    //                 {
    //                     MyAudio.Instance.PlaySound("Happy", TypeSound.Clone);
    //                     var vfx = MyFactory.GetPoolerDefault("UI_Like");
    //                     vfx.transform.position = Camera.main.WorldToScreenPoint(person.GetGameObject().transform.position);
    //                     vfx.transform.parent = transform;
    //                     vfx.transform.localScale = Vector3.one;
    //                     vfx.transform.Rotate(Vector3.forward * UnityEngine.Random.Range(-30, 30));
    //                     AllDefaultPool.Add(vfx);

    //                 }
    //                 else
    //                 {
    //                     MyAudio.Instance.PlaySound("Bad", TypeSound.Clone);
    //                     var vfx = MyFactory.GetPoolerDefault("UI_UnLike");
    //                     vfx.transform.position = Camera.main.WorldToScreenPoint(person.GetGameObject().transform.position);
    //                     vfx.transform.parent = transform;
    //                     vfx.transform.localScale = Vector3.one;
    //                     vfx.AddTimeLife(2);
    //                 }
    //                 this.actionDelayAnim -= actionDelay;
    //                 actionUpdateSmt -= action;

    //             }
    //         };
    //         actionUpdateSmt += action;
    //         this.actionDelayAnim += actionDelay;
    //         i++;
    //     }
    //     actionUpdate = () =>
    //     {
    //         if (actionDelayAnim != null)
    //         {
    //             return;
    //         }
    //         actionUpdate = null;
    //         MyThread.Instance.AddDelayAction(0.5f, () =>
    //         {
    //             actionMoveToUI?.Invoke();
    //         });
    //     };
    //     actionMoveToUI = () =>
    //     {
    //         actionDelayAnim = null;
    //         int index = 0;
    //         foreach (var item in AllDefaultPool)
    //         {
    //             float delay = (float)index * 0.35f;
    //             System.Action actionWait = () =>
    //             {

    //             };
    //             actionDelayAnim += actionWait;
    //             item.transform.DOScale(Vector3.one * 1.5f, 0.15f).OnComplete(() =>
    //             {
    //                 item.transform.DOScale(Vector3.one, 0.25f);
    //                 item.transform.DOMove(target.position, 0.4f).SetEase(easeState1).OnComplete(() =>
    //                 {
    //                     MyAudio.Instance.PlaySound("Coin", TypeSound.Clone);
    //                     amountRs++;
    //                     txt.text = (amountRs).ToString() + "/" + LevelController.Instance.CountPerson;
    //                     actionDelayAnim -= actionWait;
    //                     if (!delayAnim)
    //                     {
    //                         delayAnim = true;
    //                         target.transform.DOPunchScale(Vector3.one * 0.5f, 0.1f);
    //                         txt.transform.DOPunchScale(Vector3.one * 0.5f, 0.15f).OnComplete(() =>
    //                         {
    //                             delayAnim = false;
    //                         });
    //                     }
    //                     item.DoDetroy();
    //                 });
    //             }).SetDelay(delay);  //SetDelay(UnityEngine.Random.Range(0, 0.75f));
    //             index++;
    //         }
    //         actionUpdate = () =>
    //         {
    //             if (actionDelayAnim != null)
    //             {
    //                 return;
    //             }
    //             actionUpdate = null;
    //             actionUpdate = () =>
    //             {
    //                 delayEnd -= Time.deltaTime;
    //                 if (delayEnd < 0)
    //                 {
    //                     actionUpdate = null;
    //                     Debug.Log("Done All Anim");
    //                     GameAction.Instance.InvokeAction(TypeAction.EndGame_2);
    //                     actionDone?.Invoke();
    //                 }
    //             };
    //         };
    //     };
    // }

    // public void PlayCoins(int amout, Vector2 startPos, Vector2 endPos, System.Action<int> actionAddValue, System.Action actionDone)
    // {
    //     int perCost = 5;
    //     int amountCost = amout / perCost;
    //     GameData.GetData<DataResources>().AddRs(TypeResources.Gold, amout);
    //     var radius = 250;
    //     System.Action actionUpdate_1 = null;
    //     System.Action actionUpdate_2 = null;
    //     List<DefaultPool> AllPooler = new List<DefaultPool>();
    //     actionUpdate_1 = () =>
    //     {
    //         int count = 0;
    //         actionUpdate = () =>
    //         {
    //             if (count <= 0)
    //             {
    //                 Debug.Log("Done Coins Anim");
    //                 actionUpdate_2?.Invoke();
    //             }
    //         };
    //         for (int i = 0; i < amountCost; i++)
    //         {
    //             var vfx = MyFactory.GetPoolerDefault("UI_Coins");
    //             AllPooler.Add(vfx);
    //             vfx.transform.parent = transform;
    //             vfx.transform.localScale = Vector3.one;
    //             vfx.transform.position = startPos;
    //             vfx.transform.DOMove(startPos + UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(0, radius), UnityEngine.Random.Range(0.35f, 1f))
    //             .SetDelay(UnityEngine.Random.Range(0f, 0.5f))
    //             .OnComplete(() =>
    //             {
    //                 count--;
    //             });
    //             count++;
    //         }

    //     };

    //     actionUpdate_2 = () =>
    //     {
    //         int count = 0;
    //         foreach (var vfx in AllPooler)
    //         {
    //             vfx.transform.DOMove(endPos, 0.65f).SetEase(Ease.InOutFlash)
    //             .SetDelay(UnityEngine.Random.Range(0f, 0.25f))
    //             .OnComplete(() =>
    //             {
    //                 actionAddValue?.Invoke(perCost);
    //                 vfx.DoDetroy();
    //                 count--;
    //             });
    //             count++;
    //         }

    //         actionUpdate = () =>
    //         {
    //             if (count <= 0)
    //             {
    //                 Debug.Log("Done Coins Anim");
    //                 actionUpdate = null;
    //                 actionDone?.Invoke();
    //             }
    //         };
    //     };

    //     actionUpdate_1?.Invoke();
    // }


    // void Update()
    // {
    //     actionUpdate?.Invoke();
    //     actionUpdateSmt?.Invoke();
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         var coin = GameData.GetData<DataResources>().GetAmountRs(TypeResources.Gold);
    //         var txt = ScreenUI.Instance.GetPanelByName<ScreenHome>().transform.FindChildByName("goldField").GetChild(0).GetComponent<Text>();
    //         var target = ScreenUI.Instance.GetPanelByName<ScreenHome>().transform.FindChildByName("Coin_ic");

    //         Tweener tween = null;
    //         PlayCoins(200, Vector2.zero, target.GetComponent<RectTransform>().position, (value) =>
    //         {
    //             coin += value;
    //             txt.text = coin.ToString();
    //             Debug.Log("Add Coin : " + coin);
    //             if (tween == null)
    //             {
    //                 tween = txt.transform.DOPunchScale(Vector3.one * 0.25f, 0.1f).OnComplete(() =>
    //                 {
    //                     txt.transform.localScale = Vector3.one;
    //                     tween = null;
    //                 });
    //             }

    //         },
    //         () =>
    //         {
    //             Debug.Log("Done All Coins");
    //             GameAction.Instance.InvokeAction(TypeAction.RefreshUI);
    //         });
    //     }
    // }
    // public void AddCoinEndGame(int coinAdd,RectTransform posStart,System.Action actionEnd)
    // {
    //     var coin = GameData.GetData<DataResources>().GetAmountRs(TypeResources.Gold);
    //     var txt = PopupUI.Instance.GetPanelByName<PopupReward>().transform.FindChildByName("goldField").GetChild(0).GetComponent<Text>();
    //     var target = PopupUI.Instance.GetPanelByName<PopupReward>().transform.FindChildByName("Coin_ic");
    //     Tweener tween = null;
    //     PlayCoins(coinAdd, posStart.position, target.GetComponent<RectTransform>().position, (value) =>
    //     {
    //         coin += value;
    //         txt.text = coin.ToString();
    //         Debug.Log("Add Coin : " + coin);
    //         if (tween == null)
    //         {
    //             tween = txt.transform.DOPunchScale(Vector3.one * 0.25f, 0.1f).OnComplete(() =>
    //             {
    //                 txt.transform.localScale = Vector3.one;
    //                 tween = null;
    //             });
    //         }

    //     },
    //     () =>
    //     {
    //         Debug.Log("Done All Coins");
    //         actionEnd?.Invoke();
    //         GameAction.Instance.InvokeAction(TypeAction.RefreshUI);
    //     });
    // }
}