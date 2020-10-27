using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PotionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image potionImage;

    public delegate bool PotionUseDelegate();
    public PotionUseDelegate potionUseDelegate;

    Slider potionCoolTimeSlider;
    TMP_Text potionText;
    bool overPotionUI;
    bool canUsePotion;

    IEnumerator CoolTime(float time)
    {
        canUsePotion = false;
        float cnt = 0f;

        while(cnt <= time)
        {
            yield return null;
            cnt += Time.deltaTime;

            potionCoolTimeSlider.value = (time - cnt) / time;
        }

        canUsePotion = true;
    }

    private void Awake()
    {
        potionText = GetComponentInChildren<TMP_Text>();
        potionCoolTimeSlider = GetComponentInChildren<Slider>();
    }
    public void DisplayPotion(int number)
    {
        potionText.text = number.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canUsePotion) return;

        bool potionUsed = potionUseDelegate();
        if(potionUsed)
        {
            StartCoroutine(CoolTime(3f));
            //Debug.Log("Potion used");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Down");
        potionImage.transform.localScale = Vector3.one * 1.2f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Enter");
        potionImage.transform.localScale = Vector3.one * 1.5f;
        ControlManager.Instance.isMouseOverUI = true;
        overPotionUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Exit");
        potionImage.transform.localScale = Vector3.one;
        ControlManager.Instance.isMouseOverUI = false;
        overPotionUI = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Up");
        if (overPotionUI)
        {
            potionImage.transform.localScale = Vector3.one * 1.5f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        overPotionUI = false;
        canUsePotion = true;
    }
}
