using UnityEngine;
using UnityEngine.UI;

public class StrengthScript : MonoBehaviour
{
    public Text text;
    public Unit unit;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
    }

    private void Update()
    {
        if (unit != null) text.text = unit.Info.Strength.ToString();
    }
}