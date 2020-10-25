using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordDisplay : MonoBehaviour
{
    public TMP_Text TMP;
    public float speed;
    public string words;
    public bool loop;

    private void OnEnable()
    {
        TMP.text = "";
        StartCoroutine(display());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator display()
    {
        do
        {
            TMP.text = "";
            foreach (char c in words)
            {
                TMP.text += c;
                yield return new WaitForSeconds(speed);

                
            }
            yield return null;
        }
        while (loop);
    }
}
