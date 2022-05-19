using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HumanAnimPData
{
    public static Color[] availableSkinColors = new Color[]
    {
        new Color(0.895f, 0.625963f, 0.2971399f)
    };

    public ColorS skinColor;
    public ColorS shirtColor;
    public ColorS bootsColor;
    public Vector3S eyePosition;
    public Vector3S eyeScale;


    public static HumanAnimPData Default
    {
        get
        {
            return new HumanAnimPData
            {
                eyePosition = new Vector3(Random.Range(0.1f, 0.16f), Random.Range(-0.01f, 0.03f)),
                eyeScale = new Vector3(Random.Range(0.4f, 0.6f), Random.Range(0.4f, 0.6f)),
                skinColor = availableSkinColors[Random.Range(0, availableSkinColors.Length)],
                shirtColor = new Color(Random.value, Random.value, Random.value),
                bootsColor = Color.HSVToRGB(Random.value, 1f, 0.12f)
            };
        }
    }
}