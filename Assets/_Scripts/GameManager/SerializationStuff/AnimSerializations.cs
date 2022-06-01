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
    public Vector3S globalScale;
    public Vector3S bodyScale;

    public static HumanAnimPData Rand
    {
        get
        {
            return new HumanAnimPData
            {
                eyePosition = new Vector3(Random.Range(0.12f, 0.14f), Random.Range(-0.01f, 0.03f)),
                eyeScale = new Vector3(Random.Range(0.4f, 0.5f), Random.Range(0.4f, 0.5f)),
                skinColor = availableSkinColors[Random.Range(0, availableSkinColors.Length)],
                shirtColor = new Color(Random.value, Random.value, Random.value),
                bootsColor = Color.HSVToRGB(Random.value, 1f, 0.12f),
                globalScale = Vector3.one,
                bodyScale = Vector3.one
            };
        }
    }

    public static HumanAnimPData Enemy
    {
        get
        {
            var grayscale = Random.Range(0.3f, 0.6f);

            return new HumanAnimPData
            {
                eyePosition = new Vector3(Random.Range(0.15f, 0.17f), Random.Range(-0.04f, -0.01f)),
                eyeScale = new Vector3(Random.Range(0.4f, 0.5f), Random.Range(0.3f, 0.4f)),
                skinColor = availableSkinColors[Random.Range(0, availableSkinColors.Length)],
                shirtColor = new Color(grayscale, grayscale, grayscale),
                bootsColor = Color.HSVToRGB(Random.value, 1f, 0.06f),
                globalScale = Vector3.one,
                bodyScale = Vector3.one
            };
        }
    }
}