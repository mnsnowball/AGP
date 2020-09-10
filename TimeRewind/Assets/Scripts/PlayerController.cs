using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    int speed = 5;
    float pickUpDist = 3f;
    public GameObject actionBlock;
    public GameObject pickedUpPosition;

    BlockSpot currentSpot;
    bool hasBlock;
    bool canChange;

    // Use this for initialization
    void Start () {
        canChange = true;
	}
	
	// Update is called once per frame
	void Update () {
        MovePlayer();

        float distToBlock = Vector3.Distance(transform.position, actionBlock.transform.position);
        bool isInRange = distToBlock < pickUpDist;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            canChange = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isInRange && !hasBlock && canChange)
        {
            PickUpBlock();
            canChange = false;
        } 
        if (Input.GetKeyDown(KeyCode.Space) && hasBlock && canChange)
        {
            PutDownBlock();
            canChange = false;
        } 
    }

    void MovePlayer(){
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        transform.position += new Vector3(input.x * Time.deltaTime * speed, 0, input.y * Time.deltaTime * speed); 
    }

    void PickUpBlock(){
        Debug.Log("Picking up");
        // if the button is being pressed and I'm in range of the actionblock
        // set its position to be above my head
        actionBlock.transform.position = pickedUpPosition.transform.position;
        actionBlock.transform.parent = pickedUpPosition.transform;
        hasBlock = true;
    }

    void PutDownBlock(){
        Debug.Log("Putting Down");
        // if there's a blockspot and I have a block, put it there
        if (currentSpot != null && hasBlock)
        {
            currentSpot.SetSliderValue();
            actionBlock.transform.parent = currentSpot.gameObject.transform;
            actionBlock.transform.position = currentSpot.gameObject.transform.position;
            hasBlock = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "blockSpot")
        {
            currentSpot = other.gameObject.GetComponent<BlockSpot>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "blockSpot" && other == currentSpot)
        {
            currentSpot = null;
        }
    }

}
