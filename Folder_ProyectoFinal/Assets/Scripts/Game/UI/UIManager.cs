using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI txtPRO;  
    private int enemyKills = 0;  

    private void OnEnable()
    {
        EnemieLife.OnDeath += UpdateKillCount;  
    }

    private void OnDisable()
    {
        EnemieLife.OnDeath -= UpdateKillCount;  
    }

    private void UpdateKillCount()
    {
        enemyKills++; 
        txtPRO.text = enemyKills.ToString();  
    }
}