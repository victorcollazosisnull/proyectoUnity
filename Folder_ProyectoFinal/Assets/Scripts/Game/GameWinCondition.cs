using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinCondition : MonoBehaviour
{
    [Header("Requirements")]
    public int requiredDiamonds = 9;
    [Header("Dependencies")]
    public UIManager uiManager; 
    public GameObject obstacleToDestroy;
    [Header("Victory Settings")]
    public Image darkBackground;
    public GameObject victoryPanel;
    public float fadeDuration = 3f;
    private void Awake()
    {
        victoryPanel.SetActive(false);
    }
    private void OnEnable()
    {
        LaraCroftHealth.OnVictory += HandleVictory;
    }

    private void OnDisable()
    {
        LaraCroftHealth.OnVictory -= HandleVictory;
    }
    private void Update()
    {
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (uiManager != null && uiManager.GetDiamondCount() >= requiredDiamonds)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        if (obstacleToDestroy != null)
        {
            Destroy(obstacleToDestroy); 
            Debug.Log("murop destruido");
        }
    }

    private void HandleVictory()
    {
        StartCoroutine(FadeToVictory());
    }

    private IEnumerator FadeToVictory()
    {
        float elapsed = 0f;
        Color startColor = darkBackground.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            darkBackground.color = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
            yield return null;
        }
        victoryPanel.SetActive(true);
        EnableCursor();
    }

    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
