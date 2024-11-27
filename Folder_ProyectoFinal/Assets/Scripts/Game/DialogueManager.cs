using UnityEngine;
using System;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI noOption;
    [SerializeField] private TextMeshProUGUI yesOption;
    [SerializeField] private GameObject nextOption;
    [SerializeField] private GameObject panelYesOrNo;
    [SerializeField] private GameObject finishDialogue;

    private DoubleCircularLinkedList<DialoguesNodes> dialogueNodes = new DoubleCircularLinkedList<DialoguesNodes>();
    private DoubleCircularLinkedList<DialoguesNodes>.Node currentNode;
    public event Action OnDialogueEnded;

    public NPCMovement npcMovement;

    public void EndDialogue()
    {
        dialogueUI.SetActive(false);
        Debug.Log("Diálogo terminado.");
        OnDialogueEnded?.Invoke();

        if (npcMovement != null)
        {
            npcMovement.ResumePatrol();
        }
    }

    public void StartDialogue(DoubleCircularLinkedList<DialoguesNodes> nodes)
    {
        if (nodes == null || nodes.Count == 0)
        {
            Debug.LogError("Lista de diálogos vacía.");
            dialogueUI.SetActive(false);
            return;
        }

        dialogueNodes = nodes;
        currentNode = dialogueNodes.Head;
        ShowCurrentDialogue();
    }

    public void OnCloseButtonClicked()
    {
        EndDialogue();
    }

    private void ShowCurrentDialogue()
    {
        if (currentNode == null || currentNode.Value == null)
        {
            EndDialogue();
            return;
        }

        dialogueUI.SetActive(true);
        dialogueText.text = currentNode.Value.Text;

        if (currentNode.Value.HasOptions)
        {
            nextOption.SetActive(false);
            panelYesOrNo.SetActive(true);
            noOption.text = currentNode.Value.noOption;
            yesOption.text = currentNode.Value.yesOption;
            finishDialogue.SetActive(false);
        }
        else
        {
            nextOption.SetActive(true);
            panelYesOrNo.SetActive(false);
            finishDialogue.SetActive(true);
        }
    }

    public void OnNextSelected()
    {
        if (currentNode.Value.HasOptions)
        {
            return;
        }
        AdvanceDialogue();
    }

    public void OnLeftOptionSelected()
    {
        HandleChoice(true);
    }

    public void OnRightOptionSelected()
    {
        HandleChoice(false);
    }

    private void HandleChoice(bool accepted)
    {
        string result = accepted ? "Ok, ¡gogogo!" : "Para la próxima.";
        dialogueText.text = result;
        AdvanceDialogue();
    }

    private void AdvanceDialogue()
    {
        if (currentNode.Next == null || currentNode.Next == dialogueNodes.Head)
        {
            EndDialogue();
            Debug.Log("Fin del diálogo.");
        }
        else
        {
            currentNode = currentNode.Next;
            ShowCurrentDialogue();
            Debug.Log("Avanzando al siguiente nodo.");
        }
    }
}
public class DialoguesNodes
{
    public string Text { get; set; }
    public string noOption { get; set; } 
    public string yesOption { get; set; }
    public bool HasOptions { get; set; } 

    public DialoguesNodes(string text, string leftOption = null, string rightOption = null, bool hasOptions = false)
    {
        Text = text;
        noOption = leftOption;
        yesOption = rightOption;
        HasOptions = hasOptions;
    }
}