using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;
    private Enemy holder;
    private void OnEnable()
    {
        EventBus.OnEntityReceivedDamage += UpdateHealthBar;
    }
    private void OnDisable()
    {
        EventBus.OnEntityReceivedDamage -= UpdateHealthBar;
    }

    private void Start()
    {
        healthSlider = GetComponentInChildren<Slider>();
        holder = GetComponentInParent<Enemy>();
        healthSlider.value = 1;
    }

    public void UpdateHealthBar(Enemy receivingEnemy)
    {
        if (receivingEnemy == holder)
        {
            healthSlider.value = receivingEnemy.GetCurrentHP()/receivingEnemy.Data.HealthPoints;
        }
    }
}