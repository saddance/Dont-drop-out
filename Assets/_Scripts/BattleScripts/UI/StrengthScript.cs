using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrengthScript : MonoBehaviour
{
    public Text text;
    public Unit unit;

    void Awake()
    {
        unit = GetComponentInParent<Unit>();
    }

    void Update()
    {
        if (unit != null)
        {
            text.text = unit.Info.Strength.ToString();
        }
    }
}
