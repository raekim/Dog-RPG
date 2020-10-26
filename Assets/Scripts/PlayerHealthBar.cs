using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    public TMP_Text healthText;
    public Image fillImage;

    Slider mySlider;

    private void Awake()
    {
        mySlider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void HealthDisplay(float ratio, int currHP, int maxHP)
    {
        mySlider.value = ratio;

        // hp 바의 피통 부분을 없앤다
        if (mySlider.value <= float.Epsilon)
        {
            fillImage.enabled = false;
        }

        // 문자열로 hp 표시
        healthText.text = currHP.ToString() + "/" + maxHP.ToString();
    }
}
