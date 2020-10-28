using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TalkBoard : MonoBehaviour, IPointerClickHandler
{
    public delegate void TalkFinishDelegate();
    public TalkFinishDelegate talkFinishDelegate;

    [TextArea] public string talkContent;  // 대화 내용
    public Text displayText;

    bool displaying;
    string[] talkStrings;   // 개행문자를 기준으로 나눈 대화 문자열들
    int index;
    string currentText;

    private void OnEnable()
    {
        index = 0;
        displayText.text = "";
        displaying = false;
        currentText = "";

        // 대화 표시 시작
        DisplayNext();
    }

    // Awake는 스크립트가 enable 되지 않아도 동작한다
    private void Awake()
    {
        // 대화 내용을 개행문자를 기준으로 끊어서 저장한다
        talkStrings = talkContent.Split('\n');
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void DisplayNext()
    {
        if(displaying)
        {
            StopAllCoroutines();
            // 대화 전체 보이기
            displayText.text = currentText;
            displaying = false;
        }
        else
        {
            if (index >= talkStrings.Length)
            {
                // 대화 종료
                talkFinishDelegate();
                return;
            }

            // 다음 대화로 넘어가기
            if (index < talkStrings.Length)
            {
                currentText = talkStrings[index];
            }
            if (index + 1 < talkStrings.Length)
            {
                currentText += "\n" + talkStrings[++index];
            }
            index++;

            StartCoroutine(TextDislayEffect());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DisplayNext();
    }

    IEnumerator TextDislayEffect()
    {
        displaying = true;
        // currentText에 있는 내용을 보여준다
        displayText.text = "";

        foreach(char c in currentText)
        {
            displayText.text += c;
            yield return new WaitForSeconds(.05f);
        }

        displaying = false;
    }
}
