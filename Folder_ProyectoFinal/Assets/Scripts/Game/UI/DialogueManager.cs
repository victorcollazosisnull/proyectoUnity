using UnityEngine;
using System;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private LaraCroftMovement movement;
    [SerializeField] private LaraCroftInputReader inputReader;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI noOption;
    [SerializeField] private TextMeshProUGUI yesOption;
    [SerializeField] private GameObject panelYesOrNo;
    [SerializeField] private GameObject colliderObject;
    private DialogueNode currentNode;

    public event Action OnDialogueEnd;

    public void StartDialogue(DialogueNode rootNode)
    {
        if (rootNode == null)
        {
            Debug.LogError("Nodo nulo");
            return;
        }
        dialogueUI.SetActive(true);
        panelYesOrNo.SetActive(false); 
        currentNode = rootNode;
        ShowCurrentDialogue();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inputReader.BlockInputs(true);
        movement.StopMovement();
        colliderObject.GetComponent<Collider>().enabled = true;
    }

    private void ShowCurrentDialogue()
    {
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = currentNode.Text;

        if (currentNode.OptionYes != null || currentNode.OptionNo != null)
        {
            panelYesOrNo.SetActive(true);
            nextButton.SetActive(false);
            yesOption.text = currentNode.OptionYes;
            noOption.text = currentNode.OptionNo;
        }
        else
        {
            panelYesOrNo.SetActive(false);
            nextButton.SetActive(true);
            yesOption.text = "";
            noOption.text = "";
        }

        if (currentNode.YesNode == null && currentNode.NoNode == null)
        {
            colliderObject.GetComponent<Collider>().enabled = true;
        }
    }

    public void OnYesOptionSelected()
    {
        if (currentNode.YesNode != null)
        {
            if (colliderObject != null)
            {
                colliderObject.GetComponent<Collider>().enabled = false;
                Debug.Log("desactivado");
            }
            currentNode = currentNode.YesNode;
            ShowCurrentDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    public void OnNoOptionSelected()
    {
        if (currentNode.NoNode != null)
        {
            colliderObject.GetComponent<Collider>().enabled = true;
            Debug.Log("no puedes bajo");
            currentNode = currentNode.NoNode;
            ShowCurrentDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
        panelYesOrNo.SetActive(false);
        nextButton.SetActive(false);
        Debug.Log("Di�logo terminado.");
        OnDialogueEnd?.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputReader.BlockInputs(false);
        movement.ResumeMovement();
    }
}