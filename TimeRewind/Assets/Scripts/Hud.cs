using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public static Hud instance;
    public int numberOfSpots;
    public List<BlockHUD> blockSpots;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        for (int index = 0; index < blockSpots.Count; index++)
        {
            if (index >= numberOfSpots)
            {
                blockSpots[index].gameObject.SetActive(false);
            }
            else
            {
                SetEmptyBlock(index);
            }
        }
    }

    public void UpdateBlock(int index, Direction theDirection, int numIterations)
    {
        blockSpots[index].SetActiveBlock(theDirection);
        blockSpots[index].SetIterationText(numIterations);
    }

    public void SetEmptyBlock(int index)
    {
        blockSpots[index].SetEmpty();
    }
}
