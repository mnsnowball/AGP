using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public GameObject playingCam;
    public GameObject defaultCam;
    private void Update() {
        GameManager theManager  = GameObject.FindObjectOfType<GameManager>();
        if (!theManager.isPaused) // if it isn't paused we can assume it's playing
        {
            defaultCam.SetActive(false);
            playingCam.SetActive(true);
        } else //it's paused so set default cam to active
        {
            defaultCam.SetActive(true);
            playingCam.SetActive(false);
        }
    }
}
