using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;

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
    [Header("Audio Manager Reference")]
    [SerializeField] private AudioManager audioManager;
    [Header("Camera Sensitivity")]
    [SerializeField] private LaraCroftMovement movement;
    public Slider sensitivitySlider;

    private static float currentBrillo = 1f;
    private static float currentMasterVolume = 0.6f;
    private static float currentMusicVolume = 0.6f;
    private static float currentSFXVolume = 0.6f;

    [Header("Buttons for Mute and Unmute")]
    public Button muteMasterButton;
    public Button unmuteMasterButton;
    public Button muteMusicButton;
    public Button unmuteMusicButton;
    public Button muteSFXButton;
    public Button unmuteSFXButton;

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

        if (sensitivitySlider != null && movement != null)
        {
            sensitivitySlider.value = movement.mouseSensitivity;
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);

        }
        unmuteMasterButton.gameObject.SetActive(false);
        unmuteMusicButton.gameObject.SetActive(false);
        unmuteSFXButton.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        AudioManager.OnMasterMute += MasterMute;
        AudioManager.OnMasterUnmute += MasterUnmute;
        AudioManager.OnMusicMute += MusicMute;
        AudioManager.OnMusicUnmute += MusicUnmute;
        AudioManager.OnSFXMute += SFXMute;
        AudioManager.OnSFXUnmute += SFXUnmute;
    }

    private void OnDisable()
    {
        AudioManager.OnMasterMute -= MasterMute;
        AudioManager.OnMasterUnmute -= MasterUnmute;
        AudioManager.OnMusicMute -= MusicMute;
        AudioManager.OnMusicUnmute -= MusicUnmute;
        AudioManager.OnSFXMute -= SFXMute;
        AudioManager.OnSFXUnmute -= SFXUnmute;
    }

    public void ShowPanelOptions()
    {
        isOptionsVisible = !isOptionsVisible;

        if (isOptionsVisible)
        {
            optionsPanel.DOAnchorPos(visiblePosition, moveDuration).SetEase(Ease.OutCubic).SetUpdate(true); 
            optionsPanel.SetAsLastSibling();
        }
        else
        {
            optionsPanel.DOAnchorPos(hiddenPosition, moveDuration).SetEase(Ease.InCubic).SetUpdate(true); 
        }
    }

    private void SetBrightness(float brillo)
    {
        currentBrillo = brillo;
        Color color = brightness.color;
        color.a = 1f - brillo;
        brightness.color = color;
    }

    private void SetSensitivity(float sensitivity)
    {
        if (movement != null)
        {
            movement.mouseSensitivity = sensitivity;
        }
    }

    private void SetMasterVolume(float volume)
    {
        currentMasterVolume = volume;
        audioManager.SetMasterVolume(volume);
    }

    private void SetMusicVolume(float volume)
    {
        currentMusicVolume = volume;
        audioManager.SetMusicVolume(volume);
    }

    private void SetSFXVolume(float volume)
    {
        currentSFXVolume = volume;
        audioManager.SetSFXVolume(volume);
    }
    private void MasterMute()
    {
        ToggleButtons(muteMasterButton, unmuteMasterButton);
    }

    private void MasterUnmute()
    {
        ToggleButtons(unmuteMasterButton, muteMasterButton);
    }

    private void MusicMute()
    {
        ToggleButtons(muteMusicButton, unmuteMusicButton);
    }

    private void MusicUnmute()
    {
        ToggleButtons(unmuteMusicButton, muteMusicButton);
    }

    private void SFXMute()
    {
        ToggleButtons(muteSFXButton, unmuteSFXButton);
    }

    private void SFXUnmute()
    {
        ToggleButtons(unmuteSFXButton, muteSFXButton);
    }

    private void ToggleButtons(Button toHide, Button toShow)
    {
        toHide.gameObject.SetActive(false); 
        toShow.gameObject.SetActive(true); 
    }
    public void OnMuteMasterPressed()
    {
        audioManager.MuteMasterVolume();
    }

    public void OnUnmuteMasterPressed()
    {
        audioManager.UnmuteMasterVolume();
    }

    public void OnMuteMusicPressed()
    {
        audioManager.MuteMusicVolume();
    }

    public void OnUnmuteMusicPressed()
    {
        audioManager.UnmuteMusicVolume();
    }

    public void OnMuteSFXPressed()
    {
        audioManager.MuteSFXVolume();
    }

    public void OnUnmuteSFXPressed()
    {
        audioManager.UnmuteSFXVolume();
    }
    public void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0;
            ShowPanelOptions(); 
        }
        else
        {
            Time.timeScale = 1;
            HidePanelOptions();
        }
    }
    public void HidePanelOptions()
    {
        isOptionsVisible = false;
        optionsPanel.DOAnchorPos(hiddenPosition, moveDuration).SetEase(Ease.InCubic).SetUpdate(true); 
    }
    private void OnDestroy()
    {
        brightnessSlider.onValueChanged.RemoveListener(SetBrightness);
        masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.RemoveListener(SetSensitivity);
        }
    }
}