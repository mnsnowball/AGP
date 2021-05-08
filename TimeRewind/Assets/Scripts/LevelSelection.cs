using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    // this class will go on the level select screen books to turn them into buttons

    bool canClickMe = false;
    bool isLoading = false;
    public Outline outline;
    public LevelLoader loader;
    public int levelIndex = 0;
    Animator anim;
    public ParticleSystem particles;
    ParticleSystem.EmissionModule emission;
    private void OnMouseEnter()
    {
        if (!isLoading)
        {
            canClickMe = true;
            outline.enabled = true;
            anim.SetBool("isHovering", true);
            // enable particles
            emission.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        if (!isLoading)
        {
            canClickMe = false;
            outline.enabled = false;
            anim.SetBool("isHovering", false);
            // disable particles
            emission.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (!isLoading)
        {
            loader.LoadALevel(levelIndex);
            isLoading = true;
        }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        emission = particles.emission;
        canClickMe = false;
        outline.enabled = false;
        anim.SetBool("isHovering", false);
        // disable particles
        emission.enabled = false;
        if (!XMLManager.ins.unlockedLevels.isUnlocked[levelIndex])
        {
            // if the level that I am associated with has not been unlocked, disable myself
            //Debug.Log("Level at index " + levelIndex + " has not been unlocked");
            this.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        
    }
}
