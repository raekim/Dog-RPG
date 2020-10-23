using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Talk : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject talkBubble;
    public GameObject talkBoard;
    public Transform bubbleTransform;
    
    Image bubbleImage;
    TMP_Text bubbleText;
    Image boardImage;
    Text boardText;
    float offsetY = 3f;

    bool talkingOn;
    bool clickable;

    float transparencyConstant = 20f;   // 투명도가 0이 되는 거리
    float clickableDistance = 15f;
    float transparency;

    private void Awake()
    {
        bubbleImage = talkBubble.GetComponent<Image>();
        boardImage = talkBoard.GetComponent<Image>();
        bubbleText = talkBubble.GetComponentInChildren<TMP_Text>();
        boardText = talkBoard.GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        talkingOn = false;
        clickable = false;
    }

    // Update is called once per frame
    void Update()
    {
        clickable = false;

        // 위치 조절
        var pos = Camera.main.WorldToScreenPoint(bubbleTransform.position + Vector3.up * offsetY);
        transform.position = pos;

        // 투명도 조절
        float dist = Vector3.Distance(Camera.main.transform.position, bubbleTransform.position);
        //Debug.Log(dist);
        if (dist <= clickableDistance)
        {
            dist = 0f;
            clickable = true;
        }
        ChangeTransparency((transparencyConstant - dist) / transparencyConstant);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!talkingOn && clickable)
        {
            bubbleImage.color = new Color32(0xE3, 0x30, 0x30, 0xFF);
            transform.localScale = Vector3.one * 1.4f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!talkingOn)
        {
            bubbleImage.color = Color.white;
            transform.localScale = Vector3.one;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!talkingOn && clickable)
        {
            talkBubble.SetActive(false);
            talkBoard.SetActive(true);
            talkingOn = true;
        }
    }

    public void TalkFinished()
    {
        talkBubble.SetActive(true);
        talkBoard.SetActive(false);
        talkingOn = false;
    }

    void ChangeTransparency(float amount)
    {
        transparency = amount;

        bubbleImage.color = new Color(bubbleImage.color.r, bubbleImage.color.g, bubbleImage.color.b, transparency);
        boardImage.color = new Color(boardImage.color.r, boardImage.color.g, boardImage.color.b, transparency);
        bubbleText.color = new Color(bubbleText.color.r, bubbleText.color.g, bubbleText.color.b, transparency);
        boardText.color = new Color(boardText.color.r, boardText.color.g, boardText.color.b, transparency);
    }
}
