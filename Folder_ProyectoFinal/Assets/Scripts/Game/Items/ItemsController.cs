using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class ItemsController : MonoBehaviour
{
    [SerializeField] private AnimationCurve scaleCurve; 
    [SerializeField] private float scaleDuration = 1f;  
    public Sprite itemSprite;                           

    private float elapsedTime = 0f;                  
    private Vector3 originalScale;               

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float normalizedTime = (elapsedTime % scaleDuration) / scaleDuration;

        float curveValue = scaleCurve.Evaluate(normalizedTime);
        transform.localScale = originalScale * (1f + curveValue);
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