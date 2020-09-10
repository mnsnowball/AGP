using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timeline : MonoBehaviour
{
    public GameManager theManager;
    public List<AiPath> thePath;
    int pathIndex;
    
    AiPath currentGoal;
    public bool goalReached;
    public float goalReachRange = 0.05f;
    public float speed = 5;
    Vector3 startPosition;
    public bool hasStarted;
    public int startIndex = 3;
    public Slider timeSlider;
    public bool isAffectedBySlider;
    private bool isWaiting;
    float timeElapsed = 0;
    float timeToWait = 0;

    // Start is called before the first frame update
    void Start()
    {
        theManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        pathIndex = 0;
        currentGoal = thePath[0];
        goalReached = false;
        isWaiting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting)
        {
            if (isAffectedBySlider)
            {
                startIndex = (int)timeSlider.value;
            }
            
            if (theManager.currentTimeIndex == startIndex && !hasStarted)
            {
                SetStart(true);
            }

            goalReached = IsGoalReached();
            if(goalReached){
                HandleGoals();
            }
            currentGoal = thePath[pathIndex];
            if(!theManager.isPaused && hasStarted){
                transform.position = Vector3.MoveTowards(transform.position, currentGoal.transform.position, speed * Time.deltaTime);
            } else{
                transform.position = Vector3.MoveTowards(transform.position, transform.position, speed * Time.deltaTime);
            }
        } 
        else // waiting for specified time
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeToWait)
            {
                isWaiting = false;
                timeElapsed = 0;
            }
        }
        
    }

    // returns true if we are in range of the goal
    bool IsGoalReached(){
        return (Vector3.Distance(this.transform.position, currentGoal.transform.position) < goalReachRange);
    }

    public void HandleGoals(){
        if((goalReached && pathIndex != thePath.Count - 1)){
            Debug.Log("Handling Goals");
        //switch(theManager.currentTimeState){
            
                //case(TimeState.forward):
                if (thePath[pathIndex].hasWaitTime)
                {
                    isWaiting = true;
                    timeElapsed = 0;
                    timeToWait = thePath[pathIndex].waitTime;
                }
                if(pathIndex < thePath.Count - 1){ // if we're not at the end of the path, increment the  path index
                        pathIndex++; // move to the next point in the path
                }
                //break;
                
                //case(TimeState.backward):
                //if (thePath[pathIndex].hasWaitTime)
                //{
                //    isWaiting = true;
                //    timeElapsed = 0;
                //    timeToWait = thePath[pathIndex].waitTime;
                //}
                //if(pathIndex > 0){ // if we're not at the end of the path, increment the  path index
                //    pathIndex--; // move to the next point in the path
                //}
                
                //break;
            //}
        }
    }

    public void SetStart(bool toSet){
        hasStarted = toSet;
    }

    public void ResetSelf(){ // resets everything back to its start position/state
        transform.position = startPosition;
        pathIndex = 0;
        isWaiting = false;
        timeElapsed = 0;
        goalReached = false;
        hasStarted = false;
    }

}
