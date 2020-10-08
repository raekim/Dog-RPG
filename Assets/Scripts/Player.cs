using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = .1f;

    Animator animator;
    Rigidbody rb;

    Vector3 dir;
    bool leftMouseClicked;

    enum State
    {
        Idle,
        Run,
        Attack
    }

    State currentState;
    State beforeState;

    bool stateChanged;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        currentState = beforeState = State.Idle;

        StartCoroutine(FSM());
    }

    void Update()
    {
        Control();
    }

    void Control()
    {
        leftMouseClicked = Input.GetKeyDown(KeyCode.Mouse0);

        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir.Normalize();    // 방향만 필요하기 때문에 벡터 정규화
    }

    void ChangeState(State nextState)
    {
        stateChanged = true;
        beforeState = currentState;
        currentState = nextState;
    }

    IEnumerator FSM()
    {
        while(true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    IEnumerator Idle()
    {
        Debug.Log("Idle");
        stateChanged = false;

        while (!stateChanged)
        {
            yield return null;

            if (GetWASDKeyDown())
            {
                ChangeState(State.Run);
            }

            if (leftMouseClicked)
            {
                ChangeState(State.Attack);
            }
        }
    }

    bool GetWASDKeyDown()
    {
        return dir.magnitude > 0f;
    }

    IEnumerator Run()
    {
        Debug.Log("Run");
        animator.SetBool("Running", true);
        stateChanged = false;

        while (!stateChanged)
        {
            yield return null;

            rb.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);

            if (!GetWASDKeyDown())
            {
                ChangeState(State.Idle);
            }

            if(leftMouseClicked)
            {
                ChangeState(State.Attack);
            }
        }

        animator.SetBool("Running", false);
    }

    IEnumerator Attack()
    {
        Debug.Log("Attack");
        animator.Play("Attack");
        animator.SetBool("Attacking", true);

        bool attacking = true;

        float attackLength = .7f;
        float delta = 0f;

        while (attacking)
        {
            yield return null;

            delta += Time.deltaTime;

            if (delta >= attackLength)
            {
                attacking = false;
            }

            // 시간 내에 마우스 클릭하면 공격 유지
            if (leftMouseClicked && delta > .3f)
            {
                delta = 0f;
            }  
        }

        if (GetWASDKeyDown())
        {
            ChangeState(State.Run);
        }
        else
        {
            ChangeState(State.Idle);
        }

        animator.SetBool("Attacking", false);
    } 
}
