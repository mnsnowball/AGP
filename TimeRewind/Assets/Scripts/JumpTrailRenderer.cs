using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class JumpTrailRenderer : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Transform endPosition;
    public float angle;
    public int resolution = 10;
    public float maxHeight = 1f;
    public float minDistance = 0.01f;
    public float middle = 0.6f;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Update()
    {
        RenderArc();
    }
    
    // populating the line renderer with the appropriate settings
    void RenderArc(){
        lineRenderer.positionCount = (resolution + 1);
        lineRenderer.SetPositions(CalculateArcArray());
        // set position 0 to my transform
        // set position [resolution] to goal transform
    }

    Vector3[] CalculateArcArray(){
        Vector3[] arcArray = new Vector3[resolution + 1];
        Vector3 start = transform.position;
        Vector3 endBase = endPosition.position;
        float tipHeight = endBase.y - start.y;
        endBase.y = start.y;

        float distance = Vector3.Distance(start, endBase);
        if (distance < minDistance) {
            distance = minDistance;
        }
        Vector3 direction = (endBase - start) / distance;
        float a = 0.0f;
        float b = 0.0f;
        CalculateParabolaParameters(out a, out b, maxHeight, tipHeight, distance, middle);

        arcArray[0] = start;
        for (int i = 1; i <= resolution; i++)
        {
            float x = (float)i / (float)resolution * distance;
            float y = CalculateParabola(a, b, 0.0f, x);
            arcArray[i] = start + direction * x + Vector3.up * y;
        }

        return arcArray;
    }

    float CalculateParabola(float a, float b, float c, float x) {
        // parabola equation in standard form
        return a * x * x + b * x + c;
    }

    void CalculateParabolaParameters(
        out float a, // a and b are coefficients of the standard form:
        out float b, // y = ax² + bx + c where c = 0
        float h,     // h is the height of the parabola
        float ht,    // ht is the height of the target (staff tip)
        float d,     // d is the distance to the target (staff tip)
        float f)     // f is the fraction of the distance to the vertex
    {
        a = (ht - (h / f)) / (d * d * (1.0f - f));
        b = (h / (f * d)) - a * f * d;
    }
}
