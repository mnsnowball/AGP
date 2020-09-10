using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Timeline[] timelines;
    public bool isPaused;
    public TimeBlock[] timeBlocks;
    public int currentTimeIndex = 0;
    public GameObject levelCompletePanel;

    // Start is called before the first frame update
    void Start()
    {
        timelines = GameObject.FindObjectsOfType(typeof(Timeline)) as Timeline[];
        timeBlocks[0].StartPlayingBlock();
        DisableObject(levelCompletePanel);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBlocks[timeBlocks.Length - 1].hasFinished)
        {
            isPaused = true;
        }
    }

    public void SetPlay(){

        isPaused = false;
        timeBlocks[currentTimeIndex].StartPlayingBlock();
    }

    public void SetPause(bool toSet){
        isPaused = toSet;
    }


    /*void UpdateTimelines(bool timeChanged){
        for(int i = 0; i < timelines.Length; i++){
            if(!timelines[i].goalReached){
                timelines[i].HandleGoals(timeChanged);
            }            
        }
    }*/

    public void IncrementTimeBlock(){
        if(currentTimeIndex < timeBlocks.Length - 1){ // if we're not at the end of time then move on to the next block
            currentTimeIndex++;
            timeBlocks[currentTimeIndex].StartPlayingBlock();
        } 
    }

    public void DecrementTimeBlock(){
        if(currentTimeIndex > 0){ // if we're not at the end of time then move on to the next block
            currentTimeIndex--;
            timeBlocks[currentTimeIndex].StartPlayingBlock();
        } 
    }

    public void ResetBlocks(){
        for (int i = 0; i < timeBlocks.Length; i++)
        {
            timeBlocks[i].ResetBlock();
        }
        for (int i = 0; i < timelines.Length; i++)
        {
            timelines[i].ResetSelf();
        }
        isPaused = true;
        currentTimeIndex = 0;
    }
    
    public void LevelComplete(){
        EnableObject(levelCompletePanel);
        Time.timeScale = 0;
    }

    public void EnableObject(GameObject obj){
        obj.SetActive(true);
    }

    public void DisableObject(GameObject obj){
        obj.SetActive(false);
    }

    public void WaitThenLevelComplete(float toWait){
        Invoke("LevelComplete", toWait);
    }
}
