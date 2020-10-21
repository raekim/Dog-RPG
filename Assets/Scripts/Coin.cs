using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0f, Random.Range(0f, 360f), 0f, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        // 코인 천천히 회전
        transform.Rotate(0f, 30f * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerStay(Collider other)
    {
        // 코인을 먹는다
        if(Vector3.Distance(other.gameObject.transform.position, transform.position) < 1f)
        {
            Destroy(gameObject);
            CoinPickManager.Instance.CoinPick(transform.position);
        }
    }
}
