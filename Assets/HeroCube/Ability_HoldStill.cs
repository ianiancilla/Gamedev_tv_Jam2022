using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HeroCharacterController))]
public class Ability_HoldStill : CharacterAbility
{
    // cache
    HeroCharacterController heroController;

    void Start()
    {
        // cache
        heroController = GetComponent<HeroCharacterController>();
    }

    public void HoldStillInput(InputAction.CallbackContext value)
    {
        bool inputValue = value.ReadValueAsButton();
        Debug.Log("Dash Input: " + inputValue);
        heroController.HoldingStill = inputValue;
    }

}
