using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraControl : MonoBehaviour
{

    public CinemachineVirtualCamera pausedCam; // the paused camera
    public CinemachineVirtualCamera defaultCam; // the gameplay cam

    GameManager theManager;

    private void Start() {
        theManager  = GameObject.FindObjectOfType<GameManager>();
        
        pausedCam.gameObject.SetActive(false);
        defaultCam.gameObject.SetActive(true);
        
    }
    private void Update() {
        if (theManager.isPlaying)
        {
            if (!pausedCam.gameObject.activeSelf)
            {
                pausedCam.gameObject.SetActive(true);
                defaultCam.gameObject.SetActive(false);
            }  
        } else {
            if (!theManager.isPaused && pausedCam.gameObject.activeSelf) // if it isn't paused  and the paused camera is active, unpause
            {
                defaultCam.gameObject.SetActive(true);
                pausedCam.gameObject.SetActive(false);
                //Debug.Log("Changing to unpause");
            }
            if (theManager.isPaused && !pausedCam.gameObject.activeSelf) // if it's paused and the pause cam isn't active
            {
                pausedCam.gameObject.SetActive(true);
                defaultCam.gameObject.SetActive(false);
                //Debug.Log("Changing to pause");
            }
        }
        
        
    }
}
