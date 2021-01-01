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


    public bool isPaused;
    public bool isPlaying;
    public float playDelay = 0.75f;
    public BlockReader reader;
    bool canPause;
    //public TimeBlock[] timeBlocks;
    public int currentTimeIndex = 0;
    public bool isLevelComplete = false;
    public GameObject levelCompletePanel;
    public GameObject levelFailedPanel;
    public GameObject pausePanel;
    List<AsyncOperation> scenesLoading;
    public PlayerControl playerControl;
    public bool canPlay = false;
    bool isLevelFailed;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        isPaused = false;
        canPause = true;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            canPause = false;
            SetPause(!isPaused);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            canPause = true;
        }

        if (Input.GetKey(KeyCode.Space) && !isPlaying && canPlay)
        {
            isPlaying = true;
            Invoke("PlayScene", playDelay);
        }
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
        pausePanel.SetActive(isPaused);
    }
    
    // enables UI to say the level is complete and sets time scale to zero
    public void LevelComplete(){
        Debug.Log("Level complete!");
        isLevelComplete = true;
        EnableObject(levelCompletePanel);
        //Time.timeScale = 0;
    }

    public void LevelFailed(){
        if (!isLevelFailed)
        {
            isLevelFailed = true;
            levelFailedPanel.SetActive(true);
        }
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

    void PlayScene(){
        reader.Play();
    }

    public void StopPlayerMoving(){
        playerControl.StopMoving();
    }

    public void StartPlayerMoving(){
        playerControl.StartMoving();
    }
}
