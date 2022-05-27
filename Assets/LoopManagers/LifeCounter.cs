using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LifeCounter : MonoBehaviour
{
    [SerializeField] int maxLives;
    [SerializeField] TextMeshProUGUI lifeCounter;


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
    }

    private void UpdateText()
    {
        lifeCounter.text = currentLives.ToString();
    }
}
