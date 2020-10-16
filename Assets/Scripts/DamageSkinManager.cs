using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DamageSkinManager : MonoBehaviour
{
    public Text textStyle;
    public int damageTextCount; // 큐에 존재할 테미지 텍스트 갯수
    Queue<DamageText> damageTextQ;
    private static DamageSkinManager instance = null;

    public static DamageSkinManager Instance => instance;

    class DamageText
    {
        public Text text;
        public Coroutine displayCoroutine;

        public DamageText(Text text) { this.text = text; displayCoroutine = null; }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        damageTextQ = new Queue<DamageText>();

        // 모든 데미지 텍스트 오브젝트들의 부모
        GameObject textParent = new GameObject("Damage Texts");
        textParent.transform.parent = GameObject.Find("Canvas").transform;
        textParent.transform.position = Vector3.zero;

        // 데미지 텍스트들 초기화
        for (int i=0; i< damageTextCount; ++i)
        {
            var textObject = new GameObject("DamageText" + i.ToString());

            textObject.transform.parent = textParent.transform;
            var damageText = new DamageText(textObject.AddComponent<Text>());

            // 텍스트 스타일 설정
            var text = damageText.text;
            text.font = textStyle.font;
            text.fontSize = textStyle.fontSize;
            text.fontStyle = textStyle.fontStyle;
            text.alignment = textStyle.alignment;
            text.color = textStyle.color;

            // 새로 만든 데미지 텍스트 큐에 넣기
            textObject.SetActive(false);
            damageTextQ.Enqueue(damageText);
        }
    }

    public void DisplayDamage(int amount, Vector3 worldPosition)
    {
        // 큐의 맨 앞에 있는 텍스트를 사용하여 데미지 UI를 표시한다
        var damageText = damageTextQ.Dequeue();
        damageText.text.text = amount.ToString();

        if (damageText.displayCoroutine != null) StopCoroutine(damageText.displayCoroutine);

        damageText.displayCoroutine = StartCoroutine(DamageTextEffectCoroutine(damageText.text, worldPosition));

        // 큐에서 빼낸 것 도로 넣기
        damageTextQ.Enqueue(damageText);
    }

    void SetTextAlpha(Text text, float a)
    {
        Color c = text.color;
        c.a = a;
        if (c.a < 0f) c.a = 0f;
        text.color = c;
    }

    IEnumerator DamageTextEffectCoroutine(Text text, Vector3 worldPosition)
    {
        text.gameObject.SetActive(true);

        // 데미지 텍스트가 점점 위로 떠오르면서 사라지는 효과
        float cnt = 0f;
        float t = 1f;

        Vector3 posDiff = UnityEngine.Random.insideUnitCircle;
        posDiff.z = 0f;
        posDiff *= .3f;
        worldPosition += posDiff;   // 위치에 변화를 주기 위함

        // 초기 위치로 움직이고 투명도를 1로 초기화
        text.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
        SetTextAlpha(text, 1f);

        // 잠시 멈춰있기
        while(cnt < .5f)
        {
            yield return null;
            text.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
            cnt += Time.deltaTime;
        }

        cnt = 0f;

        while (cnt < t)
        {
            yield return null;

            // 텍스트가 위로 떠오르기
            worldPosition.y += Time.deltaTime;

            // 텍스트 투명도 조절
            SetTextAlpha(text, text.color.a - Time.deltaTime / t);

            text.transform.position = Camera.main.WorldToScreenPoint(worldPosition);

            cnt += Time.deltaTime;
        }

        text.gameObject.SetActive(false);
    }
}
