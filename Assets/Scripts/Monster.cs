using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    public GameObject HPBarPrefab;  // 몬스터 hp 바 프리팹
    protected HealthBar HPBar;

    protected void Awake()
    {
        Transform healthBarPosition = transform.Find("Health Bar Position").transform;

        if (healthBarPosition != null)
        {
            HPBar = Instantiate(HPBarPrefab, GameObject.Find("Canvas").transform).GetComponent<HealthBar>();
            HPBar.SetHealthBarPositionTransform(healthBarPosition);
        }
    }

    override protected void HPChanged()
    {
        if (HPBar != null)
        {
            HPBar.HealthDisplay((float)currentHP / maxHP);
        }
    }

    override public void TakeDamage(int amount)
    {
        if (getHitDelegate != null)
        {
            getHitDelegate(amount);
            HPChanged();
        }

        // 몬스터 사망
        if (!isInvincible && currentHP == 0)
        {
            if (dieDelegate != null) dieDelegate();
        }
    }
}
