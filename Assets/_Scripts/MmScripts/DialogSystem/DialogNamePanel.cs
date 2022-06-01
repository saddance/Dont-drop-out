using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogNamePanel : MonoBehaviour
{
    public Text text;
    private RectTransform rTr;
    [SerializeField] private float additionalWidth = 20f;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        rTr = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        rTr.sizeDelta = new Vector2(Mathf.Max(rTr.sizeDelta.x, additionalWidth + text.rectTransform.sizeDelta.x), rTr.sizeDelta.y);
    }
}
