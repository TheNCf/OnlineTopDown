using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealthBar : MonoBehaviour
{
    private Slider _healthBar;

    private void Awake()
    {
        _healthBar = GetComponent<Slider>();
    }

    public void SetValue(int health)
    {
        _healthBar.value = health;
    }

    public void Setup(int maxHealth)
    {
        _healthBar.maxValue = maxHealth;
        SetValue(maxHealth);
    }
}
