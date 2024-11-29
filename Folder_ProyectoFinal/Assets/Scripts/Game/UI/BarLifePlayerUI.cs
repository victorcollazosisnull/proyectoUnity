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

    void Update()
    {
        // Para probar nomas profe xd
        if (Input.GetKeyDown(KeyCode.T))
        {
            RestarVida();
        }
    }

    void RestarVida()
    {
        currentLife -= 1;
        if (currentLife < 0) currentLife = 0;  

        bar.fillAmount = currentLife / maxLife;

        circleUI.fillAmount = currentLife / maxLife;

        ActualizarColor();
    }

    void ActualizarColor()
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

