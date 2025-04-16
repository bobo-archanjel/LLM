using System.Drawing;
using UnityEngine;

public class PlayerActionTracker : MonoBehaviour
{
    private int jumpCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpCount++;
            Debug.Log($"Skok číslo: {jumpCount}");
            CheckJumpQuestCompletion();
        }
    }

    private void CheckJumpQuestCompletion()
    {
        Quest activeQuest = QuestManager.Instance.GetActiveQuest();

        if (activeQuest != null && !activeQuest.isCompleted) // Len ak úloha ešte nie je dokončená
        {
            Debug.Log("<color=green>" + activeQuest.questName + "</color>");
            string questName = activeQuest.questName.ToLower();
            Debug.Log($"Aktívna úloha: '{questName}' (Dĺžka: {questName.Length})");

            if ((questName == "j" || questName == "jump 2 times") && jumpCount >= 2)
            {
                QuestManager.Instance.CompleteQuest(activeQuest.questName);
                Debug.Log("Úloha 'Jump twice' bola splnená!");
            }
        }
        else if (activeQuest == null)
        {
            Debug.LogWarning("Žiadna aktívna úloha nie je nastavená.");
        }
    }

}
