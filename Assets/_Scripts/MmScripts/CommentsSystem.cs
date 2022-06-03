using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentsSystem : MonoBehaviour
{
    public static CommentsSystem instance;
    [SerializeField] float commentDuration;
    [SerializeField] float commentDelay;
    private Text text;

    List<string> comments = new List<string>();

    private void Awake()
    {
        text = GetComponent<Text>();
        instance = this;
    }

    private void Start()
    {
        if (GameManager.currentSave.startComment != null)
        {
            AddComment(GameManager.currentSave.startComment);
            GameManager.currentSave.startComment = null;
        }
    }

    public void AddComment(string comment)
    {
        StartCoroutine(PostComment(comment));
    }

    IEnumerator PostComment(string comment)
    {
        yield return new WaitForSeconds(commentDelay);
        comments.Add(comment);
        yield return new WaitForSeconds(commentDuration);
        comments.Remove(comment);
    }

    private void LateUpdate()
    {
        text.text = string.Join("\n", comments.ToArray());   
    }
}
