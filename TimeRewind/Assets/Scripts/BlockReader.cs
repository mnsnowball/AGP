using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockReader : MonoBehaviour
{
    public BlockSpace[] spaces;
    public int numberOfSpaces;
    public Client theClient;

    private void Start() {
        
    }

    public void Play(){
        Debug.Log("Playing");
        for (int i = 0; i < spaces.Length; i++)
        {
            if (spaces[i].hasBlock)
            {
                theClient.AddDirection(spaces[i].blockHeld.direction);
            }
        }

    }
}
