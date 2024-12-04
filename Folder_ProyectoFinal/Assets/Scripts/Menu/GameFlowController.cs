using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class GameFlowController : MonoBehaviour
{
    public static event Action OnMenuExited;
    public static event Action OnGameStarted;

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
    }

    public void OnPlayButton()
    {
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
        ToggleCamera(shipCamera);
    }

    public void OnCreditsButton()
    {
        panelCreditsController.ToggleCreditsPanel();
        ToggleCamera(shipCamera);
    }

    public void OnExitOptions()
    {
        panelOptionsController.ShowPanelOptions(); 
        ToggleCamera(menuCamera);
    }

    public void OnExitCredits()
    {
        panelCreditsController.ToggleCreditsPanel();
        ToggleCamera(menuCamera);
    }

    private void ToggleCamera(CinemachineVirtualCamera targetCamera)
    {
        menuCamera.Priority = 0;
        gameplayCamera.Priority = 0;
        shipCamera.Priority = 0;

        targetCamera.Priority = 10;
    }
}