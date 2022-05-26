using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAbilityBase : MonoBehaviour
{
    [Header ("Feedback")]
    [SerializeField] public AudioSource BGM;
    [SerializeField] float volume = 0.2f;

    [Header("UI")]
    [SerializeField] ControlActivator[] UI_Control;
    [SerializeField] Color activatedColor = Color.magenta;

    private void OnEnable()
    {
        BGM.mute = false;
        foreach (ControlActivator ui in UI_Control)
        {
            ui.activatedColor = activatedColor;
            ui?.SetControlActivation(true);
        }
    }

    private void OnDisable()
    {
        BGM.mute = true;
        foreach (ControlActivator ui in UI_Control)
        {
            ui?.SetControlActivation(false);
        }
    }

    public Color GetColor() { return activatedColor; }
}
