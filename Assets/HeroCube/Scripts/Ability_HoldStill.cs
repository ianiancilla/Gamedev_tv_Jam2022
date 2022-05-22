using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HeroCharacterController))]
public class Ability_HoldStill : MonoBehaviour, ICharacterAbility
{
    public string AbilityName { get; } = "HoldStill";

    // cache
    HeroCharacterController heroController;

    void Start()
    {
        // cache
        heroController = GetComponent<HeroCharacterController>();
    }

    private void HandleHoldStill(bool holdingStill)
    {
        if (!this.isActiveAndEnabled) { return; }
        heroController.HoldingStill = holdingStill;
    }

    public void HoldStillInput(InputAction.CallbackContext value)
    {
        bool inputValue = value.ReadValueAsButton();
        Debug.Log("Dash Input: " + inputValue);

        HandleHoldStill(inputValue);
    }

}
