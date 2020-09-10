using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;

    private bool isMoving;
    private Vector3 input;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.z = Input.GetAxisRaw("Vertical");
        

            // makes it so you can't move diagonally
            if (input.x != 0)
            {
                input.z = 0;
            }

            if (input != Vector3.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.z += input.z;

                StartCoroutine(Move(targetPos));
            }
        }
    }

    IEnumerator Move(Vector3 targetPos){
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
}
