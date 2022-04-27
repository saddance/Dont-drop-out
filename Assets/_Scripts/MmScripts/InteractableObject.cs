using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool IsInittialized;
    public Personality personality { get; private set; }

    private void Update()
    {
        if (!IsInittialized)
            Debug.Log("InteractableObject is not initialized");
    }

    public void Init(Personality personality)
    {
        IsInittialized = true;
        this.personality = personality;
    }
}