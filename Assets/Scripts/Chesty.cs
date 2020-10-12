using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chesty : Character
{
    Transform playerTransform;
    public float moveSpeed;
    public float attackRange;
    public float followRange;

    SphereCollider awakeCollider;

    enum State
    {
        Sleep,
        Battle,
        Walking
    }

    State currentState;
    bool stateChanged;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        awakeCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        currentState = State.Sleep;
        StartCoroutine(FSM());
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
        awakeCollider.enabled = true;
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
        float walkingTime = Random.Range(1.5f, 3f);

        while (delta < walkingTime)
        {
            Debug.Log(delta);
            yield return null;

            delta += Time.deltaTime;
            transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        }

        animator.SetTrigger("Sleep");
        ChangeState(State.Sleep);
    }

    IEnumerator Battle()
    {
        Debug.Log("Battle");
        animator.SetBool("Battle", true);
        stateChanged = false;
        yield return new WaitForSeconds(1f);

        while (!stateChanged)
        {
            yield return null;

            float dist = Vector3.Distance(transform.position, playerTransform.position);

            if(dist < attackRange) // 플레이어가 공격 범위 내에 있으면 플레이어를 공격
            {
                yield return StartCoroutine(Attack());
            }
            else if(dist < followRange) // 플레이어가 가까이 있으면 플레이어를 따라 감
            {
                yield return StartCoroutine(FollowPlayer());
            }
            else
            {
                // 잠시 대기하다가 전투모드 종료
                yield return new WaitForSeconds(3f);
                dist = Vector3.Distance(transform.position, playerTransform.position);
                if (dist > followRange)
                {
                    ChangeState(State.Walking);
                }
            }
        }

        animator.SetBool("Battle", false);
    }

    IEnumerator FollowPlayer()
    {
        Debug.Log("following");
        animator.SetBool("Follow Player", true);
        bool following = true;

        while(following)
        {
            yield return null;

            // 플레이어를 따라다님
            Vector3 dir = (playerTransform.position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);

            float dist = Vector3.Distance(transform.position, playerTransform.position);
            following = dist < followRange && attackRange < dist;
        }

        animator.SetBool("Follow Player", false);

        Debug.Log("following over");
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

    // 버그 해결: OnTriggerEnter가 두 번 call되는 현상
    // -> Sphere Collider의 contact point가 여러 개라서 일어날 수 있는 일
    private void OnTriggerEnter(Collider other)
    {
        // Sleep 도중에 플레이어가 가까이 접근하면 깨어난다
        if (other.gameObject.tag == "Player")
        {
            playerTransform = other.gameObject.transform;
            ChangeState(State.Battle);
            animator.SetTrigger("Awake");

            awakeCollider.enabled = false;
        }
    }
}
