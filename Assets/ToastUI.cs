using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class ToastUI : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public CanvasGroup canvasGroup;

    public static ToastUI Instance;

    void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0;
    }

    public void ShowToast(string message, float duration = 1.25f)
    {
        StopAllCoroutines();
        StartCoroutine(Show(message, duration));
    }

    IEnumerator Show(string message, float duration)
    {
        messageText.text = message;

        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(duration);

        canvasGroup.alpha = 0;
    }
}