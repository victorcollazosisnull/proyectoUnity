using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class ItemsController : MonoBehaviour
{
    public Sprite itemSprite;
    public float scaleLenght = 0.2f;   
    public float scaleDuration = 1f;  

    private Vector3 originalScale;  

    void Start()
    {
        originalScale = transform.localScale;

        ScaleItem();
    }

    void ScaleItem()
    {
        Vector3 targetScale = originalScale + new Vector3(scaleLenght, scaleLenght, scaleLenght);
        transform.DOScale(targetScale, scaleDuration)
                 .SetLoops(-1, LoopType.Yoyo)  
                 .SetEase(Ease.InOutSine);  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LaraCroftInventory playerInventory = other.GetComponent<LaraCroftInventory>();
            if (playerInventory != null)
            {
                playerInventory.AddImageToBox(itemSprite);
            }
            Destroy(gameObject);
        }
    }
}