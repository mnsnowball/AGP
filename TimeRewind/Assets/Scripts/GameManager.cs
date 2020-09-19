using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject loadingScreen;
    public Slider progressBar;


    Timeline[] timelines;
    public bool isPaused;
    //public TimeBlock[] timeBlocks;
    public int currentTimeIndex = 0;
    public GameObject levelCompletePanel;
    List<AsyncOperation> scenesLoading;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;


        //timelines = GameObject.FindObjectsOfType(typeof(Timeline)) as Timeline[];
        //timeBlocks[0].StartPlayingBlock();
        //DisableObject(levelCompletePanel);
        //Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (timeBlocks[timeBlocks.Length - 1].hasFinished)
        //{
            //isPaused = true;
        //}
    }


    //List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadGame(){
        loadingScreen.gameObject.SetActive(true);
        scenesLoading = new List<AsyncOperation>();
        
        scenesLoading.Add(SceneManager.LoadSceneAsync(1));
        
        StartCoroutine(GetSceneLoadProgress(scenesLoading));

    }

    float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress(List<AsyncOperation> theScenes){

        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while(!scenesLoading[i].isDone){
                totalSceneProgress = 0;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                Debug.Log(totalSceneProgress);
                progressBar.value = totalSceneProgress;

                yield return null;
            }

            loadingScreen.gameObject.SetActive(false);
        }
    }

    public void SetPlay(){

        isPaused = false;
        //timeBlocks[currentTimeIndex].StartPlayingBlock();
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

    /*public void IncrementTimeBlock(){
        if(currentTimeIndex < timeBlocks.Length - 1){ // if we're not at the end of time then move on to the next block
            currentTimeIndex++;
            timeBlocks[currentTimeIndex].StartPlayingBlock();
        } 
    }*/

    /*public void DecrementTimeBlock(){
        if(currentTimeIndex > 0){ // if we're not at the end of time then move on to the next block
            currentTimeIndex--;
            timeBlocks[currentTimeIndex].StartPlayingBlock();
        } 
    }

    // resets the scene without needing to load it again
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
    }*/
    
    // enables UI to say the level is complete and sets time scale to zero
    public void LevelComplete(){
        EnableObject(levelCompletePanel);
        //Time.timeScale = 0;
    }

    // enables any object passed in
    public void EnableObject(GameObject obj){
        obj.SetActive(true);
    }

    // disables any object passed in
    public void DisableObject(GameObject obj){
        obj.SetActive(false);
    }

    // runs the LevelComplete function after toWait seconds
    public void WaitThenLevelComplete(float toWait){
        Invoke("LevelComplete", toWait);
    }
}
