using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = .1f;

    Animator animator;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir.Normalize();    // 대각선 이동 감안하여 벡터 길이를 1로 정규화

        bool running = dir.magnitude > 0f;  // 캐릭터 이동
        animator.SetBool("Running", running);

        if (running)
        {
            rb.MovePosition(transform.position + dir * moveSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
