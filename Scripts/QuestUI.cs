using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance;

    public GameObject questPanel;
    public TMP_Text questNameText;
    public TMP_Text questDescriptionText;
    public TMP_Text questStatusText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowQuest(Quest quest)
    {
        if (questPanel != null)
        {
            questPanel.SetActive(true);
            questNameText.text = quest.questName;
            questDescriptionText.text = quest.description;
            questStatusText.text = quest.isCompleted ? "Status: Splnená!" : "Status: Nesplnená";
        }
        else
        {
            Debug.LogError("Quest Panel nie je priradený.");
        }
    }

    public void CompleteQuestUI(Quest quest)
    {
        if (questStatusText != null)
        {
            questStatusText.text = "Status: Splnená!";
            Debug.Log("Status úlohy v UI bol aktualizovaný na: Splnená!");
        }
        else
        {
            Debug.LogError("Quest Status Text nie je priradený.");
        }
    }

    public void HideQuestUIAfterDelay(float delay)
    {
        if (questPanel != null)
        {
            StartCoroutine(HidePanelAfterDelay(delay));
        }
    }

    private System.Collections.IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (questPanel != null)
        {
            questPanel.SetActive(false);
            Debug.Log("Quest panel bol skrytý.");
        }
    }

}
