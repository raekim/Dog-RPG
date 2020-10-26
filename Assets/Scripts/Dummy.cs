using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Monster
{
    new private void Awake()
    {
        base.Awake();

        getHitDelegate += GetHit;
        dieDelegate += Die;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        HPBar.Init("연습 표적", 0);
    }

    public void GetHit(int amount)
    {
        AddToHP(amount);
        animator.SetTrigger("Hit");
        Debug.Log("Dummy: Ouch!");
    }

    void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        animator.Play("Die");
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
