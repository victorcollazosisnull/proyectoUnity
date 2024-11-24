using UnityEngine;
using System;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI leftOptionText;
    [SerializeField] private TextMeshProUGUI rightOptionText;
    [SerializeField] private GameObject nextButton; 
    [SerializeField] private GameObject optionsPanel; 

    private DoubleCircularLinkedList<DialogueNode> dialogueNodes = new DoubleCircularLinkedList<DialogueNode>();
    private DoubleCircularLinkedList<DialogueNode>.Node currentNode;
    public event Action OnDialogueEnded;

    public void EndDialogue()
    {
        dialogueUI.SetActive(false);
        Debug.Log("Dialogo terminado");
        OnDialogueEnded?.Invoke();
    }

    public void StartDialogue(DoubleCircularLinkedList<DialogueNode> nodes)
    {
        if (nodes == null || nodes.Count == 0)
        {
            Debug.LogError("vacía");
            dialogueUI.SetActive(false);
            return;
        }

        dialogueNodes = nodes;
        currentNode = dialogueNodes.Head;
        ShowCurrentDialogue();
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
            nextButton.SetActive(false);  
            optionsPanel.SetActive(true); 
            leftOptionText.text = currentNode.Value.noOption;
            rightOptionText.text = currentNode.Value.yesOption;
        }
        else
        {
            nextButton.SetActive(true);   
            optionsPanel.SetActive(false); 
        }
    }

    public void OnNextSelected()
    {
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
        string nextText = accepted ? "Ok aquí vamos." : "Para la próxima.";
        currentNode = currentNode.Next; 

        if (currentNode != null)
        {
            currentNode.Value.Text = nextText; 
            ShowCurrentDialogue();

            optionsPanel.SetActive(false); 
            nextButton.SetActive(true);   
        }
        else
        {
            EndDialogue(); 
        }
    }

    private void AdvanceDialogue()
    {
        currentNode = currentNode.Next;

        if (currentNode == null || currentNode.Value == null || currentNode == dialogueNodes.Head)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentDialogue();
        }
    }
}
public class DialogueNode
{
    public string Text { get; set; }
    public string noOption { get; set; } 
    public string yesOption { get; set; }
    public bool HasOptions { get; set; } 

    public DialogueNode(string text, string leftOption = null, string rightOption = null, bool hasOptions = false)
    {
        Text = text;
        noOption = leftOption;
        yesOption = rightOption;
        HasOptions = hasOptions;
    }
}