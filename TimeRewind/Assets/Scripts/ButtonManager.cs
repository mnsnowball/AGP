using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject enabledPos;
    public GameObject disabledPos;
    public void LoadTheScene(string toLoad){
        Debug.Log("ButtonManager Loading");
        SceneManager.LoadScene(toLoad);
    }

    public void QuitGame(){
        Application.Quit();
        Debug.Log("Quit");
    }

    public void EnableObject(GameObject toEnable){
        toEnable.transform.position = enabledPos.transform.position;
    }

    public void DisableObject(GameObject toDisable){
        toDisable.transform.position = disabledPos.transform.position;
    }  
}
