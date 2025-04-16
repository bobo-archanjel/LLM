using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalBar;
    [SerializeField] private Image currentBar;

    private void Start()
    {
        totalBar.fillAmount = playerHealth.currentHealth / 10;

    }

    private void Update()
    {
        currentBar.fillAmount = playerHealth.currentHealth / 10;

    }
}
