using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSwordAttack : Attack
{
    public int damage;

    readonly float colliderApearTime = .35f;
    bool colliderApeared;
    bool leftMouseClicked;
    float delta;

    public SphereCollider attackCollider;

    private void Awake()
    {
        attackLength = .7f;
    }

    public override void AttackStart()
    {
        attacking = false;
        colliderApeared = false;
        delta = 0f;
        attackCollider.enabled = false;
    }

    public override void UpdateAttack(bool leftMouseClicked)
    {
        this.leftMouseClicked = leftMouseClicked;
    }

    private void Update()
    {
        delta += Time.deltaTime;

        // 시간 내에 마우스 클릭하면 공격 유지
        if (delta > .3f && leftMouseClicked)
        {
            Debug.Log("clicked");
            attacking = true;
        }
        
        // 공격 모션 막바지에 콜라이더 생성 (공격마다 한 번만 생성)
        if(!colliderApeared && delta >= colliderApearTime)
        {
            attackCollider.enabled = true;
            colliderApeared = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.gameObject.GetComponent<Character>();
        if(character != null)
        {
            character.TakeDamage(-damage);
            attackCollider.enabled = false;
        }
    }
}
