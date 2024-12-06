using DG.Tweening;
using UnityEngine;

public class TitleScaler : MonoBehaviour
{
    public float escalaMaxima = 1.5f;  
    public float escalaMinima = 1f;   
    public float velocidad = 1f;       

    private void Start()
    {
        PingPongScale();
    }

    void PingPongScale()
    {
        transform.DOScale(escalaMaxima, velocidad)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine);   
    }
}