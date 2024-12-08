using UnityEngine;
using UnityEngine.UI;

public class LaraCroftInventory : MonoBehaviour
{
    private LaraCroftInputReader inputReader;
    private LaraCroftMovement laraMovement;
    private LaraCroftHealth health;
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
    public GameObject gun;

    private GameObject currentEquippedItem;
    private void Awake()
    {
        inputReader = GetComponent<LaraCroftInputReader>();
        laraMovement = GetComponent<LaraCroftMovement>();
        health = GetComponent<LaraCroftHealth>();
    }
    void Start()
    {
        for (int i = 0; i < InventoryBoxes.Length; i++)
        {
            inventorySlots.InsertAtEnd(InventoryBoxes[i]);
        }
        currentBox = inventorySlots.Head;
        originalScaleBoxes = currentBox.Value.transform.localScale;
        HighlightCurrentBox();

        inputReader.OnMouseWheelInput += HandleMouse;

        if (bow != null)
        {
            bow.SetActive(false);
        }
        if (gun != null)
        {
            gun.SetActive(false);
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
            gun.SetActive(false);
            currentEquippedItem = bow;
            laraMovement.EquipBow(true);
            laraMovement.EquipKit(false);
            laraMovement.EquipPotion(false);
            laraMovement.EquipGun(false);
            potion.SetActive(false);
            kit.SetActive(false);
            Torch.SetActive(false);
        }
        else if (currentBox.Value.sprite != null && currentBox.Value.sprite.name == "gun")
        {
            gun.SetActive(true);
            bow.SetActive(false);
            laraMovement.EquipGun(true);
            currentEquippedItem = gun;
            laraMovement.EquipBow(false);
            laraMovement.EquipKit(false);
            laraMovement.EquipPotion(false);
        }
        else if (currentBox.Value.sprite != null && currentBox.Value.sprite.name == "Potion")
        {
            GetPotion();
            bow.SetActive(false);
            currentEquippedItem = potion;
            laraMovement.EquipPotion(true);
            laraMovement.EquipKit(false);
            laraMovement.EquipGun(false);
        }
        else if (currentBox.Value.sprite != null && currentBox.Value.sprite.name == "kit")
        {
            GetKit();
            laraMovement.EquipKit(true);
            bow.SetActive(false);
            laraMovement.EquipPotion(false);
            laraMovement.EquipGun(false);
            currentEquippedItem = kit;
        }
        else if (currentBox.Value.sprite != null && currentBox.Value.sprite.name == "torch")
        {
            EquipTorch();
            currentEquippedItem = Torch;
            bow.SetActive(false);
            laraMovement.EquipKit(false);
            laraMovement.EquipPotion(false);
            laraMovement.EquipGun(false);
        }
        else
        {
            bow.SetActive(false);
            laraMovement.EquipBow(false);
            laraMovement.EquipKit(false);
            laraMovement.EquipGun(false);
            laraMovement.EquipPotion(false);
            potion.SetActive(false);
            kit.SetActive(false);
            Torch.SetActive(false);
            currentEquippedItem = null;
        }
    }
    private void GetPotion()
    {
        potion.SetActive(true);
        laraMovement.EquipBow(false);
        bow.SetActive(false);
        kit.SetActive(false);
        Torch.SetActive(false);
        Debug.Log("poción...");
    }
    public void UsePotion()
    {
        Debug.Log("uso potion");
        health.UsePotion();
        potion.SetActive(false);
        currentBox.Value.sprite = null;
    }
    private void GetKit()
    {
        kit.SetActive(true);
        laraMovement.EquipBow(false);
        bow.SetActive(false);
        potion.SetActive(false);
        Torch.SetActive(false);
        Debug.Log("kit...");
    }
    public void UseKit()
    {
        Debug.Log("uso kit");
        health.UseMedKit();
        kit.SetActive(false);
        currentBox.Value.sprite = null;
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