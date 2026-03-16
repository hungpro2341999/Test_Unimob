using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSoundShow : MonoBehaviour
{
    void OnEnable()
    {
        string nameSound = "PopupShow";
        MyAudio.Instance.PlaySound(nameSound, TypeSound.Clone);
    }
}
