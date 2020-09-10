using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBlock : MonoBehaviour
{
    GameManager theManager;
    public float timeElapsed = 0;
    public float totalTime = 5f;
    Slider theSlider;
    public bool hasStarted = false;
    public bool hasFinished = false;
    Timeline toPlay;
    // Start is called before the first frame update
    void Start()
    {
        theSlider = GetComponent<Slider>();
        theSlider.maxValue = totalTime;
        theSlider.minValue = 0;
        theManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStarted && !hasFinished && !theManager.isPaused){
            theSlider.value += Time.deltaTime;
            timeElapsed += Time.deltaTime;
        }
        if(timeElapsed >= totalTime && !hasFinished){
            hasFinished = true;
            theManager.IncrementTimeBlock();
        }        
    }

    public void StartPlayingBlock(){
        hasStarted = true;
        hasFinished = false;
    }

    public void ResetBlock(){
        hasStarted = false;
        hasFinished = false;
        theSlider.value = 0;
        timeElapsed = 0;
    }
}
