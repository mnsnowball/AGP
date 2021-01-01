using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light light;

    //public float maxIntensityChange;
    public float maxIntensity;
    public float minIntensity;
    bool isDoneChanging;

    //public float maxRangeChange;
    public float maxRange;
    public float minRange;

    public float changeSpeed;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        light.range = Random.Range(minRange, maxRange);

        if (isDoneChanging)
        {
            float intensityChange = Random.Range(minIntensity, maxIntensity);
            StartCoroutine(ChangeIntensity(intensityChange));
        }
        
    }

    IEnumerator ChangeIntensity(float changeTo){
        isDoneChanging = false;
        float elapsedTime = 0f;
        while (Mathf.Abs(light.intensity - changeTo) < 0.01f )
        {
            light.intensity = Mathf.Lerp(light.intensity, changeTo, elapsedTime);
            elapsedTime += changeSpeed * Time.deltaTime;
            yield return null;
        }
        isDoneChanging = true;
    }
}
