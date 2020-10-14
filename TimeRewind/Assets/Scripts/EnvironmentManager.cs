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

    private DirectionBlock[,] directionBlocks;

    // Start is called before the first frame update
    void Start()
    {
        obstacleCells = obstacles.GetCells();
        blockCells = blocks.GetCells();
        //Debug.Log("x size is " + array2DBool.GridSize.x + " and y size is " + array2DBool.GridSize.y);
        //Debug.Log("x size is " + cells.GetLength(0) + " and y size is " + cells.GetLength(1));
    }

    // checks if a space is occupied and returns the result
    public bool IsOccupied(int x, int y){
        return obstacleCells[x, y];
    }

    public bool HasBlock(int x, int y){
        return blockCells[x, y];
    }


    // sets the bool at coordinate [x, y] 
    public void SetOccupied(int x, int y, bool toSet){
        obstacleCells[x, y] = toSet;

    }

    public bool getBlock(int x, int y){
        return blockCells[x, y];
    }

}
