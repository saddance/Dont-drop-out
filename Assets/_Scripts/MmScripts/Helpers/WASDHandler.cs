using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WASDHandler
{
    private static readonly KeyCode[] handlingCodes = {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};

    private List<Tuple<KeyCode, float>> pressedButtons = new List<Tuple<KeyCode, float>>();

    public KeyCode? GetPressedButton()
    {
        foreach (var code in handlingCodes)
            if (Input.GetKey(code))
            {
                if (pressedButtons.Count(x => x.Item1 == code) == 0)
                    pressedButtons.Add(Tuple.Create(code, Time.time));
            }
            else
            {
                pressedButtons = pressedButtons.Where(x => x.Item1 != code).ToList();
            }

        if (pressedButtons.Count == 0)
            return null;
        return pressedButtons.OrderBy(x => x.Item2).Last().Item1;
    }
}