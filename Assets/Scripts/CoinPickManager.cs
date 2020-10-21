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
    Vector2 goalUIAnchoredPosition;

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
        goalUIAnchoredPosition = goalUI.GetComponent<RectTransform>().anchoredPosition +
            goalUI.transform.parent.transform.GetComponent<RectTransform>().anchoredPosition;
    }

    public void CoinPick(Vector3 coinWorldPosition)
    {
        // 새로운 2D 이미지가 생성된다
        var obj = new GameObject("coinUI");
        obj.AddComponent<RectTransform>();
        obj.GetComponent<RectTransform>().SetParent(statusCanvasTransform.transform);
        obj.AddComponent<Image>().sprite = coinSprite;
        obj.transform.localScale = new Vector3(.4f, .4f, 1f);

        // 코인을 먹은 위치에 생성
        var coinPos = Camera.main.WorldToViewportPoint(coinWorldPosition);
        coinPos.x -= .5f;
        coinPos.x *= statusCanvasTransform.GetComponent<RectTransform>().sizeDelta.x;
        coinPos.y -= .5f;
        coinPos.y *= statusCanvasTransform.GetComponent<RectTransform>().sizeDelta.y;
        coinPos.z = 0f;

        obj.GetComponent<RectTransform>().anchoredPosition = coinPos;

        StartCoroutine(DisplayCoinUI(obj.GetComponent<Image>()));
    }

    IEnumerator DisplayCoinUI(Image coinImage)
    {
        var rectTrans = coinImage.GetComponent<RectTransform>();

        Debug.Log(Vector2.Distance(rectTrans.anchoredPosition, goalUIAnchoredPosition));

        Vector2 vel = (goalUIAnchoredPosition - rectTrans.anchoredPosition).normalized * 10f;
        vel += Random.insideUnitCircle * 200f;

        while (Vector2.Distance(rectTrans.anchoredPosition, goalUIAnchoredPosition) > 40f)
        {
            yield return null;
            rectTrans.anchoredPosition += vel * Time.deltaTime;

            Vector2 vel2 = (goalUIAnchoredPosition - rectTrans.anchoredPosition).normalized * 500f;
        
            vel = Vector2.Lerp(vel, vel2, Time.deltaTime);
        }

        Destroy(coinImage.gameObject);
        GoldManager.Instance.AddGold(1);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
