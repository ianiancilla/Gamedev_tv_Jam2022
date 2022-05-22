using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Ability_Tiny : CharacterAbility
{
    //properties
    [SerializeField] [Range(0.1f, 1f)] float tinyScaleModifier = .3f;

    // cache
    CharacterController characterController;

    // variables
    float charaControllerDefaultHeight;


    void Start()
    {
        // cache
        characterController = GetComponent<CharacterController>();

        // initialise
        charaControllerDefaultHeight = characterController.height;
    }

    void FixedUpdate()
    {
    }

    private void HandleBeTiny(bool isTiny)
    {
        if (isTiny)
        {
            transform.localScale = Vector3.one * tinyScaleModifier;
            characterController.height = charaControllerDefaultHeight * tinyScaleModifier;
        }
        else
        {
            transform.localScale = Vector3.one;
            characterController.height = charaControllerDefaultHeight;
        }
    }
    public void BeTinyInput(InputAction.CallbackContext value)
    {
        bool isTiny = value.ReadValueAsButton();
        Debug.Log("Being Tiny: " + isTiny);
        HandleBeTiny(isTiny);
    }

}
