using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonSoundPlayer : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    bool isClicked = false;
    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!isClicked)
        {
            AudioManager.instance.PlaySound("ButtonHover");
        }
        
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        AudioManager.instance.PlaySound("ButtonClick");
        isClicked = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isClicked = false;
    }

}
