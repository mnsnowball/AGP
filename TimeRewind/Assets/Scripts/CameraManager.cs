using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CameraMode {Main, View, Paused};
public class CameraManager : MonoBehaviour
{
    public static CameraManager ins;
    public CameraMode mode;
    public CameraMode previousMode;

    public GameObject sinclairCam;
    public GameObject pauseCam;
    public GameObject sceneCam;
    public Animator loaderAnim;
    private CinemachineFreeLook look;
    bool canSwitch = true;
    bool isChangeEnabled = false;

    private void Awake()
    {
        // set up the singleton pattern
        if (ins == null)
        {
            ins = this;
            look = sceneCam.GetComponent<CinemachineFreeLook>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mode = CameraMode.Main;
        previousMode = CameraMode.Main;
        canSwitch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeEnabled)
        {
            return;
        }
        // if I have the wrong state, update it
        if (GameManager.instance.isPaused && mode != CameraMode.Paused) // if the game is paused and we haven't changed the state yet
        {
            //SetPausedView();
            StartCoroutine(TransitionTo(CameraMode.Paused, false));
        }
        else if (!GameManager.instance.isPaused && mode == CameraMode.Paused) // if the game is not paused and our state is still paused, change it back to the previous
        {
            //SwitchToMainView();
            StartCoroutine(TransitionTo(CameraMode.Main, false));
        }

        // handle switching if the tab key gets pressed
        if (Input.GetKey(KeyCode.Tab) && canSwitch)
        {
            canSwitch = false;
            switch (mode)
            {
                case CameraMode.Main:
                    //SwitchToClientView();
                    StartCoroutine(TransitionTo(CameraMode.View, true));
                    break;
                case CameraMode.View:
                    //SwitchToMainView();
                    StartCoroutine(TransitionTo(CameraMode.Main, true));
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            canSwitch = true;
        }
    }

    public void SwitchToClientView() 
    {
        if (!isChangeEnabled)
        {
            return;
        }
        GameManager.instance.StopPlayerMoving();
        Debug.Log("Switching to client view");
        previousMode = mode;
        mode = CameraMode.View;

        //sceneCam.SetActive(true);
        sinclairCam.SetActive(false);
        pauseCam.SetActive(false);
        mode = CameraMode.View;
    }

    public void SwitchToMainView() 
    {
        if (!isChangeEnabled)
        {
            return;
        }
        GameManager.instance.StartPlayerMoving();
        Debug.Log("Switching to sinclair view");
        previousMode = mode;
        mode = CameraMode.Main;

        sinclairCam.SetActive(true);
        //sceneCam.SetActive(false);
        pauseCam.SetActive(false);
        mode = CameraMode.Main;
    }

    public void SetPausedView() 
    {
        if (!isChangeEnabled)
        {
            return;
        }
        GameManager.instance.StopPlayerMoving();
        Debug.Log("Pausing");
        previousMode = mode;
        mode = CameraMode.Paused;

        pauseCam.SetActive(true);
        //mode = CameraMode.Paused;
        //sceneCam.SetActive(false);
        sinclairCam.SetActive(false);
        
    }

    public IEnumerator TransitionTo(CameraMode theMode, bool doTransition) 
    {

        if (doTransition)
        {
            loaderAnim.SetTrigger("CircleOut");
            yield return new WaitForSeconds(0.5f); // wait for the loader to animate and fill up the screen
        } 
        

        switch (theMode)
        {
            case CameraMode.Main:
                //look.PreviousStateIsValid = false;
                look.m_XAxis.Value = 0;
                yield return null;
                look.m_XAxis.m_InputAxisName = "";
                look.m_YAxis.m_InputAxisName = "";
                
                SwitchToMainView();
                break;
            case CameraMode.View:
                SwitchToClientView();
                look.m_XAxis.m_InputAxisName = "Mouse X";
                look.m_YAxis.m_InputAxisName = "Mouse Y";
                break;
            case CameraMode.Paused:
                //look.PreviousStateIsValid = false;
                look.m_XAxis.Value = 0;
                yield return null;
                look.m_XAxis.m_InputAxisName = "";
                look.m_YAxis.m_InputAxisName = "";
                SetPausedView();
                break;
            default:
                break;
        }

        if (doTransition)
        {
            yield return new WaitForSeconds(.5f); // wait for the camera to move
            loaderAnim.SetTrigger("CircleIn"); // animate the circle back down
        }
        
        yield return null;
    }

    public void SetChangeEnabled(bool toSet) {
        //Debug.Log("Setting change enabled to " + toSet);
        isChangeEnabled = toSet;
    }
}
