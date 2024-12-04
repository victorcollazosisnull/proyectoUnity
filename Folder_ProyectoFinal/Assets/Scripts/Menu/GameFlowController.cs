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
    public float transitionTime = 1f; 

    [Header("CanvasGame")]
    public GameObject canvasMenu;
    public GameObject canvasGame;

    [Header("Panel Options")]
    public PanelOptionsController panelOptionsController; 

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        menuCamera.gameObject.SetActive(true);
        gameplayCamera.gameObject.SetActive(false);
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

        menuCamera.gameObject.SetActive(false);
        gameplayCamera.gameObject.SetActive(true);

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
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}