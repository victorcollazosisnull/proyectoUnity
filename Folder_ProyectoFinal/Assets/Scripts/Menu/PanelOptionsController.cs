using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelOptionsController : MonoBehaviour
{
    public static bool isGamePaused = false;

    [Header("Panel Options Movement")]
    public RectTransform optionsPanel;
    public Vector2 hiddenPosition = new Vector2(-2000f, 0f);
    public Vector2 visiblePosition = Vector2.zero;
    public float moveDuration = 0.5f; 
    private bool isOptionsVisible = false;

    [Header("Brightness Settings")]
    public Slider brightnessSlider;
    public Image brightness;

    [Header("Audio Settings")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private static float currentBrillo = 1f;
    private static float currentMasterVolume = 0.6f;
    private static float currentMusicVolume = 0.6f;
    private static float currentSFXVolume = 0.6f;

    void Start()
    {
        optionsPanel.anchoredPosition = hiddenPosition;

        brightnessSlider.value = currentBrillo;
        SetBrightness(currentBrillo);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);

        masterSlider.value = currentMasterVolume;
        musicSlider.value = currentMusicVolume;
        sfxSlider.value = currentSFXVolume;

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void ShowPanelOptions()
    {
        isOptionsVisible = !isOptionsVisible;

        if (isOptionsVisible)
        {
            optionsPanel.DOAnchorPos(visiblePosition, moveDuration).SetEase(Ease.OutCubic);
            optionsPanel.SetAsLastSibling();
        }
        else
        {
            optionsPanel.DOAnchorPos(hiddenPosition, moveDuration).SetEase(Ease.InCubic);
        }
    }

    private void SetBrightness(float brillo)
    {
        currentBrillo = brillo;
        Color color = brightness.color;
        color.a = 1f - brillo;
        brightness.color = color;
    }

    private void SetMasterVolume(float volume)
    {
        currentMasterVolume = volume;
        AudioManager.instance.SetMasterVolume(volume);
    }

    private void SetMusicVolume(float volume)
    {
        currentMusicVolume = volume;
        AudioManager.instance.SetMusicVolume(volume);
    }

    private void SetSFXVolume(float volume)
    {
        currentSFXVolume = volume;
        AudioManager.instance.SetSFXVolume(volume);
    }

    private void OnDestroy()
    {
        brightnessSlider.onValueChanged.RemoveListener(SetBrightness);
        masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }
}