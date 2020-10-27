using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text GoldNeededText; // 골드 부족
    public Text AmountText; // 구매 수량
    public Text TotalText;  // 골드 합계
    public PotionShop myPotionShop;
    public int itemPrice;

    bool mouseOver;
    int buyAmount;  // 구매 갯수

    private void OnEnable()
    {
        buyAmount = 1;
        DisplayBuyInfo();
    }

    public void BuyRequest()
    {
        int goldNeeded = buyAmount * itemPrice; // 필요 골드량

        if (GoldManager.Instance.GetGoldAmount() < goldNeeded)
        {
            // 보유 골드량이 부족
            StopAllCoroutines();
            StartCoroutine(DisplayGoldNeededText());
        }
        else
        {
            // 골드 차감하고 구매 요청
            GoldManager.Instance.AddGold(-goldNeeded);
            myPotionShop.BuyItem(buyAmount);

            // 구매 창을 닫는다
            CloseBuyUI();
        }
    }

    IEnumerator DisplayGoldNeededText()
    {
        GoldNeededText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        GoldNeededText.gameObject.SetActive(false);
    }

    public void IncreaseBuyAmount()
    {
        if (buyAmount < 100) buyAmount++;
        DisplayBuyInfo();
    }

    public void DecreaseBuyAmount()
    {
        if (buyAmount > 1) buyAmount--;
        DisplayBuyInfo();
    }

    void DisplayBuyInfo()
    {
        AmountText.text = "구매수량 : " + buyAmount.ToString() + " 개";
        TotalText.text = "합계 : " + (buyAmount * itemPrice).ToString() + "골드";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    void CloseBuyUI()
    {
        GoldNeededText.gameObject.SetActive(false);
        myPotionShop.UnFreezeShop();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(!mouseOver)
            {
                CloseBuyUI();
            }
        }
    }
}
