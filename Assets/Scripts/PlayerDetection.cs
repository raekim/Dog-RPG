using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public delegate void PlayerDetectDelegate(GameObject player);
    public PlayerDetectDelegate playerDetectDelegate;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerDetectDelegate(other.gameObject);
        }
    }
}
