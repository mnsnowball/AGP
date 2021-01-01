using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    public Queue<Direction> directions;
    bool isMoving;
    public float moveIncrement = 1f;
    public float moveSpeed = 5f;
    public bool hasFinished = false;
    public int startPosX = 0;
    public int startPosY = 0;
    public EnvironmentManager em;
    int currentXPos = 0;
    int currentYPos = 0;
    // Start is called before the first frame update
    void Start()
    {
        directions = new Queue<Direction>();
        currentXPos = startPosX;
        currentYPos = startPosY;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving && directions.Count > 0)
        {
            ReadDirection(directions.Dequeue());
        }
        if (hasFinished && !isMoving && directions.Count == 0 && !GameManager.instance.isLevelComplete)
        {
            Debug.Log("Level Failed");
            GameManager.instance.LevelFailed();
        }
    }

    public void AddDirection(Direction toAdd){
        directions.Enqueue(toAdd);
    }

    public void AddDirectionSet(List<Direction> toAdd){
        for (int i = 0; i < toAdd.Count; i++)
        {
            Debug.Log("Adding " + toAdd[i]);
            directions.Enqueue(toAdd[i]);
        }
    }

    public void ReadDirection(Direction toRead){
        Debug.Log("Client: Reading " + toRead);
        Vector3 target = new Vector3(0,0,0);
        int targetX = currentXPos;
        int targetY = currentYPos;
        switch (toRead)
        {
            case(Direction.up):
                target = new Vector3(0, 0, moveIncrement);
                target += transform.position;
                targetY--;
            break;
            case(Direction.down):
                target = new Vector3(0, 0, -moveIncrement);
                target += transform.position;
                targetY++;
            break;
            case(Direction.left):
                target = new Vector3(-moveIncrement, 0, 0);
                target += transform.position;
                targetX--;
            break;
            case(Direction.right):
                target = new Vector3(moveIncrement, 0, 0);
                target += transform.position;
                targetX++;
            break;
            default:
            break;
        }

        //Debug.Log("Can client go? " + EnvironmentManager.instance.CanClientGo(targetY, targetX));
        if (!EnvironmentManager.instance.HasClientObstacle(targetY, targetX))
        {
            StartCoroutine(Move(target));
            currentYPos = targetY;
            currentXPos = targetX;
        } else {
            Debug.Log("Client has gotten a direction it can't follow");
        }
        
    }

    public IEnumerator Move(Vector3 targetPos){
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        transform.position = targetPos;
        isMoving = false;
    }
}
