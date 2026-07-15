using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class MenuController : MonoBehaviour
{

    public GameObject menuUI;

    public GameObject combatUI;

    public GameObject menuCamera;
    public GameObject combatCamera;

    public PlayableDirector cutscene;

    public float timerToGameplay = 5;
    private bool startTimer = false;


    public void StartGame()
    {
        menuUI.SetActive(false);
        cutscene.Play();
        startTimer = true;
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }

    void Update()
    {
        if (startTimer)
        {
            timerToGameplay -= Time.deltaTime;

            if (timerToGameplay <= 0)
            {
                combatUI.SetActive(true);
                menuCamera.SetActive(false);
                combatCamera.SetActive(true);
                startTimer = false;
            }
        }
    }
}
