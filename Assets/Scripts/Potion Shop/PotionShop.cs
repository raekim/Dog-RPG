using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    bool freeze;
    bool mouseOver;
    public GameObject ItemDescription;
    public GameObject BuyUI;
    public PotionManager potionManager;

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
        BuyUI.SetActive(true);
        BuyUI.transform.position = Input.mousePosition;
        ItemDescription.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (freeze) return;

        ItemDescription.SetActive(true);
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemDescription.SetActive(false);
        mouseOver = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        mouseOver = freeze = false;
        ControlManager.Instance.isInteractingWithUI = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!freeze && mouseOver)
        {
            // 아이템 설명이 커서를 따라다닌다
            if (!ItemDescription.activeSelf) ItemDescription.SetActive(true);
            ItemDescription.transform.position = Input.mousePosition;
        }
    }
}
