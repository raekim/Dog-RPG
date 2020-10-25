using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Transform modelTransform;
    public float moveSpeed = .1f;
    public Attack[] playerAttacks;

    Vector3 dir;

    enum State
    {
        Idle,
        Run,
        Attack
    }

    State currentState;
    bool stateChanged;

    new private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Idle;
        StartCoroutine(FSM());
    }

    void ChangeState(State nextState)
    {
        stateChanged = true;
        currentState = nextState;
    }

    IEnumerator FSM()
    {
        while(true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    //bool rotating;
    //IEnumerator RotateTowards()
    //{
    //
    //}

    void RotateModelTowardsDir(float t)
    {
        modelTransform.LookAt(modelTransform.position + dir);
    }

    IEnumerator Idle()
    {
        //Debug.Log("Idle");
        stateChanged = false;

        while (!stateChanged)
        {
            yield return null;

            if (ControlManager.Instance.GetPlayerMoveKeyDown(out dir))
            {
                ChangeState(State.Run);
            }

            if (ControlManager.Instance.GetPlayerAttackButtonDown())
            {
                ChangeState(State.Attack);
            }
        }
    }

    IEnumerator Run()
    {
        //Debug.Log("Run");
        animator.SetBool("Running", true);
        stateChanged = false;
        float cnt = 0f;

        while (!stateChanged)
        {
            yield return null;

            // 달리는 방향을 바라본다
            RotateModelTowardsDir(cnt);
            cnt += Time.deltaTime;

            rb.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);
            if (!ControlManager.Instance.GetPlayerMoveKeyDown(out dir))
            {
                ChangeState(State.Idle);
            }

            if(ControlManager.Instance.GetPlayerAttackButtonDown())
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
            
            playerAttacks[0].UpdateAttack(ControlManager.Instance.GetPlayerAttackButtonDown());  // 나중에 싱글톤 플레이어 컨트롤 인풋 클래스가 작성되면 그것을 사용
            
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

        if (ControlManager.Instance.GetPlayerMoveKeyDown(out dir))
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
