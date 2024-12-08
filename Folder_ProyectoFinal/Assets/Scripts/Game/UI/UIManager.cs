using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI txtPROKills;
    public TextMeshProUGUI txtPRODiamonds;
    private int enemyKills = 0;
    private int diamonds = 0;
    private void OnEnable()
    {
        EnemieLife.OnDeath += UpdateKillCount;
        LaraCroftMovement.OnDiamondCollected += UpdateDiamondsCount;
    }

    private void OnDisable()
    {
        EnemieLife.OnDeath -= UpdateKillCount;
        LaraCroftMovement.OnDiamondCollected -= UpdateDiamondsCount;
    }

    private void UpdateKillCount()
    {
        enemyKills++; 
        txtPROKills.text = enemyKills.ToString();  
    }
    private void UpdateDiamondsCount(int newDiamondCount)
    {
        diamonds = newDiamondCount;
        txtPRODiamonds.text = $"{diamonds}"; 
    }
}