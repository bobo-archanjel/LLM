using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<ItemData> items = new List<ItemData>();

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

    private async void Start()
    {
        LLMItemDescription llmItemDescription = FindObjectOfType<LLMItemDescription>();

        if (llmItemDescription == null)
        {
            return;
        }

        ItemData testItem = Resources.Load<ItemData>("HealingPotion");

        if (testItem != null)
        {
            await AddItem(testItem, llmItemDescription);
        }
        else
        {
            Debug.LogError("HealingPotion sa nenašiel v Resources!");
        }
    }

    public async Task AddItem(ItemData newItem, LLMItemDescription llmItemDescription)
    {
        if (!items.Contains(newItem))
        {
            string description = await llmItemDescription.GenerateItemDescription(newItem);
            items.Add(newItem);

            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
            {
                inventoryUI.UpdateInventoryUI();
            }
        }
        else
        {
            Debug.Log($"Predmet {newItem.itemName} už je v inventári!");
        }
    }

}
