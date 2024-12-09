using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinCondition : MonoBehaviour
{
    [Header("Requirements")]
    public int requiredDiamonds = 9;

    [Header("Dependencies")]
    public UIManager uiManager; 
    public GameObject obstacleToDestroy; 

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
}