using UnityEngine;
using LLMUnity;
using System.Threading.Tasks;
using System;

public class LLMItemDescription : MonoBehaviour
{
    public static LLMItemDescription Instance;
    [SerializeField] private LLMCharacter llmCharacter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Hľadáme LLMCharacter v scéne
        llmCharacter = GetComponent<LLMCharacter>();
        if (llmCharacter == null)
        {
            llmCharacter = FindObjectOfType<LLMCharacter>(); // Nájde prvý LLMCharacter v scéne
        }

        if (llmCharacter == null)
        {
            Debug.LogError("LLMCharacter komponent nebol nájdený v scéne!");
        }
        else
        {
            Debug.Log("LLMCharacter bol úspešne nájdený!");
        }

        ItemData[] itemData = Resources.LoadAll<ItemData>("");
        foreach (ItemData item in itemData)
        {
            GenerateItemDescription(item);
        }
    }


    public async Task<string> GenerateItemDescription(ItemData item)
    {
        if (llmCharacter == null)
        {
            return "No description available.";
        }

        if (item == null)
        {
            return "No description available.";
        }

        string prompt = $"Describe an item called '{item.itemName}' that {item.itemUse}. Respond in one sentence.";

        // Vytvorenie Task-u na čakanie na odpoveď
        TaskCompletionSource<string> responseTask = new TaskCompletionSource<string>();

        _ = llmCharacter.Chat(prompt, (string reply) =>
        {
            if (!responseTask.Task.IsCompleted && !string.IsNullOrWhiteSpace(reply))
            {
                responseTask.TrySetResult(reply.Trim());
            }
            else
            {
                responseTask.TrySetResult("No description available.");
            }
        });

        // Čakáme na odpoveď
        string result = await responseTask.Task;

        item.itemDescription = result;

        return result;
    }

}
