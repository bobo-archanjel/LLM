using UnityEngine;
using TMPro;

public class ChatUI : MonoBehaviour
{
    public TMP_Text npcReplyText; // Text for NPC's reply
    public TMP_InputField playerInputField; // InputField for player's input
    public GameObject chatPanel; // Panel UI for showing/hiding chat
    public NPCchat npcChat; // Reference to NPCchat script

    public void ShowChatUI()
    {
        if (chatPanel == null)
        {
            Debug.LogError("ChatPanel is not assigned in the Inspector!");
            return;
        }

        chatPanel.SetActive(true); // Activate the chat panel
        playerInputField.Select(); // Focus on the input field
        Debug.Log("Chat UI activated.");
    }

    public void HideChatUI()
    {
        if (chatPanel == null)
        {
            Debug.LogError("ChatPanel is not assigned in the Inspector!");
            return;
        }

        chatPanel.SetActive(false); // Deactivate the chat panel
        npcReplyText.text = ""; // Clear NPC's reply
        Debug.Log("Chat UI hidden.");
    }

    public void OnSendButtonClicked()
    {
        string playerMessage = playerInputField.text.Trim();

        if (string.IsNullOrWhiteSpace(playerMessage))
        {
            Debug.LogWarning("Player entered an empty message.");
            return;
        }

        if (npcChat == null)
        {
            Debug.LogError("NPCchat reference is missing in ChatUI.");
            return;
        }

        npcChat.SendMessageToNPC(playerMessage); // Send message to NPCchat
        playerInputField.text = ""; // Clear the input field
    }

    public void DisplayNPCReply(string reply)
    {
        if (npcReplyText == null)
        {
            Debug.LogError("NPCReplyText is not assigned in the Inspector!");
            return;
        }

        npcReplyText.text = reply; // Display NPC's reply in the UI
        Debug.Log($"NPC replied: {reply}");
    }
}
