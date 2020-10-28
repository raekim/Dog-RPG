using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    bool freeze;
    bool mouseOver;
    public GameObject tankyouTalkBoard;
    public GameObject itemDescription;
    public GameObject buyUI;
    public PotionManager potionManager;

    public delegate void ShopCloseDelegate();
    public ShopCloseDelegate shopCloseDelegate;

    public void UnFreezeShop()
    {
        freeze = false;
    }

    public void BuyItem(int amount)
    {
        potionManager.AddToPotionNum(amount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (freeze) return;

        // 클릭 한 지점에 구매 수량 결정 대화창을 연다
        freeze = true;
        buyUI.SetActive(true);
        buyUI.transform.position = Input.mousePosition;
        itemDescription.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (freeze) return;

        itemDescription.SetActive(true);
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDescription.SetActive(false);
        mouseOver = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        mouseOver = freeze = false;
        ControlManager.Instance.isInteractingWithUI = true;

        tankyouTalkBoard.GetComponent<TalkBoard>().talkFinishDelegate += ClosePotionShop;
    }

    void HidePotionShopUI()
    {
        // 포션 구매 UI가 보이지 않고 감사 인사 대화가 뜬다
        gameObject.SetActive(false);
        tankyouTalkBoard.SetActive(true);
    }

    void ClosePotionShop()
    {
        // 포션 샵 전체를 완전히 닫는다
        tankyouTalkBoard.SetActive(false);
        shopCloseDelegate();
    }

    // Update is called once per frame
    void Update()
    {
        if(!freeze)
        {
            if(mouseOver)
            {
                // 아이템 설명이 커서를 따라다닌다
                if (!itemDescription.activeSelf) itemDescription.SetActive(true);
                itemDescription.transform.position = Input.mousePosition;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    HidePotionShopUI();
                }
            }
        }
    }
}
