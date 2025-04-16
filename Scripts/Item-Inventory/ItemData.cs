using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string itemUse; // Manuálne definované použitie (napr. "Heals 20 HP")
    [TextArea] public string itemDescription; // Automaticky generovaný popis LLM
    public Sprite itemIcon; // Obrázok predmetu
}
