using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ObjectHover : MonoBehaviour
{
    Animator myAnim;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Debug.Log("Clicked");
        myAnim.SetBool("Idle", false);
        myAnim.SetBool("Clicked", true);
    }

    private void OnMouseExit()
    {
        myAnim.SetBool("Clicked", false);
        myAnim.SetBool("Idle", true);
    }
}
