using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.canPlay = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.canPlay = false;
        }
    }
}
