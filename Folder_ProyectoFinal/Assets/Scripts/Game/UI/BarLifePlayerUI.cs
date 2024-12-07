using UnityEngine;
using UnityEngine.UI;

public class BarLifePlayerUI : MonoBehaviour
{
    public Image bar;
    public Image circleUI;
    public float currentLife = 8f;
    public float maxLife = 8f;

    public Color colorAzul = Color.blue;
    public Color colorVerde = Color.green;
    public Color colorAmarillo = Color.yellow;
    public Color colorRojo = Color.red;

    private void OnEnable()
    {
        EnemyPatrol.OnPlayerDamage += RestarVida;
    }

    private void OnDisable()
    {
        EnemyPatrol.OnPlayerDamage -= RestarVida;
    }

    public void RestarVida(float damageAmount)
    {
        currentLife -= damageAmount;
        if (currentLife < 0)
        {
            currentLife = 0; 
        }

        bar.fillAmount = currentLife / maxLife;
        circleUI.fillAmount = currentLife / maxLife;

        ActualizarColor();
    }

    public void ActualizarColor()
    {
        if (currentLife >= 6)
        {
            circleUI.color = colorAzul;
        }
        else if (currentLife >= 4)
        {
            circleUI.color = colorVerde;
        }
        else if (currentLife >= 2)
        {
            circleUI.color = colorAmarillo;
        }
        else
        {
            circleUI.color = colorRojo;
        }
    }
}