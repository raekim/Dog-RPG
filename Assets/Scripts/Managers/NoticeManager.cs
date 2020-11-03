using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeManager : MonoBehaviour
{
    private static NoticeManager instance = null;
    public static NoticeManager Instance => instance;

    public Text noticeText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetNoticeActive(bool val)
    {
        noticeText.gameObject.SetActive(val);
    }

    public void WriteToNotice(string str)
    {
        noticeText.text = str;
    }
}
