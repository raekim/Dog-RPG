using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Talk : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject talkBubble;
    public GameObject talkBoard;
    public Transform bubbleTransform;

    Image bubbleImage;
    float offsetY;

    bool talkingOn;

    private void Awake()
    {
        bubbleImage = talkBubble.GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        offsetY = 3f;
        talkingOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 위치 조절
        var pos = Camera.main.WorldToScreenPoint(bubbleTransform.position + Vector3.up * offsetY);
        transform.position = pos;

        // 투명도 조절
        //float dist = Vector3.Distance(Camera.main.transform.position, healthBarPositionTransform.position);
        //if (dist <= 5f) dist = 0f;
        //ChangeTransparency((transparencyConstant - dist) / transparencyConstant);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!talkingOn)
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
        if(!talkingOn)
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

    //void ChangeTransparency(float amount)
    //{
    //    transparency = amount;
    //    foreach (Image im in myImages)
    //    {
    //        im.color = new Color(im.color.r, im.color.g, im.color.b, transparency);
    //    }
    //    foreach (Text txt in myTexts)
    //    {
    //        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, transparency);
    //    }
    //}


}
