using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalkBubble : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image bubbleImage;

    public Transform bubbleTransform;
    float offsetY;

    private void Awake()
    {
        bubbleImage = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        offsetY = 3f;
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
        bubbleImage.color = new Color(.7f, .3f, .3f);
        transform.localScale = Vector3.one * 1.4f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bubbleImage.color = Color.white;
        transform.localScale = Vector3.one;
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
