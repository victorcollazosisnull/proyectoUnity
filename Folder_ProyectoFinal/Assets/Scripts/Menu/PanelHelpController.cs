using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHelpController : MonoBehaviour
{
    [Header("Panel Help Settings")]
    public RectTransform helpPanel;
    public Vector2 hiddenPosition = new Vector2(0f, 2000f);
    public Vector2 centerPosition = Vector2.zero;
    public float moveDuration = 0.7f;
    public GameObject canvasMenu;
    private bool isPanelVisible = false;

    private void Start()
    {
        helpPanel.anchoredPosition = hiddenPosition;
    }

    public void ToggleHelpPanel()
    {
        isPanelVisible = !isPanelVisible;

        if (isPanelVisible)
        {
            canvasMenu.SetActive(false);
            helpPanel.DOAnchorPos(centerPosition, moveDuration).SetEase(Ease.OutCubic);
        }
        else
        {
            helpPanel.DOAnchorPos(hiddenPosition, moveDuration).SetEase(Ease.InCubic);
        }
    }
    private IEnumerator ActivateMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canvasMenu.SetActive(true);
    }
}