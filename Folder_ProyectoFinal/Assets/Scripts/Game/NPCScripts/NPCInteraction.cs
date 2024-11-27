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
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueEnd += EndInteraction;
        }
    }

    private void OnDisable()
    {
        OnInteract -= InteractWithNPC;
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueEnd -= EndInteraction;
        }
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
            var rootNode = BuildDialogueTree();
            dialogueManager.StartDialogue(rootNode);
            Debug.Log("Diálogo iniciado");
        }
    }
    private void EndInteraction()
    {
        isInteracting = false;
        npcMovement.ResumePatrol();
        npcAnimator.SetTrigger("Walk"); 
    }
    private DialogueNode BuildDialogueTree()
    {
        var rootNode = new DialogueNode("NPC: hola te puedo preguntar algo");
        var node2 = new DialogueNode("TOMB RAIDER: dime");
        var node3 = new DialogueNode("NPC: Victor es el mas gozu?", "Sí", "No");
        var node4 = new DialogueNode("TOMB RAIDER: Obvio <3");
        var node5 = new DialogueNode("TOMB RAIDER: nahh, es realidad si es muy gozu y fachero");

        rootNode.SetYesNode(node2); 
        node2.SetYesNode(node3);    
        node3.SetYesNode(node4); 
        node3.SetNoNode(node5);   

        return rootNode; 
    }
}