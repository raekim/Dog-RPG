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

        takeDamageDelegate = new TakeDamageDelegate(GetHit);
    }

    public void GetHit()
    {
        animator.SetTrigger("Hit");
        Debug.Log("Ouch");
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.name);
    //}
}
