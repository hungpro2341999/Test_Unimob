using UnityEngine;

public class SafeAreaUI : MonoBehaviour
{
    public bool isChangeWithHigh = false;
    public bool isTop = false;
    public bool isTwoSide = false;


    Vector2 offsetMin;
    Vector2 offsetMax;
    Vector2 anchoredPosition;
    Vector2 sizeDelta;
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = gameObject.GetComponent<RectTransform>();
        offsetMin = rect.offsetMin;
        offsetMax = rect.offsetMax;
        anchoredPosition = rect.anchoredPosition;
        sizeDelta = rect.sizeDelta;
        UpdateSafeBanner();
       // GameAction.Instance.RegisterAction(TypeAction.UpdateAds, UpdateSafeBanner);
    }
    public void UpdateSafeBanner()
    {
        float sx = (Screen.width / 1080.0f);
        float sy = (Screen.height / 1920.0f);
        float sl = (sx < sy ? sx : sy);
        sl = 1;
        float vlTop = ((HelperSafeUI.GetUpSafeArea()) / sl);
        float vlBot = (HelperSafeUI.GetBottomSafeArea() / sl);
        if (isTwoSide)
        {
            if (!isChangeWithHigh)
            {
                rect.offsetMin = new Vector2(offsetMin.x, offsetMin.y + vlBot);
                rect.offsetMax = new Vector2(offsetMax.x, offsetMax.y - vlTop);
            }
            else
            {
                rect.offsetMax = new Vector2(offsetMax.x, offsetMax.y - vlTop);
            }
        }
        else
        {
            if (!isChangeWithHigh)
            {
                rect.anchoredPosition = anchoredPosition + Vector2.up * (isTop ? -1 * vlTop : 1 * vlBot);
            }
            else
            {
                rect.sizeDelta = sizeDelta + Vector2.up * (isTop ? -1 * vlTop : 1 * vlBot);
            }
        }
    }
    
}

public class HelperSafeUI
{
    public static float GetUpSafeArea()
    {
        Rect safeArea = Screen.safeArea;
        float screenHeight = Screen.height;
        float safeAreaTop = screenHeight - (safeArea.y + safeArea.height);
        return safeAreaTop/2;
    }
    public static float GetBottomSafeArea()
    {
        // Rect safeArea = Screen.safeArea;
        // float screenHeight = Screen.height;
        // float safeAreaBottom = safeArea.y;
        // var isShowAds = Advertisements.Instance.CanShowAds();
        // float height = 0;
        // if (isShowAds)
        // {
        //     height = Advertisements.Instance.GetBannerHeight();
        //     if (height <= 150)
        //         height = 150;
        // }
        // return safeAreaBottom + height;
        return 100;
    }
}