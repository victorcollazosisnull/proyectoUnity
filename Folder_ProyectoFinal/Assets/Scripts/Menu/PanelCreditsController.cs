using DG.Tweening;
using UnityEngine;

public class PanelCreditsController : MonoBehaviour
{
    [Header("Panel Credits Movement")]
    public RectTransform creditsPanel; 
    public Vector2 hiddenPosition = new Vector2(0f, 2000f); 
    public Vector2 visiblePosition = Vector2.zero; 
    public float moveDuration = 0.5f; 

    private bool isCreditsVisible = false; 

    private void Start()
    {
        creditsPanel.anchoredPosition = hiddenPosition;
    }

    public void ToggleCreditsPanel()
    {
        isCreditsVisible = !isCreditsVisible;

        if (isCreditsVisible)
        {
            creditsPanel.DOAnchorPos(visiblePosition, moveDuration).SetEase(Ease.OutCubic);
        }
        else
        {
            creditsPanel.DOAnchorPos(hiddenPosition, moveDuration).SetEase(Ease.InCubic);
        }
    }
}