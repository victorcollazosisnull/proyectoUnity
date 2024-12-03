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
    public bool isMainNPC = false;
    public event Action OnInteract;
    public DialogueManager dialogueManager;
    [Header("Scriptable Objetc Dialogues")]
    public NPCDialogueSO nPCDialogue;

    private void Start()
    {
        interactText.gameObject.SetActive(false);
        if (isMainNPC)
        {
            npcAnimator.SetTrigger("Idle");
        }
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
            DialogueNode rootNode;

            if (isMainNPC) 
            {
                rootNode = BuildDialogueTree();
            }
            else 
            {
                rootNode = BuildDialogueTreeWithTextOnly();
            }

            dialogueManager.StartDialogue(rootNode);
        }
    }

    private DialogueNode BuildDialogueTreeWithTextOnly()
    {
        if (nPCDialogue == null)
        {
            Debug.LogError("ScriptableObject missing :(");
            return null;
        }
        var rootNode = new DialogueNode(nPCDialogue.dialogueText); 
        rootNode.SetYesNode(null); 
        return rootNode;
    }
    private void EndInteraction()
    {
        if (isMainNPC)
        {
            npcAnimator.SetTrigger("Idle");
        }
        else
        {
            npcMovement.ResumePatrol();
            npcAnimator.SetTrigger("Walk");
        }
        isInteracting = false;
    }
    private DialogueNode BuildDialogueTree()
    {
        var rootNode = new DialogueNode("NPC: hola te puedo preguntar algo");
        var node2 = new DialogueNode("TOMB RAIDER: dime");
        var node3 = new DialogueNode("NPC: Victor es el mas gozu?", "Sí", "Por supuesto");
        var node4 = new DialogueNode("TOMB RAIDER: Obvio <3");
        var node5 = new DialogueNode("TOMB RAIDER: como debe ser");

        rootNode.SetYesNode(node2); 
        node2.SetYesNode(node3);    
        node3.SetYesNode(node4); 
        node3.SetNoNode(node5);   

        return rootNode; 
    }
}