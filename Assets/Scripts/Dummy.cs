using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character
{
    private void Awake()
    {
        getHitDelegate += GetHit;
        dieDelegate += Die;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
