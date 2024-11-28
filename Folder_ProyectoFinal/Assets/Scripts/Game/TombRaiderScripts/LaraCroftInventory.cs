using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LaraCroftInventory : MonoBehaviour
{
    private LaraCroftInputReader inputReader;
    public DoubleCircularLinkedList<Image> inventorySlots = new DoubleCircularLinkedList<Image>();
    private DoubleCircularLinkedList<Image>.Node currentBox;
    public Image[] InventoryBoxes;
    private Vector3 originalScaleBoxes;
    private Vector3 highlightedScaleBoxes = new Vector3(1.3f, 1.3f, 1.3f);
    private float lastScroll = 0f;
    private float scrollSensitivity = 0.1f;
    public GameObject bow;

    void Start()
    {
        for (int i = 0; i < InventoryBoxes.Length; i++)
        {
            inventorySlots.InsertAtEnd(InventoryBoxes[i]);
        }
        currentBox = inventorySlots.Head;
        originalScaleBoxes = currentBox.Value.transform.localScale;
        HighlightCurrentBox();
        inputReader = GetComponent<LaraCroftInputReader>();
        inputReader.OnMouseWheelInput += HandleMouse;

        if (bow != null)
        {
            bow.SetActive(false);
        }
    }

    public void AddImageToBox(Sprite itemSprite)
    {
        for (int i = 0; i < InventoryBoxes.Length; i++)
        {
            if (InventoryBoxes[i].sprite == null)
            {
                InventoryBoxes[i].sprite = itemSprite;
                return;
            }
        }
    }

    void OnDisable()
    {
        inputReader.OnMouseWheelInput -= HandleMouse;
    }

    private void HandleMouse(float scroll)
    {
        if (Mathf.Abs(scroll - lastScroll) > scrollSensitivity)
        {
            if (scroll > 0)
            {
                MoveToNextBox();
            }
            else if (scroll < 0)
            {
                MoveToPreviousBox();
            }
            lastScroll = scroll;
        }
    }

    private void MoveToNextBox()
    {
        if (currentBox.Next == null)
        {
            return;
        }
        UnhighlightCurrentBox();
        currentBox = currentBox.Next;
        HighlightCurrentBox();

        CheckForBow();
    }

    private void MoveToPreviousBox()
    {
        if (currentBox.Previous == null)
        {
            return;
        }
        UnhighlightCurrentBox();
        currentBox = currentBox.Previous;
        HighlightCurrentBox();

        CheckForBow();
    }

    private void HighlightCurrentBox()
    {
        currentBox.Value.transform.localScale = highlightedScaleBoxes;
    }

    private void UnhighlightCurrentBox()
    {
        currentBox.Value.transform.localScale = originalScaleBoxes;
    }

    private void CheckForBow()
    {
        if (bow == null) return;

        Sprite bowSprite = bow.GetComponent<SpriteRenderer>().sprite; 
        if (currentBox.Value.sprite == bowSprite)
        {
            bow.SetActive(true); 
        }
        else
        {
            bow.SetActive(false); 
        }
    }
}