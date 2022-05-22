using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private PlayerTurnManager ptm;
    void Start()
    {
        ptm = GetComponentInParent<PlayerTurnManager>();
    }
    public void Attack()
    {
        ptm.TryToAttack();
    }

    public void Deselect()
    {
        ptm.Deselect();
    }
}
