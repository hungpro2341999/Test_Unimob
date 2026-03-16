using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public enum TypeSoundClick
{
     Button_Select,Button_Start,Button_Login,Button_Back,ButtonClick
};
public class RippleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public bool invokeOnce = false;
    public float activeScale = 1.2f;
    public bool interactable = true;
    public bool playSoundClick = true;
    public UnityEvent onClick;

    bool invoked = false;
    const float ZoomOutTime = 0.1f;
    const float ZoomInTime = 0.1f;
    public TypeSoundClick Sound = TypeSoundClick.ButtonClick;

    Vector3 baseScale = new Vector3(1.0f, 1.0f, 1.0f);
    [HideInInspector] private Button mButton = null;

    void Start()
    {
        baseScale = transform.localScale;
        mButton = gameObject.GetComponent<Button>();
    }

    void OnEnable()
    {
        ResetInvokeState();
    }

    public void ResetInvokeState()
    {
        invoked = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (mButton != null)
        {
            interactable = mButton.interactable;
        }
        if (interactable)
        {
            StartCoroutine("StartClick");
        }
        //MyAudio.Instance.PlaySound("sfx_click",TypeSound.None);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine("StartClick");
        transform.localScale = baseScale;
    }

    public void OnPointerClick(PointerEventData eventdata)
    {
        if (interactable && (!invokeOnce || !invoked))
        {
            if (playSoundClick)
            {
            }
            onClick.Invoke();
            invoked = true;
        }
    }

    IEnumerator StartClick()
    {
        float tCounter = 0;

        while (tCounter < ZoomOutTime)
        {
            tCounter += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(baseScale, baseScale * activeScale, tCounter / ZoomOutTime);
            yield return null;
        }
    }

    IEnumerator StartExit()
    {
        float tCounter = 0;

        while (tCounter < ZoomInTime)
        {
            tCounter += Time.deltaTime;
            transform.localScale = Vector3.Lerp(baseScale * activeScale, baseScale, tCounter / ZoomInTime);
            yield return null;
        }
    }

}