using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public interface IShowInfor
{
    void ShowInfor();
}

public class Box : MonoBehaviour, IShowInfor
{
    public bool isInteract = true;
    public int Index = 0;
    public BigNumber price;
   
    Animation anim;

    void Start()
    {       
        anim = GetComponent<Animation>();
    }
    public void OpenBox(bool isOpen)
    {
        Debug.Log("Mouse Click");
        string name = isOpen ? "BoxOpen" : "BoxIdle";
        anim.clip = anim.GetClip(name);
        anim.Play();      
    }
    public void ShowInfor()
    {
        if(!isInteract)
        {
            return;
        }
        EventBus.Run(new EventCickBox { @object = this });
    }        
    public struct EventCickBox 
    {
        public Box @object;
    }
}
public struct EventOpenTree
{
    public Tree tree;
}