using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        takeDamageDelegate += GetHit;
        dieDelegate += Die;
    }

    public void GetHit()
    {
        animator.SetTrigger("Hit");
        Debug.Log("Ouch");
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.name);
    //}
}
