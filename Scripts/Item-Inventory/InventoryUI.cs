using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform inventoryPanel; // Grid Layout, kde sa budú zobrazovať predmety
    public GameObject itemSlotPrefab; // Prefab slotu pre itemy
    public TMP_Text descriptionText; // UI pre zobrazenie popisu

    private void Start()
    {
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        Debug.Log($"📦 Aktualizujem inventár, počet itemov: {Inventory.Instance.items.Count}");

        // Odstránime všetky predchádzajúce sloty
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        // Pridáme nové sloty
        int slotCount = 0;
        foreach (ItemData item in Inventory.Instance.items)
        {
            Debug.Log($"🛠️ Pridávam item: {item.itemName}");

            GameObject slot = Instantiate(itemSlotPrefab, inventoryPanel);
            slotCount++;

            ItemSlot slotScript = slot.GetComponent<ItemSlot>();
            if (slotScript != null)
            {
                slotScript.SetItem(item);
            }
            else
            {
                Debug.LogError("❌ ItemSlot skript nie je priradený k prefab itemu!");
            }
        }

        Debug.Log($"✅ Vytvorené sloty: {slotCount}");
    }


}
