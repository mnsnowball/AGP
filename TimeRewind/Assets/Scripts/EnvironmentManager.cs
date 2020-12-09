using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;


public class EnvironmentManager : MonoBehaviour
{
    [SerializeField]
    private Array2DBool obstacles;
    [SerializeField]
    private Array2DBool blocks;
    bool[,] obstacleCells;
    bool[,] blockCells;

    public List<DirectionBlock> directionBlocks; // will check the indices of these against those passed in
    public float moveIncrement = 1;

    // Start is called before the first frame update
    void Start()
    {
        obstacleCells = obstacles.GetCells();
        blockCells = blocks.GetCells();
    }

    // checks if a space is occupied and returns the result
    public bool IsOccupied(int x, int y){
        return obstacleCells[x, y];
    }

    public bool HasBlock(int x, int y){
        //Debug.Log("The block at " + x + ", " + y + " is " + blockCells[x, y]);
        return blockCells[x, y];
    }


    // sets the bool at coordinate [x, y] 
    public void SetOccupied(int x, int y, bool toSet){
        obstacleCells[x, y] = toSet;
    }

    public void SetBlock(int x, int y, bool toSet){
        blockCells[x, y] = toSet;
    }

    // this function finds the direction with the indices currentX and currentY and moves it to indices of targetX and targetY
    public void MoveBlock(int currentY, int currentX, int targetY, int targetX){
        DirectionBlock toMove = null;
        // find the block whose indices match currentY and currentX
        for (int i = 0; i < directionBlocks.Count; i++)
        {
            if (directionBlocks[i].yPosition == currentY && directionBlocks[i].xPosition == currentX)
            {
                toMove = directionBlocks[i];
                //Debug.Log("Found correct block!");
            }
        }
        if (toMove == null)
        {
            Debug.LogError("Unable to find block");
        } else { // we have access to the block we need to move
            // if currentY != targetY we're moving vertically
            if (currentY != targetY)
            {
                // if targetY is less, move up one
                if (targetY < currentY)
                {
                    toMove.MoveToken(new Vector3(toMove.gameObject.transform.position.x, toMove.gameObject.transform.position.y, toMove.gameObject.transform.position.z + moveIncrement));
                    // fix indices
                    //increment y
                    toMove.DecrementY();
                    //blockCells[currentY, currentX] = false;
                    SetBlock(currentY, currentX, false);
                    //blockCells[targetY, targetX] = true;
                    SetBlock(targetY, targetX, true);
                } else // if it's less, move down one
                {
                    toMove.MoveToken(new Vector3(toMove.gameObject.transform.position.x, toMove.gameObject.transform.position.y, toMove.gameObject.transform.position.z - moveIncrement));
                    // fix indices
                    // decrement Y
                    toMove.IncrementY();
                    //blockCells[currentY, currentX] = false;
                    SetBlock(currentY, currentX, false);
                    //blockCells[targetY, targetX] = true;
                    SetBlock(targetY, targetX, true);
                }
                
            } else if (currentX != targetX) // moving vertically
            {
                // if targetX is greater, move to the right
                if (targetX > currentX)
                {
                    toMove.MoveToken(new Vector3(toMove.gameObject.transform.position.x + moveIncrement, toMove.gameObject.transform.position.y, toMove.gameObject.transform.position.z));
                    // fix indices
                    // increment x
                    toMove.IncrementX();
                    //blockCells[currentY, currentX] = false;
                    SetBlock(currentY, currentX, false);
                    //blockCells[targetY, targetX] = true;
                    SetBlock(targetY, targetX, true);
                } else // if it's less, move to the left
                {
                    toMove.MoveToken(new Vector3(toMove.gameObject.transform.position.x - moveIncrement, toMove.gameObject.transform.position.y, toMove.gameObject.transform.position.z));
                    // fix indices
                    // decrement x
                    toMove.DecrementX();
                    //blockCells[currentY, currentX] = false;
                    SetBlock(currentY, currentX, false);
                    //blockCells[targetY, targetX] = true;
                    SetBlock(targetY, targetX, true);
                }
            } else // else debug.Error because we shouldn't get here
            {
                Debug.LogError("currentY and currentX = targetY and targetX, this function should not have been called");
            }
        
        }
    }

    // returns a reference to the direction block at a specified environment coordinate
    public DirectionBlock GetBlock(int x, int y){
        //Debug.Log("Getting blocks at [" + x + ", " + y + "]");
        DirectionBlock theBlock = null;
        for (int i = 0; i < directionBlocks.Count; i++)
        {
            if (directionBlocks[i].yPosition == x && directionBlocks[i].xPosition == y)
            {
                theBlock = directionBlocks[i];
            }
        }
        return theBlock;
    }

}
