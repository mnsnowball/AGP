using UnityEngine;

public class BlockSpace : MonoBehaviour
{
    
    public int readerIndex; // my position in the space array
    public DirectionBlock blockHeld;

    //[HideInInspector]
    public bool hasBlock = false;


    private void Update()
    {
        if (hasBlock)
        {
            Hud.instance.UpdateBlock(readerIndex, blockHeld.direction, blockHeld.numberOfIterations);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "DirectionBlock")// if a direction block hits me, it's my direction block
        {
            blockHeld = other.gameObject.GetComponent<DirectionBlock>();
            hasBlock = true;
            Hud.instance.UpdateBlock(readerIndex, blockHeld.direction, blockHeld.numberOfIterations);
            //play sound
            // activate an effect
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "DirectionBlock")// if a direction block leaves me, I don't have one anymore
        {
            blockHeld = null;
            hasBlock = false;
            Hud.instance.SetEmptyBlock(readerIndex);
        }
    }
}
