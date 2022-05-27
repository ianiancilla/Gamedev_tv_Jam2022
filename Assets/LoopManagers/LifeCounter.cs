using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public class LifeCounter : MonoBehaviour
{
    [SerializeField] int maxLives;
    [SerializeField] TextMeshProUGUI lifeCounter;

    public UnityEvent GameOver;

    public int currentLives { get; private set; }

    private void Start()
    {
        currentLives = maxLives;
        UpdateText();
    }

    public void OnDeath()
    {
        currentLives--;
        UpdateText();

        if (currentLives <= 0) { GameOver.Invoke(); }
    }

    private void UpdateText()
    {
        lifeCounter.text = currentLives.ToString();
    }
}
