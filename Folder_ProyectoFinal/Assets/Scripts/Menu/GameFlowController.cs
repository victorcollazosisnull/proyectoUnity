using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }
    private bool isGameActived;

    public static event Action OnMenuExited;
    public static event Action OnGameStarted;
    public static event Action OnGamePaused;

    [SerializeField] private LaraCroftInputReader InputReader;
    public Animator playerAnimator;
    public CinemachineVirtualCamera menuCamera;
    public CinemachineVirtualCamera gameplayCamera;
    public CinemachineVirtualCamera shipCamera;
    public float transitionTime = 0.5f;

    [Header("CanvasGame")]
    public GameObject canvasMenu;
    public GameObject canvasGame;

    [Header("Panel Options")]
    [SerializeField] private PanelOptionsController panelOptionsController;

    [Header("Panel Credits")]
    [SerializeField] private PanelCreditsController panelCreditsController;

    [Header("Panel Help")]
    [SerializeField] private PanelHelpController panelHelpController;

    [Header("Audio Settings")]
    [SerializeField] private AudioClipsSO menuMusicClip; 
    [SerializeField] private AudioClipsSO gameMusicClip; 
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        menuCamera.Priority = 10;
        gameplayCamera.Priority = 0;
        shipCamera.Priority = 0;

        canvasMenu.SetActive(true);
        canvasGame.SetActive(false);

        InputReader.BlockInputs(true);

        InputReader.OnPauseInput += HandlePauseInput;

        audioSource.clip = menuMusicClip.clip;
        audioSource.volume = menuMusicClip.volume;
        audioSource.pitch = menuMusicClip.pitch;
        audioSource.outputAudioMixerGroup = musicMixerGroup;
        audioSource.Play();
    }
    private void OnDestroy()
    {
        InputReader.OnPauseInput -= HandlePauseInput; 
    }
    public void OnPlayButton()
    {
        isGameActived = true;
        canvasMenu.SetActive(false);
        StartCoroutine(TransitionToGameplay());
        ChangeMusicToGame();
    }
    private void ChangeMusicToGame()
    {
        audioSource.Stop();

        audioSource.clip = gameMusicClip.clip;
        audioSource.volume = gameMusicClip.volume;
        audioSource.pitch = gameMusicClip.pitch;
        audioSource.outputAudioMixerGroup = musicMixerGroup;
        audioSource.Play();
    }
    private IEnumerator TransitionToGameplay()
    {
        OnMenuExited?.Invoke();
        InputReader.BlockInputs(true);
        playerAnimator.SetTrigger("SitToSitUp");

        menuCamera.Priority = 0;
        gameplayCamera.Priority = 10;

        yield return new WaitForSeconds(2f);

        canvasMenu.SetActive(false);
        canvasGame.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputReader.BlockInputs(false);
        panelOptionsController.UpdatePanelForGame(true);
        OnGameStarted?.Invoke();
    }

    public void OnOptionsButton()
    {
        panelOptionsController.ShowPanelOptions();
        canvasMenu.SetActive(false);
        ToggleCamera(shipCamera);
    }

    public void OnCreditsButton()
    {
        panelCreditsController.ToggleCreditsPanel();
        canvasMenu.SetActive(false);
        ToggleCamera(shipCamera);
    }
    public void OnHelpButton()
    {
        panelHelpController.ToggleHelpPanel();
        canvasMenu.SetActive(false);
        ToggleCamera(shipCamera);
    }
    public void OnExitOptions()
    {
        if (!isGameActived)
        {
            panelOptionsController.ShowPanelOptions();
            ToggleCamera(menuCamera);
            StartCoroutine(ActivateMenuAfterDelay(1f));
        }
        else if (isGameActived)
        {
            panelOptionsController.ShowPanelOptions();
            canvasMenu.SetActive(false);
            canvasGame.SetActive(true);
        }
    }

    public void OnExitCredits()
    {
        panelCreditsController.ToggleCreditsPanel();
        ToggleCamera(menuCamera);
        StartCoroutine(ActivateMenuAfterDelay(1f));
    }
    public void OnExitHelp()
    {
        panelHelpController.ToggleHelpPanel();
        ToggleCamera(menuCamera);
        StartCoroutine(ActivateMenuAfterDelay(1f));
    }
    private IEnumerator ActivateMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        canvasMenu.SetActive(true);
    }
    private void ToggleCamera(CinemachineVirtualCamera targetCamera)
    {
        menuCamera.Priority = 0;
        gameplayCamera.Priority = 0;
        shipCamera.Priority = 0;

        targetCamera.Priority = 10;
    }
    private void HandlePauseInput()
    {
        if (!isGameActived) return; 

        if (PanelOptionsController.isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if (isGameActived)
        {
            PanelOptionsController.isGamePaused = true;
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            canvasGame.SetActive(false);
            panelOptionsController.ShowPanelOptions();
        }
    }

    public void ResumeGame()
    {
        if (isGameActived)
        {
            PanelOptionsController.isGamePaused = false;
            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            canvasGame.SetActive(true);
            canvasMenu.SetActive(false);

            panelOptionsController.HidePanelOptions();
        }
    }
    public void ReturnToMenu()
    {
        InputReader.BlockInputs(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("ScenePrototypes");
    }
    private void ChangeMusicToMenu()
    {
        audioSource.Stop();

        audioSource.clip = menuMusicClip.clip;
        audioSource.volume = menuMusicClip.volume;
        audioSource.pitch = menuMusicClip.pitch;
        audioSource.outputAudioMixerGroup = musicMixerGroup;
        audioSource.Play();
    }

}