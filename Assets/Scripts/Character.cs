using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isInvincible;

    protected int maxHP;
    protected int currentHP;

    protected Animator animator;
    protected Rigidbody rb;

    protected delegate void TakeDamageDelegate();
    protected TakeDamageDelegate takeDamageDelegate;

    public delegate void DieDelegate();
    public DieDelegate dieDelegate;

    protected void SetMaxHP(int amount)
    {
        maxHP = 0;
        if (amount < 0) return;
        maxHP = amount;
    }

    protected void FillUpHPToMax()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP < 0)
        {
            currentHP = 0;
            // 몬스터 사망
            if (!isInvincible)
            {
                if(dieDelegate != null) dieDelegate();
            }
        }

        if(takeDamageDelegate != null) takeDamageDelegate();
    }
}
