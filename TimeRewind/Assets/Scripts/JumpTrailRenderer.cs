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
    public float velocity = 5f;
    public float maxHeight = 1f;

    float g;
    float radianAngle;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics.gravity.y);
    }

    // Start is called before the first frame update
    void Update()
    {
        RenderArc();
    }
    
    // populating the line renderer with the appropriate settings
    void RenderArc(){
        lineRenderer.SetVertexCount(resolution + 1);
        lineRenderer.SetPositions(CalculateArcArray());
        // set position 0 to my transform
        // set position [resolution] to goal transform
    }

    Vector3[] CalculateArcArray(){
        Vector3[] arcArray = new Vector3[resolution + 1];
        arcArray[0] = this.transform.position;
        arcArray[resolution] = endPosition.transform.position;
        radianAngle = Mathf.Deg2Rad * angle;
        float distance = Vector3.Distance(this.transform.position, endPosition.transform.position);

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, distance);
        }

        return arcArray;
    }

    float CalculateAngle(){
        float xMid = (transform.position.x + endPosition.transform.position.x)/2;
        float yMid = ((transform.position.y + endPosition.transform.position.y)/2) + maxHeight;
        float zMid = (transform.position.z + endPosition.transform.position.z)/2;
        Vector3 midPoint = new Vector3(xMid, yMid, zMid);
        return Vector3.Angle(this.transform.position, midPoint);
    }

    Vector3 CalculateArcPoint(float t, float maxDistance){
        Vector2 groundStartPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 groundEndPos = new Vector2(endPosition.transform.position.x, endPosition.transform.position.z);

        Vector2 lerp = Vector2.Lerp(groundStartPos, groundEndPos, t);

        float x = lerp.x;
        float y = (x * Mathf.Tan(radianAngle) - ((g * x * x )/(2 * velocity * velocity * Mathf.Cos(radianAngle)* Mathf.Cos(radianAngle)))) + (Mathf.Lerp(transform.position.y, endPosition.transform.position.y, t));
        // float y = Mathf.Lerp(transform.position.y, endPosition.transform.position.y, t);
        float z = lerp.y;
        return new Vector3(x, y, z);
    }
}
