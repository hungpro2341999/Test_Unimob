using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnActive : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
}
