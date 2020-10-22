using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string str = "안녕하세요";
        Debug.Log(str.Length);

        foreach(var c in str)
        {
            Debug.Log(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
