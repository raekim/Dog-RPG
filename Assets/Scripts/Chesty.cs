using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chesty : Character
{
    Transform playerTransform;
    public float moveSpeed;
    public float attackRange;
    public float followRange;

    enum State
    {
        Sleep,
        Battle,
        Walking,
    }

    State currentState;
    bool stateChanged;

    new private void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        getHitDelegate += GetHit;
        dieDelegate += Die;
        GetComponentInChildren<PlayerDetection>().playerDetectDelegate += OnPlayerDetect;
    }

    private void Start()
    {
        HPBar.Init("체스티", 30);
        HPBar.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        currentState = State.Sleep;
        StartCoroutine(FSM());

        // HP 초기화
        SetMaxHP(30);
        FillUpHPToMax();
        isAlive = true;
    }

    public void GetHit(int amount)
    {
        if (!isAlive) return;

        // Sleep 중에는 타격을 입지 않는다
        if (currentState == State.Sleep) return;

        Debug.Log("Chesty: Ouch!!");
        AddToHP(amount);
        animator.SetTrigger("Hit");

        DamageSkinManager.Instance.DisplayDamage(-amount, transform.position + Vector3.up * 2f);
    }

    void Die()
    {
        if (!isAlive) return;   // 두 번 죽지 않는다

        isAlive = false;
        StopAllCoroutines();
        StartCoroutine(DieCoroutine());
        GetComponentInChildren<BoxCollider>().enabled = false;
    }

    IEnumerator DieCoroutine()
    {
        Debug.Log("Chesty Dies");
        animator.SetTrigger("Die");

        // Drop Gold
        GoldManager.Instance.SpawnGold(3, transform.position);

        yield return new WaitForSeconds(5f);

        //Destroy(gameObject);
        gameObject.SetActive(false);
        HPBar.gameObject.SetActive(false);
    }

    void ChangeState(State nextState)
    {
        stateChanged = true;
        currentState = nextState;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, followRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    IEnumerator Sleep()
    {
        stateChanged = false;
        while (!stateChanged)
        {
            yield return null;
        }
    }

    IEnumerator Walking()
    {
        // 가까운 랜덤한 방향으로 걸어간다
        Vector3 dir = Random.insideUnitSphere;
        dir.y = 0f;

        float delta = 0f;
        float walkingTime = Random.Range(2f, 4f);
        stateChanged = false;

        while (!stateChanged && delta < walkingTime)
        {
            yield return null;

            delta += Time.deltaTime;
            transform.Translate(dir * moveSpeed * .5f * Time.deltaTime, Space.World);
        }

        if(delta >= walkingTime)
        {
            animator.SetTrigger("Sleep");
            ChangeState(State.Sleep);
        }
    }

    IEnumerator Battle()
    {
        animator.SetBool("Battle", true);
        stateChanged = false;

        float idlingDelta = 0f;

        while (!stateChanged)
        {
            yield return null;

            float dist = Vector3.Distance(transform.position, playerTransform.position);

            if(dist < attackRange) // 플레이어가 공격 범위 내에 있으면 플레이어를 공격
            {
                idlingDelta = 0f;
                yield return StartCoroutine(Attack());
            }
            else if(dist < followRange) // 플레이어가 가까이 있으면 플레이어를 따라 감
            {
                idlingDelta = 0f;
                yield return StartCoroutine(FollowPlayer());
            }
            else
            {
                // 잠시 대기하다가 전투모드 종료
                idlingDelta += Time.deltaTime;

                if (idlingDelta > 3f)
                {
                    dist = Vector3.Distance(transform.position, playerTransform.position);
                    if (dist > followRange)
                    {
                        ChangeState(State.Walking);
                    }
                }
            }
        }

        animator.SetBool("Battle", false);
    }

    IEnumerator FollowPlayer()
    {
        animator.SetBool("Follow Player", true);
        bool following = true;

        while (following)
        {
            yield return null;

            // 플레이어를 따라다님
            transform.LookAt(playerTransform.position);
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
            
            float dist = Vector3.Distance(transform.position, playerTransform.position);
            following = dist < followRange && attackRange < dist;
        }

        animator.SetBool("Follow Player", false);
    }

    IEnumerator Attack()
    {
        animator.SetBool("Attacking", true);
        // 플레이어 공격
        bool attacking = true;

        while (attacking)
        {
            yield return null;

            attacking = Vector3.Distance(transform.position, playerTransform.position) < attackRange;
        }

        animator.SetBool("Attacking", false);
    }

    void OnPlayerDetect(GameObject playerObject)
    {
        playerTransform = playerObject.transform;

        // Sleep 도중에 플레이어가 가까이 접근하면 깨어난다
        if(currentState == State.Sleep)
        {
            ChangeState(State.Battle);
            animator.SetTrigger("Awake");
        }
        // 걸어가던 도중에 플레이어가 가까이 접근하면 공격모드로 돌입한다
        else if (currentState == State.Walking)
        {
            ChangeState(State.Battle);
        }
    }
}
