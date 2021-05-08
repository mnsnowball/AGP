using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockHUD : MonoBehaviour
{
    public GameObject upBlock, 
                      rightBlock, 
                      leftBlock, 
                      downBlock, 
                      jumpBlock;

    public TextMeshProUGUI numberText;

    public void SetActiveBlock(Direction toSet) 
    {
        upBlock.SetActive(false);
        rightBlock.SetActive(false);
        leftBlock.SetActive(false);
        downBlock.SetActive(false);
        jumpBlock.SetActive(false);
        switch (toSet)
        {
            case Direction.up:
            { 
                upBlock.SetActive(true);
                break;
            }

            case Direction.down:
            { 
                downBlock.SetActive(true);
                break;
            }

            case Direction.left:
            {
                leftBlock.SetActive(true);
                break;
            }
                
            case Direction.right:
            { 
                rightBlock.SetActive(true);
                break;
            }

            case Direction.jump:
            { 
                jumpBlock.SetActive(true);
                break;
            }

            default:
            { 
                Debug.LogWarning("Invalid direction");
                break;
            }
        }
    }

    public void SetEmpty() 
    {
        upBlock.SetActive(false);
        rightBlock.SetActive(false);
        leftBlock.SetActive(false);
        downBlock.SetActive(false);
        jumpBlock.SetActive(false);
        numberText.text = "";
    }

    public void SetIterationText(int numberOfIterations) 
    {
        numberText.text = "x " + numberOfIterations.ToString();
    }
}
