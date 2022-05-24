using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAbilityBase : MonoBehaviour
{
    [Header ("Feedback")]
    [SerializeField] public AudioSource BGM;
    [SerializeField] float volume = 0.2f;

    private void OnEnable()
    {
        BGM.mute = false;
    }

    private void OnDisable()
    {
        BGM.mute = true;
    }
}
