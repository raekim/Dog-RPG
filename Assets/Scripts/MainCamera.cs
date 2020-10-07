using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float cameraRotateSpeed;
    public Transform playerTransform;

    Vector3 diff;   // 카메라와 캐릭터 사이의 차이 벡터
    bool canRotate;

    // Start is called before the first frame update
    void Start()
    {
        canRotate = true;
        transform.LookAt(playerTransform);
    }

    // Update is called once per frame
    void Update()
    {
        canRotate = Input.GetKey(KeyCode.Mouse1);   // 마우스 오른쪽 클릭 감지

        if(canRotate)
        {                                      // 드래그 방향과 부호
            float mouseY = Input.GetAxis("Mouse Y");    // down < 0, up > 0
            float mouseX = Input.GetAxis("Mouse X");    // left < 0, right > 0

            Debug.Log("X: " + mouseX + "Y: " + mouseY);

            // 시점 거꾸로 뒤집히는 것 방지를 위해 Y축 회전 제한
            // 캐릭터 forward 벡터와 캐릭터-카메라 벡터가 이루는 각이 0-90도 사이어야 한다
            var YDot = Vector3.Dot(playerTransform.up, (transform.position - playerTransform.position).normalized);

            // Y축 회전
            Debug.Log("mouseY" + mouseY);
            Debug.Log("Dot" + YDot);
            bool rotationOutOfRange = (YDot < .1f && mouseY > 0) ||(YDot > 0.9f && mouseY < 0);
            if (!rotationOutOfRange)
            {
                transform.RotateAround(playerTransform.position, -transform.right, mouseY * cameraRotateSpeed * Time.deltaTime);
            }
            //if (Mathf.Abs(YDot) < 0.9f || Mathf.Sign(mouseY) == Mathf.Sign(YDot))
            //{
            //    transform.RotateAround(playerTransform.position, -transform.right, mouseY * cameraRotateSpeed * Time.deltaTime);
            //    //Debug.Log("YDot: " + YDot);
            //}
            
            // X축 회전
            transform.RotateAround(playerTransform.position, transform.up, mouseX * cameraRotateSpeed * Time.deltaTime);
            
            transform.LookAt(playerTransform);
        }
    }
}
