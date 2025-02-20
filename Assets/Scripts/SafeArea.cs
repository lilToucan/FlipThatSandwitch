using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void OnEnable()
    {
        RectTransform rectTransforms = GetComponent<RectTransform>();
        SafeThatRect(rectTransforms);
    }

    void SafeThatRect(RectTransform _rect)
    {
        Rect safeArea = Screen.safeArea;

        Vector2 screenWidthHeight = new Vector2(Screen.width, Screen.height);

        Vector2 max = safeArea.position / screenWidthHeight;
        Vector2 min = (safeArea.position + safeArea.size) / screenWidthHeight;

        _rect.anchorMax = max;
        _rect.anchorMin = min;
    }

}
