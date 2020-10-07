using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField]
    private Array2DBool array2DBool;
    bool[,] cells;

    // Start is called before the first frame update
    void Start()
    {
        cells = array2DBool.GetCells();
        //Debug.Log("x size is " + array2DBool.GridSize.x + " and y size is " + array2DBool.GridSize.y);
        //Debug.Log("x size is " + cells.GetLength(0) + " and y size is " + cells.GetLength(1));
    }

    // checks if a space is occupied and returns the result
    public bool IsOccupied(int x, int y){
        return cells[x, y];
    }


    // sets the bool at coordinate [x, y] 
    public void SetOccupied(int x, int y, bool toSet){
        cells[x, y] = toSet;

    }

}
