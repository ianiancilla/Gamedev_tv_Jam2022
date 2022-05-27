using System;
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
    [SerializeField] float hueShift;

    private void OnEnable()
    {
        BGM.mute = false;
        foreach (ControlActivator ui in UI_Control)
        {
            ui?.SetControlActivation(true);
        }
    }

    public void OnDisable()
    {
        BGM.mute = true;
        foreach (ControlActivator ui in UI_Control)
        {
            ui?.SetControlActivation(false);
        }
    }


    public float GetHueShift() { return hueShift; }
}
