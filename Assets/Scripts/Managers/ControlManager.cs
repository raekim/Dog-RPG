using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    private static ControlManager instance = null;
    public static ControlManager Instance => instance;
    // Start is called before the first frame update

    public bool isMouseOverUI; // 마우스 포인터가 UI 위에 있는가?
    public bool isInteractingWithUI;   // UI와 상호작용 중인가? (NPC와 대화, 아이템 구매 등)

    // 플레이어 관련 조작
    bool playerAttackButtonDown;
    bool playerMoveKeyDown;
    Vector3 moveDir;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            moveDir = Vector3.zero;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool GetPlayerAttackButtonDown()
    {
        return !isMouseOverUI && !isInteractingWithUI && playerAttackButtonDown;
    }

    public bool GetPlayerMoveKeyDown(out Vector3 dir)
    {
        dir = moveDir;
        return !isInteractingWithUI && playerMoveKeyDown;
    }

    void Control()
    {
        playerAttackButtonDown = Input.GetKeyDown(KeyCode.Mouse0);  // 마우스 왼쪽 클릭

        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDir.Normalize();    // 방향만 필요하기 때문에 벡터 정규화
        playerMoveKeyDown = moveDir.magnitude > 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Control();
    }
}
