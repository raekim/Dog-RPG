using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int maxMonsterCount;
    public float spawnRadius;

    [HideInInspector]
    public bool spawnerRunning;

    public int curMonsterCount;
    public float deltaTotal;
    public float waveDelaySeconds;    // waveDelaySeconds 초 마다 체크해서 몬스터가 maxMonsterCount 에서 모자라는 수 만큼 코루틴으로 스폰한다
    public float spawnDelaySeconds;   // 개별 몬스터 스폰 딜레이

    // Start is called before the first frame update
    void Start()
    {
        InitSpawner();
        StartCoroutine(SpawnerMain());
    }

    void InitSpawner()
    {
        deltaTotal = waveDelaySeconds;
        curMonsterCount = 0;
        spawnerRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTotal += Time.deltaTime;
    }

    IEnumerator SpawnerMain()
    {
        while (spawnerRunning)
        {
            if (deltaTotal >= waveDelaySeconds)
            {
                if (curMonsterCount < maxMonsterCount)
                {
                    int spawnCount = maxMonsterCount - curMonsterCount;
                    yield return StartCoroutine(SpawnMonster(spawnCount));
                }
                deltaTotal = 0f;
            }
            yield return null;
        }
    }

    IEnumerator SpawnMonster(int count)
    {
        // count 마리의 몬스터들을 spawnDelaySeconds 초 마다 스폰한다
        for (int i=0; i<count; ++i)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = 0f;
            GameObject obj = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

            // 몬스터 사망시 monsterCount 를 하나 감소시키도록 함수 등록
            obj.GetComponent<Character>().dieDelegate += DescreaseMonsterCount;

            curMonsterCount++;
            yield return new WaitForSeconds(spawnDelaySeconds);
        }
    }

    public void DescreaseMonsterCount()
    {
        curMonsterCount--;
    }
}
