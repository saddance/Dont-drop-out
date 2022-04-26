using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class HealthBarScript : MonoBehaviour
{
    public Slider slider;

    public Unit unit;
    
    void Awake()
    {
        unit = GetComponentInParent<Unit>();
    }

    private bool isFirstTime = true;
    void Update()
    {
        if (unit == null)
        {
            return;
        }

        if (isFirstTime)
        {
            SetMaxHealth(unit.Info.Health);
            isFirstTime = false;
        }
        
        SetHealth(unit.Info.Health);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}