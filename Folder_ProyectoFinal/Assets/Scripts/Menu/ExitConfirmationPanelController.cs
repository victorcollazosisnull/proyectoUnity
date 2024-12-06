using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitConfirmationPanelController : MonoBehaviour
{
    [Header("Panel Exit Settings")]
    public RectTransform confirmationPanel;  
    public Vector2 hiddenPosition = new Vector2(0f, 2000f); 
    public Vector2 centerPosition = Vector2.zero;       
    public float moveDuration = 0.7f;
    public GameObject canvasMenu;
    private bool isPanelVisible = false; 

    private void Start()
    {
        confirmationPanel.anchoredPosition = hiddenPosition;
    }

    public void ToggleConfirmationPanel()
    {
        isPanelVisible = !isPanelVisible;

        if (isPanelVisible)
        {
            canvasMenu.SetActive(false);
            confirmationPanel.DOAnchorPos(centerPosition, moveDuration).SetEase(Ease.OutCubic);
        }
        else
        {
            canvasMenu.SetActive(true);
            confirmationPanel.DOAnchorPos(hiddenPosition, moveDuration).SetEase(Ease.InCubic);
        }
    }

    public void ConfirmExit()
    {
        Application.Quit();
    }

    public void CancelExit()
    {
        ToggleConfirmationPanel();
    }
}