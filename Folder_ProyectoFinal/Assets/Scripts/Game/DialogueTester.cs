using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    private void Start()
    {
        var dialogueList = new DoubleCircularLinkedList<DialogueNode>();

        dialogueList.InsertAtEnd(new DialogueNode("Hola bienvenido."));
        dialogueList.InsertAtEnd(new DialogueNode("Quieres aceptar una misión?", "Sí", "No", true));

        dialogueManager.StartDialogue(dialogueList);
    }
}
