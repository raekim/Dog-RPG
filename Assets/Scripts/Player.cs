using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Transform modelTransform;
    public float moveSpeed = .1f;
    public Attack[] playerAttacks;

    public PlayerHealthBar HPBar;
    public int playerMaxHP;

    Vector3 dir;

    enum State
    {
        Idle,
        Run,
        Attack,
        Stunned
    }

    State currentState;
    bool stateChanged;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        getHitDelegate += GetHit;
        dieDelegate += Die;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Idle;

        maxHP = playerMaxHP;
        // 캐릭터 체력 초기화
        SetMaxHP(maxHP);
        FillUpHPToMax();
        isAlive = true;

        StartCoroutine(FSM());
    }

    override protected void HPChanged()
    {
        if (HPBar != null)
        {
            HPBar.HealthDisplay((float)currentHP / maxHP, currentHP, maxHP);
        }
    }

    override public void TakeDamage(int amount)
    {
        if (getHitDelegate != null)
        {
            getHitDelegate(amount);
            HPChanged();
        }

        // 캐릭터 사망
        if (!isInvincible && currentHP == 0)
        {
            if (dieDelegate != null) dieDelegate();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 테스트용 데미지
            TakeDamage(-10);
        }
    }

    void GetHit(int amount)
    {
        if (!isAlive) return;   // 죽었을 때 피격되지 않는다

        Debug.Log("Player: Ouch!!");
        AddToHP(amount);
        animator.SetTrigger("Hit");

        ChangeState(State.Stunned);

        DamageSkinManager.Instance.DisplayDamage(-amount, transform.position + Vector3.up * 1.5f);
    }

    void Die()
    {
        if (!isAlive) return;   // 두 번 죽지 않는다

        isAlive = false;
        StopAllCoroutines();
        animator.SetTrigger("Die");
        //GetComponentInChildren<BoxCollider>().enabled = false;
    }

    IEnumerator Stunned()
    {
        // 플레이어 피격 시 잠시 아무것도 못 한다
        Debug.Log("Stunned");
        yield return new WaitForSeconds(.5f);
        ChangeState(State.Idle);
    }

    void ChangeState(State nextState)
    {
        if (currentState == State.Stunned)
        {
            if (nextState != State.Idle && nextState != State.Stunned) return;
        }

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

    void RotateModelTowardsDir(float t)
    {
        modelTransform.LookAt(modelTransform.position + dir);
    }

    IEnumerator Idle()
    {
        Debug.Log("Idle");
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
        Debug.Log("Run");
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
        Debug.Log("Attack");
        animator.Play("Attack");
        animator.SetBool("Attacking", true);

        bool attacking = true;
        stateChanged = false;

        float delta = 0f;

        playerAttacks[0].gameObject.SetActive(true);
        playerAttacks[0].AttackStart();

        while (!stateChanged && attacking)
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

        if(!stateChanged)
        {
            if (ControlManager.Instance.GetPlayerMoveKeyDown(out dir))
            {
                ChangeState(State.Run);
            }
            else
            {
                ChangeState(State.Idle);
            }
        }

        animator.SetBool("Attacking", false);
    } 
}
