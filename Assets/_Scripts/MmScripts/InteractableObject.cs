using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool IsInittialized;

    public void Init()
    {
        IsInittialized = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!IsInittialized)
            //Debug.Log("InteractableObject is not initialized"); 
    }
}
