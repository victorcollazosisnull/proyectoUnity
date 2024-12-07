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

    [Header("References")]
    public GameObject bow;
    public GameObject potion;
    public GameObject kit;
    public GameObject Torch;
    private LaraCroftMovement laraMovement;

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
        laraMovement = GetComponent<LaraCroftMovement>();

        inputReader.OnMouseWheelInput += HandleMouse;

        if (bow != null)
        {
            bow.SetActive(false); 
        }
    }

    public void AddImageToBox(Sprite itemSprite)
    {
        var node = inventorySlots.Head;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (node.Value.sprite == null)
            {
                node.Value.sprite = itemSprite;
                return;
            }
            node = node.Next;
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

        CheckForItem();
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

        CheckForItem();
    }

    private void HighlightCurrentBox()
    {
        currentBox.Value.transform.localScale = highlightedScaleBoxes;
    }

    private void UnhighlightCurrentBox()
    {
        currentBox.Value.transform.localScale = originalScaleBoxes;
    }

    private void CheckForItem() 
    {
        if (bow == null || laraMovement == null) return;

        if (currentBox.Value.sprite != null && currentBox.Value.sprite.name == "arrow")
        {
            bow.SetActive(true);
            laraMovement.EquipBow(true);
            potion.SetActive(false);
            kit.SetActive(false);
            Torch.SetActive(false);
        }

        else if (currentBox.Value.sprite != null && currentBox.Value.sprite.name == "Potion")
        {
            UsePotion();
        }

        else if (currentBox.Value.sprite != null && currentBox.Value.sprite.name == "kit")
        {
            UseKit();
        }

        else if (currentBox.Value.sprite != null && currentBox.Value.sprite.name == "torch")
        {
            EquipTorch();
        }
        else
        {
            bow.SetActive(false);
            laraMovement.EquipBow(false);
            potion.SetActive(false);
            kit.SetActive(false);
            Torch.SetActive(false);
        }
    }
    
    private void UsePotion()
    {
        potion.SetActive(true);
        laraMovement.EquipBow(false);
        bow.SetActive(false);
        kit.SetActive(false);
        Torch.SetActive(false);
        Debug.Log("poción...");
        
    }

    private void UseKit()
    {
        kit.SetActive(true);
        laraMovement.EquipBow(false);
        bow.SetActive(false);
        potion.SetActive(false);
        Torch.SetActive(false);
        Debug.Log("kit...");
    }

    private void EquipTorch()
    {
        Torch.SetActive(true);
        potion.SetActive(false);
        laraMovement.EquipBow(false);
        bow.SetActive(false);
        kit.SetActive(false);
        Debug.Log("antorcha...");
    }
}