using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOne : MonoBehaviour
{
    GameManager theManager;

    // Start is called before the first frame update
    void Start()
    {
        theManager  = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Bus")
        {
            Invoke("LevelComplete", 0.5f);
            Invoke("DestroyTheThing", 1.5f);
        }
    }

    private void LevelComplete(){
        theManager.WaitThenLevelComplete(3f);
    }

    private void DestroyTheThing(){
        Destroy(this.gameObject);
    }
}
