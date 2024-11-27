using UnityEngine;

public class DialogueNode 
{
    public string Text { get; private set; }
    public string OptionYes { get; private set; }
    public string OptionNo { get; private set; }

    public DialogueNode YesNode { get; private set; }
    public DialogueNode NoNode { get; private set; }

    public DialogueNode(string text, string optionYes = null, string optionNo = null)
    {
        Text = text;
        OptionYes = optionYes;
        OptionNo = optionNo;
    }

    public void SetYesNode(DialogueNode yesNode)
    {
        YesNode = yesNode;
    }

    public void SetNoNode(DialogueNode noNode)
    {
        NoNode = noNode;
    }
}