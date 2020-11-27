using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveInterest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Client")
        {
            GameManager.instance.LevelComplete();
        }
    }
}
