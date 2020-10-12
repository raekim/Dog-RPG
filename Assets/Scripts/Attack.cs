using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [HideInInspector]
    public float attackLength;
    [HideInInspector]
    public bool attacking;

    public virtual void AttackStart()
    {
    }

    public virtual void UpdateAttack(bool leftMouseClicked)
    {
    }
}
