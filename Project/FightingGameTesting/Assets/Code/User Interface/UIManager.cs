using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    //this class should be responsible for initializing all UI elements that need intialization, and should host singleton-accessible methods to access UI features.

    public static UIManager instance;

    public Slider healthBar1;
    public Slider healthBar2;

    public TMP_Text timer;

    public CharacterController player1;
    public CharacterController player2;


    private void Awake()
    {
        CreateSingleton();
    }


    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player 1").GetComponent<CharacterController>();
        player2 = GameObject.FindGameObjectWithTag("Player 2").GetComponent<CharacterController>();

        InitializeHealthBars();
    }

    void CreateSingleton()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    void InitializeHealthBars()
    {
        healthBar1.maxValue = player1.health;
        healthBar2.maxValue = player2.health;
        healthBar1.value = player1.health;
        healthBar2.value = player2.health;
    }

    public void UpdateHealthBars()
    {
        healthBar1.value = player1.health;
        healthBar2.value = player2.health;
    }

    public void UpdateTimer()
    {
        int time = (int)GameManager.instance.currentTime;
        timer.text = time.ToString();
    }
}
