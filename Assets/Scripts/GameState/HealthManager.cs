using System.Collections;
using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 10;
    public TextMeshProUGUI healthText; // UI element to display health

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UpdateHealthUI();

        Debug.Log($"Lost {amount} HP. Current HP: {currentHealth}");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
        Debug.Log($"Healed {amount} HP. Current HP: {currentHealth}");
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth}";
        }
    }
}
