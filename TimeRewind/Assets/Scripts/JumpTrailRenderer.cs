using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class JumpTrailRenderer : MonoBehaviour
{
    LineRenderer lineRenderer;
    public GameObject endPosition;
    public float angle;
    public int resolution = 10;

    public float maxHeight = 1f;

    float g;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics.gravity.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        RenderArc();
    }

    private void Update() {
        RenderArc();
    }
    
    // populating the line renderer with the appropriate settings
    void RenderArc(){
        lineRenderer.SetVertexCount(5);
        lineRenderer.SetPositions(CalculateArcArray());
        // set position 0 to my transform
        // set position [resolution] to goal transform
    }

    Vector3[] CalculateArcArray(){
        Vector3[] arcArray = new Vector3[5];
        float distance = Vector3.Distance(this.transform.position, endPosition.transform.position);

        Vector3 start = transform.position;
        float xMid = (transform.position.x + endPosition.transform.position.x)/2;
        float yMid = ((transform.position.y + endPosition.transform.position.y)/2) + maxHeight;
        float zMid = (transform.position.z + endPosition.transform.position.z)/2;
        Vector3 midPoint = new Vector3(xMid, yMid, zMid);

        arcArray[0] = start;
        arcArray[1] = GetMidPoint(start, midPoint, 0.15f);
        arcArray[2] = midPoint;
        arcArray[3] = GetMidPoint(start, endPosition.transform.position, 0.15f);
        arcArray[4] = endPosition.transform.position;


        return arcArray;
    }

    Vector3 GetMidPoint(Vector3 a, Vector3 b, float smoothing){
        float xMid = (a.x + b.x)/2f;
        float yMid = ((a.y + b.y)/2f);
        float zMid = (a.z + b.z)/2f;
        Vector3 midPoint = new Vector3(xMid, yMid, zMid);
        return midPoint;
    }

}
