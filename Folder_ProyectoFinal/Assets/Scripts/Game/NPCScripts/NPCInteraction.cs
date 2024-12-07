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
        npcMovement.isInteracting = false;
    }
    private DialogueNode BuildDialogueTree()
    {
        var rootNode = new DialogueNode("LARA: What happened here? This place looks abandoned... And those marks?");
        var node2 = new DialogueNode("NPC: Traveler, leave while you can. This city isn’t safe.");
        var node3 = new DialogueNode("LARA: I’m looking for an artifact buried nearby.");
        var node4 = new DialogueNode("NPC: The Eye of Anubis? That cursed object caused all this.");
        var node5 = new DialogueNode("LARA: Cursed? Sounds like a myth to me.");
        var node6 = new DialogueNode("NPC: It’s real! It awakened the tomb guardians. They destroyed this village.");
        var node7 = new DialogueNode("NPC: If you find it, will you use it? Risk unleashing its power?", "YES", "NO");
        var node8 = new DialogueNode("LARA: I’m ready to use it.");
        var node9 = new DialogueNode("NPC: Perhaps you’re destined to end this nightmare.");
        var node10 = new DialogueNode("LARA: No, I don’t believe in myths. This isn’t worth it.");
        var node11 = new DialogueNode("NPC: If you change your mind, the tomb awaits. Farewell.");

        rootNode.SetYesNode(node2);
        node2.SetYesNode(node3);
        node3.SetYesNode(node4);
        node4.SetYesNode(node5);
        node5.SetYesNode(node6);
        node6.SetYesNode(node7);
        node7.SetYesNode(node8);
        node7.SetNoNode(node10);
        node8.SetYesNode(node9);
        node10.SetYesNode(node11);

        return rootNode;
    }
}