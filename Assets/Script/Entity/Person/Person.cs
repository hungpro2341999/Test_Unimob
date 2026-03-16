using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour
{
   
    public enum TypeAnim
    {
            Idle,
            Walk,
            CarryIdle,
            CarryMove
    }
    public enum TypeStatus
    {
        None,Ready,Waiting
    }
    [SerializeField]
    public TypeStatus Status
    {
        get
        {
            return status;
        }
        set
        {
            status = value;
        }
    }
    public bool isRemove = false;
    public Transform posCarry;
    public TypeAnim currentAnim;
    public List<GameObject> listItemCarry = new List<GameObject>();
    [SerializeField]
    protected TypeStatus status;
    protected NavMeshAgent navMeshAgent;
    protected Animator anim;
    protected System.Action actionUpdate = null;
    public virtual void Init()
    {      
        isRemove = false;
        posCarry = transform.FindChildByName("posCarry");
        anim = GetComponent<Animator>();       
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.Warp(transform.position);
        PlayAnim(currentAnim);
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        anim.updateMode = AnimatorUpdateMode.Normal;
    }
    public void DoMove(Vector3 target,System.Action actionDone = null)
    {
        PlayAnim(TypeAnim.Walk);       
        navMeshAgent.SetDestination(target);
        actionUpdate = () =>
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                PlayAnim(TypeAnim.Idle);
                actionUpdate = null;
                actionDone?.Invoke();
            }
        };
    }
    
   
    public void DoUpdate()
    {
        actionUpdate?.Invoke();           
    }
    protected void PlayAnim(TypeAnim typeAnim)
    {
        switch (typeAnim)
        {
            case TypeAnim.Idle:
                anim.SetBool("IsEmpty", true);
                anim.SetBool("IsCarryMove", false);
                anim.SetBool("IsMove", false);
                break;
            case TypeAnim.Walk:
                anim.SetBool("IsEmpty", true);
                anim.SetBool("IsCarryMove", false);
                anim.SetBool("IsMove", true);
                break;
            case TypeAnim.CarryIdle:
                anim.SetBool("IsEmpty", false);
                anim.SetBool("IsCarryMove", false);  
                anim.SetBool("IsMove", false);
                break;
            case TypeAnim.CarryMove:
                anim.SetBool("IsEmpty", false);
                anim.SetBool("IsCarryMove", true);
                anim.SetBool("IsMove", true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(typeAnim), typeAnim, null);
        }        
    }
    
}
