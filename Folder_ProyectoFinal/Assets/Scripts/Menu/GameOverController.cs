using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [Header("Game Over Settings")]
    public Image darkBackground;
    public GameObject gameOverPanel;
    public float fadeDuration = 3f;
    private void Awake()
    {
        gameOverPanel.SetActive(false);
    }
    private void OnEnable()
    {
        LaraCroftHealth.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        LaraCroftHealth.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeToBlack()
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

        gameOverPanel.SetActive(true);
        EnableCursor();
    }

    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}