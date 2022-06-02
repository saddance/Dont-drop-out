using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunningText : MonoBehaviour
{
    [TextArea] public string text;
    public int textSpeed;
    public float delay;
    private Text uitext;
    private int shown = 0;
    
    void Start()
    {
        uitext = GetComponent<Text>();
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(delay);
        float startTime = Time.time;
        while (true)
        {
            shown = Mathf.RoundToInt((Time.time - startTime) * textSpeed);
            uitext.text = text.Substring(0, Mathf.Min(text.Length, shown));
            if (shown > text.Length)
                break;
            yield return null;
        }
    }
}
