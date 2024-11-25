using UnityEngine;
using System;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC Settings")]
    public TextMeshProUGUI interactText;
    public float interactionRadius = 3f;
    [Header("NPC References")]
    public NPCMovement npcMovement;
    public Animator npcAnimator;
    public Transform player;
    private bool playerClose = false;
    private bool isInteracting = false;

    public event Action OnInteract;
    public DialogueManager dialogueManager; 

    private void Start()
    {
        interactText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnInteract += InteractWithNPC;
    }

    private void OnDisable()
    {
        OnInteract -= InteractWithNPC;
    }

    private void Update()
    {
        if (playerClose && !npcMovement.isInteracting)
        {
            interactText.gameObject.SetActive(true);
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
        if (isInteracting)
        {
            Vector3 lookingPlayer = player.position - transform.position;
            lookingPlayer.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookingPlayer), Time.deltaTime * 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = false;
            interactText.gameObject.SetActive(false);
        }
    }

    public void Interacting()
    {
        if (playerClose && interactText.gameObject.activeInHierarchy)
        {
            OnInteract?.Invoke();
        }
    }

    private void InteractWithNPC()
    {
        isInteracting = true;
        npcMovement.StopPatrol();
        npcAnimator.SetTrigger("Idle");

        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(GetDialogueNodes()); 
        }
    }

    private DoubleCircularLinkedList<DialogueNode> GetDialogueNodes()
    {
        var dialogueList = new DoubleCircularLinkedList<DialogueNode>();

        dialogueList.InsertAtEnd(new DialogueNode("¡Hola, next ?"));
        dialogueList.InsertAtEnd(new DialogueNode("Qué harás", "Sí", "No", true));

        return dialogueList;
    }
}