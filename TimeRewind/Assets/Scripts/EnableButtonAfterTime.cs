using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableButtonAfterTime : MonoBehaviour
{
    public GameObject theButton;
    public float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        theButton.SetActive(false);
        StartCoroutine(EnableAfterSecs());
    }

    IEnumerator EnableAfterSecs()
    {
        float timeElapsed = 0;
        while (timeElapsed < waitTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        theButton.SetActive(true);
        yield return null;
    }
}
