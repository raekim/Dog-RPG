using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float cameraMoveSpeed;
    public Transform playerTransform;

    Vector3 diff;   // 카메라와 캐릭터 사이의 차이 벡터

    // Start is called before the first frame update
    void Start()
    {
        diff = playerTransform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {

        }
    }
}
