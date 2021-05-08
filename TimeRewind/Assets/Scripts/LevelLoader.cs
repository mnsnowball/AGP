using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadALevel(int index){
        StartCoroutine(LoadLevel(index));
        if (SceneManager.GetActiveScene().buildIndex < 2 && index >= 2)
        {
            // transition audio out
            // allows transition to level audio
            AudioManager.instance.DeleteSelf();
        }

        if (SceneManager.GetActiveScene().buildIndex >= 2 && index < 2)
        {
            // transition audio out
            // allow transition back to menu audio
            AudioManager.instance.DeleteSelf();
        }
    }

    public void TransitionSound() {
        // this function should be called by the LoadLevel ienumerator
        // transition sound over a specified number of seconds to a specified level
        // then transition a specific sound in
    }

    public IEnumerator LoadLevel(int buildIndex){
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(buildIndex);
    }
}
