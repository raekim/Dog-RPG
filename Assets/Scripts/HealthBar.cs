using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    float transparencyConstant = 50f;
    [Range(0f, .7f)] public float transparency;
    Image[] myImages;
    Text[] myTexts;
    Slider mySlider;
    Transform healthBarPositionTransform;

    public Image fillImage;

    private void Awake()
    {
        myImages = GetComponentsInChildren<Image>();
        myTexts = GetComponentsInChildren<Text>();
        mySlider = GetComponentInChildren<Slider>();
    }

    public void SetHealthBarPositionTransform(Transform trans)
    {
        healthBarPositionTransform = trans;

        var pos = Camera.main.WorldToScreenPoint(healthBarPositionTransform.position);
        transform.position = pos;
    }

    public void HealthDisplay(float ratio)
    {
        mySlider.value = ratio;

        // hp 바의 피통 부분을 없앤다
        if(mySlider.value <= float.Epsilon)
        {
            fillImage.enabled = false;
        }
    }

    public void Init(string name, int maxHP)
    {
        foreach (Text txt in myTexts)
        {
            txt.text = name;
        }

        var sz = mySlider.gameObject.GetComponent<RectTransform>().sizeDelta;
        sz.x = maxHP * 2.5f;
        mySlider.gameObject.GetComponent<RectTransform>().sizeDelta = sz;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayOnScreen();
        ChangeTransparency(transparency);
    }

    void DisplayOnScreen()
    {
        var pos = Camera.main.WorldToScreenPoint(healthBarPositionTransform.position);
        transform.position = pos;

        float dist = Vector3.Distance(Camera.main.transform.position, healthBarPositionTransform.position);
        if (dist <= 5f) dist = 0f;
        ChangeTransparency((transparencyConstant - dist) / transparencyConstant);
    }

    void ChangeTransparency(float amount)
    {
        transparency = amount;
        foreach(Image im in myImages)
        {
            im.color = new Color(im.color.r, im.color.g, im.color.b, transparency);
        }
        foreach (Text txt in myTexts)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, transparency);
        }
    }
}
