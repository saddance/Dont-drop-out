using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool IsInittialized;
    public Personality personality { get; private set; }

    public void Init(Personality personality)
    {
        IsInittialized = true;
        this.personality = personality;
    }
    
    void Update()
    {
        if (!IsInittialized)
            Debug.Log("InteractableObject is not initialized");
    }
}
