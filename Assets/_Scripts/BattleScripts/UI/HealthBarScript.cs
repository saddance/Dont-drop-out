using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;

    public Unit unit;

    private bool isFirstTime = true;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
    }

    private void Update()
    {
        if (unit == null) return;

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