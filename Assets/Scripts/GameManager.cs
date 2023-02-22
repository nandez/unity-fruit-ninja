using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI scoreText;

    private int score = 0;

    private void Start()
    {
        NewGame();
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = $"SCORE: {score}";
    }

    public void NewGame()
    {
        score = 0;
        scoreText.text = $"SCORE: {score}";
    }
}
