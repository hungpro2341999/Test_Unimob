using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOutside : MonoBehaviour
{

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ClickOutside");
    }
}
