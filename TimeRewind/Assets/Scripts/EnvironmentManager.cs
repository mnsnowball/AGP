using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public int labXSize;
    public int labYSize;

    // holds data regarding whether any given spot in the wizard lab is traversable
    // used by the player controller to determine if the player can move in a direction
    public List<List<bool>> wizardLab;

    // the layout of the level the player is giving directions for
    // used by the timeLine script when executing the directions the player gave
    List<List<bool>> directionEnvironment;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
