using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class ButtonsEffects : MonoBehaviour
{
    public Button button;  
    public float scaleFactor = 1.2f;  
    public float duration = 0.2f;  
    public Vector3 initialScale = new Vector3(2.6f, 2.6f, 2.6f);

    void Start()
    {
        button.onClick.AddListener(OnButtonClick);

        transform.localScale = initialScale;
    }

    void OnEnable()
    {
        transform.localScale = initialScale;
    }

    public void OnPointerEnter()
    {
        transform.DOScale(initialScale * scaleFactor, duration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit()
    {
        transform.DOScale(initialScale, duration).SetEase(Ease.OutBack);
    }

    void OnButtonClick()
    {
        transform.DOScale(initialScale, duration).SetEase(Ease.OutBack);
    }
}
