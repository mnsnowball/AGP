using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpace : MonoBehaviour
{
    public BlockReader reader;
    public int readerIndex; // my position in the space array
    public DirectionBlock blockHeld;
    public bool hasBlock = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "DirectionBlock")// if a direction block hits me, it's my direction block
        {
            blockHeld = other.gameObject.GetComponent<DirectionBlock>();
            hasBlock = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "DirectionBlock")// if a direction block leaves me, I don't have one anymore
        {
            blockHeld = null;
            hasBlock = false;
        }
    }
}
