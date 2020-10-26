using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaceCamera : MonoBehaviour
{
    public Transform playerModelTransform;

    float dist;

    private void Start()
    {
        dist = Vector3.Distance(playerModelTransform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // 캐릭터의 얼굴을 비춘다
        //transform.position = playerModelTransform.position + playerModelTransform.forward * dist;
    }
}
