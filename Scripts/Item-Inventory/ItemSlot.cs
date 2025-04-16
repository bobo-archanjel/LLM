using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text itemNameText;
    public Sprite itemIcon;
    private ItemData item;

    public void SetItem(ItemData newItem)
    {
        item = newItem;
        itemNameText.text = item.itemName;
        itemIcon = item.itemIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            Debug.LogError("❌ `item` je null pri hover myšou! Skontroluj, či sa SetItem() volá pred OnPointerEnter().");
            return;
        }

        if (ItemDescriptionUI.Instance != null)
        {
            ItemDescriptionUI.Instance.ShowDescription(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemDescriptionUI.Instance != null)
        {
            ItemDescriptionUI.Instance.HideDescription();
        }
    }
}
