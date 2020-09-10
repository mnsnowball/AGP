using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    bool canPlay = false;
    bool isPlaying = false;
    private void Update() {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            canPlay = true;
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.Space) && canPlay)
        {
            GameManager theManager  = GameObject.FindObjectOfType<GameManager>();
            canPlay = false;
            Debug.Log("Play");
            if (!isPlaying)
            {
                theManager.SetPlay();
                isPlaying = true;
            } else {
                theManager.SetPause(true);
                isPlaying = false;
            }
        }
    }
}
