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
    public Vector3S headDelta;

    public static HumanAnimPData Rand
    {
        get
        {
            return new HumanAnimPData
            {
                eyePosition = new Vector3(Random.Range(0.12f, 0.14f), Random.Range(-0.01f, 0.03f)),
                eyeScale = new Vector3(Random.Range(0.45f, 0.5f), Random.Range(0.45f, 0.5f)),
                skinColor = availableSkinColors[Random.Range(0, availableSkinColors.Length)],
                shirtColor = new Color(Random.value, Random.value, Random.value),
                bootsColor = Color.HSVToRGB(Random.value, 1f, 0.1f),
                headDelta = Vector3.zero,
            };
        }
    }

    public static HumanAnimPData Teacher
    {
        get
        {
            return new HumanAnimPData
            {
                eyePosition = new Vector3(Random.Range(0.12f, 0.14f), Random.Range(-0.01f, 0.03f)),
                eyeScale = new Vector3(Random.Range(0.45f, 0.5f), Random.Range(0.45f, 0.5f)),
                skinColor = availableSkinColors[Random.Range(0, availableSkinColors.Length)],
                shirtColor = Color.black,
                bootsColor = Color.black,
                headDelta = Vector3.up * 0.1f,
            };
        }
    }

    public static HumanAnimPData Enemy
    {
        get
        {
            var grayscale = Random.Range(0.3f, 0.5f);

            return new HumanAnimPData
            {
                eyePosition = new Vector3(Random.Range(0.17f, 0.19f), Random.Range(-0.05f, -0.02f)),
                eyeScale = new Vector3(Random.Range(0.42f, 0.46f), Random.Range(0.39f, 0.45f)),
                skinColor = availableSkinColors[Random.Range(0, availableSkinColors.Length)],
                shirtColor = new Color(grayscale, grayscale, grayscale),
                bootsColor = Color.HSVToRGB(Random.value, 1f, 0.06f),
                headDelta = Vector3.zero
            };
        }
    }
}