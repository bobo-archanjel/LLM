using UnityEngine;
using TMPro;
using LLMUnity;

public class NPCchat : MonoBehaviour
{
    public GameObject chatUI;
    public TMP_Text npcReplyText;
    public GameObject player;

    private bool isChatting = false;
    private bool awaitingReply = false;
    private LLMCharacter llmCharacter;
    private Quest currentQuest;

    private void Awake()
    {
        llmCharacter = GetComponent<LLMCharacter>();
        if (llmCharacter == null)
        {
            Debug.LogError("LLMCharacter component chýba na NPC GameObject.");
        }
    }

    private void Update()
    {
        if (isChatting && Input.GetKeyDown(KeyCode.Escape))
        {
            EndChat();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartChat();
        }
    }

    public void StartChat()
    {
        if (!isChatting)
        {
            isChatting = true;
            chatUI.SetActive(true);
            player.GetComponent<PlayerMovement>().enabled = false;
            chatUI.GetComponent<ChatUI>().ShowChatUI();
        }
    }

    public void SendMessageToNPC(string playerMessage)
    {
        if (awaitingReply || string.IsNullOrWhiteSpace(playerMessage)) return;

        if (playerMessage.ToLower().Contains("give me task"))
        {
            GenerateQuestFromLLM();
        }else HandleReply(playerMessage);
    }

    private void GenerateQuestFromLLM()
    {
        string prompt = "Give the player a task involving one of the following actions: jump, collect, attack. Include a quantity.";
        _ = llmCharacter.Chat(prompt, reply =>
        {
            HandleGeneratedQuest(reply);
        });
    }

    private void HandleGeneratedQuest(string questText)
    {
        string cleanQuestText = questText.Trim();  // Odstránenie medzier a prázdnych znakov
        npcReplyText.text = cleanQuestText;
        Debug.Log($"NPC zadal úlohu: {cleanQuestText}");

        // 🟢 Oprava: Správne uloženie celej úlohy
        currentQuest = new Quest(cleanQuestText, cleanQuestText);
        QuestManager.Instance.AddQuest(currentQuest);
    }

    private void HandleReply(string prompt)
    {
        _ = llmCharacter.Chat(prompt, reply =>
        {
            npcReplyText.text = reply;
            Debug.Log($"[NPC]: {reply}");
        });
    }

    public void EndChat()
    {
        isChatting = false;
        chatUI.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
