using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;
    private Entity holder;
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
        holder = GetComponentInParent<Entity>();
        healthSlider.value = 1;
    }

    public void UpdateHealthBar(Entity receivingEntity)
    {
        if (receivingEntity == holder)
        {
            healthSlider.value = receivingEntity.GetHP()/receivingEntity.data.healthPoints;
        }
    }
}