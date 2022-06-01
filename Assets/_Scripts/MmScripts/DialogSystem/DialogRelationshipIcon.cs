using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogRelationshipIcon : MonoBehaviour
{
    [SerializeField] private Text smile;
    [SerializeField] private Text score;

    public void Init(Personality pers)
    {
        if (pers.asFriend == null || pers.asFriend.State == null)
            gameObject.SetActive(false);
        else
        {
            var state = pers.asFriend.State;
            smile.text = new string(state.chr, 1);
            score.text = $"{pers.asFriend.friendScore}";
        }
    }
}

