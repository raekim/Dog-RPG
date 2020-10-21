using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    private static GoldManager instance = null;

    public static GoldManager Instance => instance;

    int goldAmount;
    public TMP_Text goldText;
    public GameObject GoldPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        goldAmount = 0;
        goldText.text = goldAmount.ToString();
    }

    public void AddGold(int amount)
    {
        if(goldAmount + amount < 0)
        {
            return;
        }
        goldAmount += amount;
        goldText.text = goldAmount.ToString();
    }

    public void SpawnGold(int amount, Vector3 spawnPosition)
    {
        for(int i=0; i<amount; ++i)
        {
            var coin = Instantiate(GoldPrefab, transform);
            StartCoroutine(CoinSpawnEffect(spawnPosition, coin.transform));
        }
    }

    // 코인이 던져지는 효과
    IEnumerator CoinSpawnEffect(Vector3 spawnPosition, Transform coinTrans)
    {
        coinTrans.position = spawnPosition + transform.up*1.5f;
        Vector3 vel = Random.insideUnitSphere*2f;
        vel.y = Random.Range(5f, 8f);

        while(coinTrans.position.y > 0f)
        {
            yield return null;

            coinTrans.Translate(vel * Time.deltaTime);
            vel.y -= 12f * Time.deltaTime;
        }

        var pos = coinTrans.position;
        pos.y = 0f;
        coinTrans.position = pos;
    }
}
