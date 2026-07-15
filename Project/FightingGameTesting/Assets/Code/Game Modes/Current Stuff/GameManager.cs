using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //references
    public static GameManager instance;
    public CharacterController player1;
    public CharacterController player2;

    [Header("Game Mode Data")]
    public int roundsInMatch = 3;
    public int roundTime = 60;
    public int scorePerRound = 1;
    public int scoreToWin = 2;
    public int streakToWin = 1;

    private bool player1WinsMatch;
    private bool player2WinsMatch;

    private bool player1WinsRound;
    private bool player2WinsRound;

    private bool matchOver;
    private bool roundOver;

    [Header("Game State")]
    public int currentRound;
    public float currentTime;

    [Header("Tally")]
    public Winner[] rounds;

    [Header("Winner")]
    public Winner winner;


    void Awake()
    {
        CreateSingleton();
    }
    void CreateSingleton()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetPlayers();

        rounds = new Winner[roundsInMatch];
        winner = Winner.Undecided;

        scoreToWin = roundsInMatch / 2 + 1;

        currentRound = 1;
        currentTime = roundTime;

    }

    // Update is called once per frame
    void Update()
    {

        UpdateTimer();

        player1WinsMatch = CheckMatchWin(player1);
        player2WinsMatch = CheckMatchWin(player2);

        player1WinsRound = CheckRoundLoss(player2);
        player2WinsRound = CheckRoundLoss(player1);


        matchOver = CheckMatchOver();
        roundOver = CheckRoundOver();

        ResolveGame();
    }

    void GetPlayers()
    {
        player1 = GameObject.FindGameObjectWithTag("Player 1").GetComponent<CharacterController>();
        player2 = GameObject.FindGameObjectWithTag("Player 2").GetComponent<CharacterController>();
    }

    bool CheckMatchWin(CharacterController player)
    {
        //check score
        if (player.score < scoreToWin)
            return false;
        //check streak
        if (player.streak < streakToWin)
            return false;

        return true;

    }

    bool CheckRoundLoss(CharacterController player)
    {
        if (player.health <= 0)
            return true;
        else
            return false;
    }

    bool CheckMatchOver()
    {
        if (currentRound > roundsInMatch)
            return true;
        else
            return false;
    }

    bool CheckRoundOver()
    {
        if (currentTime <= 0)
            return true;
        else
            return false;
    }

    void ResolveGame()
    {
        //tie match
        if ((player1WinsMatch && player2WinsMatch))
        {
            winner = Winner.Tie;

            Debug.Log("Outcome: it's a tie!");
            return;
        }

        //player 1 match win
        if (player1WinsMatch && !player2WinsMatch)
        {
            winner = Winner.Player1;

            Debug.Log("Outcome: player 1 wins!");
            return;
        }

        //player 2 match win
        if (player2WinsMatch && !player1WinsMatch)
        {
            winner = Winner.Player2;

            Debug.Log("Outcome: player 2 wins!");
            return;
        }

        //draw match
        if(matchOver)
        {
            winner = Winner.Draw;

            Debug.Log("Outcome: it's a draw!");
            return;
        }

        //tie round
        if(player1WinsRound && player2WinsRound)
        {
            rounds[currentRound - 1] = Winner.Tie;

            player1.score++;
            player2.score++;
            currentRound++;
            ResetRound();
            Debug.Log("Outcome: tie round! Resetting to next round");
            return;
        }

        //player 1 wins round
        if (player1WinsRound && !player2WinsRound)
        {
            rounds[currentRound - 1] = Winner.Player1;

            player1.score++;
            player1.streak++;
            player2.streak = 0;
            currentRound++;
            ResetRound();
            Debug.Log("Outcome: player 1 wins round! Resetting to next round");
            return;
        }

        //player 2 wins round
        if (player2WinsRound && !player1WinsRound)
        {
            rounds[currentRound - 1] = Winner.Player2;

            player2.score++;
            player2.streak++;
            player1.streak = 0;
            currentRound++;
            ResetRound();
            Debug.Log("Outcome: player 2 wins round! Resetting to next round");
            return;
        }

        if(roundOver)
        {
            rounds[currentRound - 1] = Winner.Draw;

            player1.streak = 0;
            player2.streak = 0;
            currentRound++;
            ResetRound();
            Debug.Log("Outcome: the round is a draw! Resetting round");
            return;
        }


    }

    void ResetRound()
    {
        player1.health = player1.maxHealth;
        player2.health = player2.maxHealth;
        UIManager.instance.UpdateHealthBars();

        currentTime = roundTime;

        player1.ResetPosition();
        player2.ResetPosition();
    }


    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;

        if (currentTime < 0)
            currentTime = 0;

        UIManager.instance.UpdateTimer();
    }
}

public struct Round
{
    Winner roundWinner;
}

public enum Winner : byte
{
    Undecided,
    Player1,
    Player2,
    Tie,
    Draw
}

public interface ICondition 
{ 



}

