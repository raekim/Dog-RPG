﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isInvincible;

    protected bool isAlive;
    protected int maxHP;
    protected int currentHP;

    protected Animator animator;
    protected Rigidbody rb;

    protected delegate void GetHitDelegate(int damage);
    protected GetHitDelegate getHitDelegate;

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

    protected void AddToHP(int amount)
    {
        currentHP += amount;
        Mathf.Clamp(currentHP, 0, maxHP);
    }

    public void TakeDamage(int amount)
    {
        if(getHitDelegate != null) getHitDelegate(amount);

        // 몬스터 사망
        if (!isInvincible && currentHP == 0)
        {
            if (dieDelegate != null) dieDelegate();
        }
    }
}
