using UnityEngine;
using UnityEngine.UIElements;

public class ItemsController : MonoBehaviour
{
    public Sprite itemSprite;
    public float rotationSpeed = 50f;
    void Update()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, rotationSpeed * Time.deltaTime);
        transform.rotation = transform.rotation * rotation;
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