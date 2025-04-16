using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemDescriptionUI : MonoBehaviour
{
    public static ItemDescriptionUI Instance;

    public GameObject descriptionPanel;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public Image itemIcon;

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

        if (descriptionPanel == null)
        {
            Debug.LogError("descriptionPanel nie je priradený! Uisti sa, že je správne nastavený v Inspector.");
            return;
        }

        descriptionPanel.SetActive(false); // Skryť popis pri štarte
    }

    public void ShowDescription(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("Chýba referencia na ItemData pri zobrazovaní popisu!");
            return;
        }

        // Zabezpečíme, že UI sa zapne len ak už nie je zapnuté
        if (!descriptionPanel.activeSelf)
        {
            itemNameText.text = item.itemName;
            itemDescriptionText.text = item.itemDescription;
            itemIcon.sprite = item.itemIcon;

            descriptionPanel.SetActive(true);
        }
    }

    public void HideDescription()
    {
        // Skryť UI len ak je aktívne (zabraňuje blikaniu)
        if (descriptionPanel.activeSelf)
        {
            descriptionPanel.SetActive(false);
        }
    }
}
