using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    private SpriteRenderer srend;

    public Color color
    {
        set
        {
            if (srend == null)
                srend = GetComponent<SpriteRenderer>();
            srend.color = value;
        }
        get
        {
            if (srend == null)
                srend = GetComponent<SpriteRenderer>();
            return srend.color;
        }
    }
}
