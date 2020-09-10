using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSpot : MonoBehaviour
{
    public bool hasSliderAssociation; // used to tell if we need to set the slider value when we get a block
    public int spotIndex;
    public Slider timeSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSliderValue(){
        if (hasSliderAssociation)
        {
            timeSlider.value = spotIndex;
        }
    }
}
