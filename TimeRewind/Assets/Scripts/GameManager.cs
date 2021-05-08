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

    private BlockReader reader;
    bool canPause;

    public bool isLevelComplete = false;
    public GameObject[] levelCompleteObjects;
    public GameObject levelFailedPanel;
    public GameObject pausePanel;
    List<AsyncOperation> scenesLoading;
    public PlayerControl playerControl;
    private Client client;
    //public ParticleSystem environmentParticles;
    public bool canPlay = false;
    public bool isPauseLocked = true;
    public int thisLevelIndex;
    bool isLevelFailed;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        isPaused = false;
        canPause = true;
        Time.timeScale = 1;
        reader = GameObject.FindObjectOfType<BlockReader>();
        client = GameObject.FindObjectOfType<Client>();
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
            Debug.Log("Playing Scene");
            Invoke("PlayScene", playDelay);
        }
    }


    //List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadGame()
    {
        loadingScreen.gameObject.SetActive(true);
        scenesLoading = new List<AsyncOperation>();
        
        scenesLoading.Add(SceneManager.LoadSceneAsync(1));
        
        StartCoroutine(GetSceneLoadProgress(scenesLoading));

    }

    float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress(List<AsyncOperation> theScenes)
    {

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

    public void SetPlay()
    {

        isPaused = false;
        //timeBlocks[currentTimeIndex].StartPlayingBlock();
    }

    public void SetPause(bool toSet)
    {
        isPaused = toSet;
        pausePanel.GetComponent<CanvasGroup>().interactable = isPaused;
        pausePanel.GetComponent<Animator>().SetBool("isPaused", isPaused);
    }
    
    // enables UI to say the level is complete and sets time scale to zero
    public void LevelComplete()
    {
        Debug.Log("Level complete!");
        AudioManager.instance.PlaySound("LevelComplete");
        isLevelComplete = true;
        EnableLevelCompleteObjects();
        playerControl.Victory();
        client.LevelComplete();
        //environmentParticles.TriggerSubEmitter(0);
        if (thisLevelIndex < (XMLManager.ins.unlockedLevels.isUnlocked.Count - 2))
        {
            XMLManager.ins.unlockedLevels.isUnlocked[thisLevelIndex + 1] = true;
        }
       
    }

    public void LevelFailed()
    {
        if (!isLevelFailed)
        {
            isLevelFailed = true;
            levelFailedPanel.SetActive(true);
            AudioManager.instance.PlaySound("LevelFailed");
        }
    }

    // enables any object passed in
    public void EnableObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void EnableLevelCompleteObjects() 
    {
        foreach (GameObject item in levelCompleteObjects)
        {
            if (item != null)
            {
                EnableObject(item);
            }
        }
    }

    // disables any object passed in
    public void DisableObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    // runs the LevelComplete function after toWait seconds
    public void WaitThenLevelComplete(float toWait)
    {
        Invoke("LevelComplete", toWait);
    }

    void PlayScene()
    {
        reader.Play();
        playerControl.StopMoving();
        playerControl.PlaySceneAnim();
        AudioManager.instance.PlaySound("PlayScene");
        CameraManager.ins.SetChangeEnabled(true);

        bool doTransition = true;
        CameraManager.ins.StartCoroutine(CameraManager.ins.TransitionTo(CameraMode.View, doTransition));
    }

    public void StopPlayerMoving()
    {
        playerControl.StopMoving();
    }

    public void StartPlayerMoving()
    {
        playerControl.StartMoving();
    }
}
