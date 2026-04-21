using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health PlayerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        totalHealthBar.fillAmount = PlayerHealth.currentHealth / 10;
    }

    private void Update()
    {
        currentHealthBar.fillAmount = PlayerHealth.currentHealth / 10; // Fill amount is divided by 10, eg. 1 = 10 health, 0.3 = 3 health
    }
}
