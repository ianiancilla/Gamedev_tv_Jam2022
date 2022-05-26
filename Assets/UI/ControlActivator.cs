using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlActivator : MonoBehaviour
{
    [SerializeField] Color deactivatedColor;
    public Color activatedColor = Color.magenta;

    [SerializeField] TMP_Text[] textFields;
    
    [SerializeField] Image controlFrame;



    public void SetControlActivation(bool newState)
    {
        foreach (TMP_Text tMP_Text in textFields)
        {
            tMP_Text.color = newState ? activatedColor : deactivatedColor;
            controlFrame.color = tMP_Text.color;
        }
    }
}
