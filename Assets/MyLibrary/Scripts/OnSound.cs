using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSound : MonoBehaviour
{
    public string nameSound;
    void OnEnable()
    {
        MyAudio.Instance.PlaySound(nameSound, TypeSound.Clone);
    }
}
