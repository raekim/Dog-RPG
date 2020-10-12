using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public float moveSpeed = .1f;
    public Attack[] playerAttacks;

    

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
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
        //Debug.Log("Idle");
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
        //Debug.Log("Run");
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
        //Debug.Log("Attack");
        animator.Play("Attack");
        animator.SetBool("Attacking", true);

        bool attacking = true;

        float delta = 0f;

        playerAttacks[0].gameObject.SetActive(true);
        playerAttacks[0].AttackStart();

        while (attacking)
        {
            yield return null;

            //attacking = playerAttacks[0].attacking;

            delta += Time.deltaTime;
            
            playerAttacks[0].UpdateAttack(leftMouseClicked);  // 나중에 싱글톤 플레이어 컨트롤 인풋 클래스가 작성되면 그것을 사용
            
            if (delta >= playerAttacks[0].attackLength)
            {
                if(playerAttacks[0].attacking)
                {
                    delta = 0f;
                    playerAttacks[0].AttackStart();
                }
                else
                {
                    attacking = false;
                }
            }
        }

        playerAttacks[0].gameObject.SetActive(false);

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
