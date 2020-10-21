using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class CoinPickManager : MonoBehaviour
{
    public Sprite coinSprite;
    public Transform statusCanvasTransform; // 코인이 그려질 캔버스

    Queue<Image> coinImageQ;
    private static CoinPickManager instance = null;

    public static CoinPickManager Instance => instance;

    public Image goalUI;   // 코인이 흡수될 UI
    Vector3 goalUIScreenPosition;

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

    // Start is called before the first frame update
    void Start()
    {
    }


    public void CoinPick(Vector3 coinWorldPosition)
    {
        //var obj = new GameObject("coinUI");
        //obj.transform.parent = statusCanvas.transform;
        //obj.AddComponent<Image>().sprite = coinSprite;
        //obj.transform.localScale = new Vector3(.4f, .4f, 1f);
        //obj.transform.position = Camera.main.WorldToScreenPoint(coinWorldPosition);
        //
        //StartCoroutine(DisplayCoinUI(obj.GetComponent<Image>()));
    }

    IEnumerator DisplayCoinUI(Image coinImage)
    {
        var rectTrans = coinImage.GetComponent<RectTransform>();
        float dist = Vector2.Distance(rectTrans.anchoredPosition, goalUIScreenPosition);

        while (Vector2.Distance(rectTrans.anchoredPosition, goalUIScreenPosition) > 1f)
        {
            yield return null;
            //rectTrans.anchoredPosition += (goalUIScreenPosition - rectTrans.anchoredPosition).normalized * dist * Time.deltaTime;

        }

        Destroy(coinImage.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Camera.main.WorldToScreenPoint(goalUI.transform.position));
    }
}
