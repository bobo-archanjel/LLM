using System;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static UnityEditor.Progress;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private List<Quest> activeQuests = new List<Quest>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddQuest(Quest newQuest)
    {
        Debug.Log("<color=red>" + newQuest.questName + "</color>");
        activeQuests.Add(newQuest);
        Debug.Log($"Nový quest pridaný: {newQuest.questName}");

        if (QuestUI.Instance != null)
        {
            QuestUI.Instance.ShowQuest(newQuest);
        }
    }

    public Quest GetActiveQuest()
    {
        foreach (Quest item in activeQuests)
        {
            Debug.Log("<color=yellow>" + item.questName + "</color>");
        }
        
        return activeQuests.Count > 0 ? activeQuests[0] : null;
    }

    public void CompleteQuest(string questName)
    {
        Quest quest = activeQuests.Find(q => q.questName.ToLower().Trim() == questName.ToLower().Trim());
        if (quest != null && !quest.isCompleted)
        {
            quest.isCompleted = true;
            Debug.Log($"Quest splnený: {quest.questName}");

            // Aktualizácia UI
            if (QuestUI.Instance != null)
            {
                QuestUI.Instance.CompleteQuestUI(quest);

                // Skrytie panelu po chvíľke
                QuestUI.Instance.HideQuestUIAfterDelay(3f); // 3 sekundy
            }
        }
    }

}
