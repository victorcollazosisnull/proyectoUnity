using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class GameFlowController : MonoBehaviour
{
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
    }
    private void OnDestroy()
    {
        InputReader.OnPauseInput -= HandlePauseInput; 
    }
    public void OnPlayButton()
    {
        isGameActived = true;
        StartCoroutine(TransitionToGameplay());
    }

    private IEnumerator TransitionToGameplay()
    {
        OnMenuExited?.Invoke();

        playerAnimator.SetTrigger("SitToSitUp");

        yield return new WaitForSeconds(transitionTime);

        menuCamera.Priority = 0;
        gameplayCamera.Priority = 10;

        canvasMenu.SetActive(false);
        canvasGame.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        OnGameStarted?.Invoke();

        InputReader.BlockInputs(false);
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

    public void OnExitOptions()
    {
        if (!isGameActived)
        {
            panelOptionsController.ShowPanelOptions();
            canvasMenu.SetActive(true);
            ToggleCamera(menuCamera);
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
        canvasMenu.SetActive(true);
        ToggleCamera(menuCamera);
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
}