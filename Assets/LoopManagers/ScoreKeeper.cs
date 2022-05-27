using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] float scorePerSecond = 10f;

    [Tooltip("Every time this many seconds pass without changing " +
             "life, the life multiplier is applied again")]
    [SerializeField] float streakDuration = 15f;
    [SerializeField] float streakMultiplierAddition = 0.5f;

    [SerializeField] HeroCharacterController characterController;
    [SerializeField] TextMeshProUGUI scoreText;

    public float Score { get; private set; }
    private float currentMultiplier = 1f;
    private float currentStreakTime = 0f;

    private void Update()
    {
        // no score while stopping or dying
        if (characterController.HoldingStill || characterController.Paused) { return; }

        CalculateCurrentMultiplier();

        Score += scorePerSecond * Time.deltaTime * currentMultiplier;

        int scoreInt = (int)(Score);

        scoreText.text = scoreInt.ToString();
    }

    private void CalculateCurrentMultiplier()
    {
        currentStreakTime += Time.deltaTime;
        if (currentStreakTime > streakDuration)
        {
            currentMultiplier += streakMultiplierAddition;
            currentStreakTime = 0f;
        }
    }

    public void ResetMultiplier()
    {
        currentMultiplier = 1f;
    }
}
