using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HeroCharacterController))]
public class Ability_HoldStill : MonoBehaviour, ICharacterAbility
{
    public string AbilityName { get; } = "HoldStill";
    public abiType AbilityType { get; } = abiType.HoldStill;


    [Header("Feedback")]
    [SerializeField] public AudioSource BGM;


    // cache
    HeroCharacterController heroController;

    void Start()
    {
        // cache
        heroController = GetComponent<HeroCharacterController>();
    }

    private void HandleHoldStill(bool holdingStill)
    {
        if (!this.enabled) { return; }
        heroController.HoldingStill = holdingStill;
    }

    public void HoldStillInput(InputAction.CallbackContext value)
    {
        bool inputValue = value.ReadValueAsButton();
        Debug.Log("Dash Input: " + inputValue);

        HandleHoldStill(inputValue);
    }

}
