using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chesty : Character
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        bool sleeping = animator.GetCurrentAnimatorStateInfo(0).IsName("Sleep");
        if (sleeping && other.gameObject.tag == "Player")
        {
            animator.SetTrigger("Awake");
        }
    }
}
