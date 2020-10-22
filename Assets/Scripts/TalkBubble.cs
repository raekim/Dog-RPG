using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TalkBubble : MonoBehaviour
{
    public Transform bubbleTransform;
    float offsetY;

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
