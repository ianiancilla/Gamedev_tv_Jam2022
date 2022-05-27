using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameOverScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText.text = StaticScore.Score.ToString();
    }

    public void ResetScore()
    {
        StaticScore.Score = 0;
    }
}
