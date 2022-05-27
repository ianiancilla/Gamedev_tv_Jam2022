using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(HeroCharacterController))]
public class Ability_Shoot : CharaAbilityBase, ICharacterAbility
{
    // properties
    public string AbilityName { get; } = "Shoot";
    public abiType AbilityType { get; } = abiType.Shoot;

    [Header("Mechanics")]
    [SerializeField] ProjectileShooter projectileShooter;

    private void OnDisable()
    {
        base.OnDisable();
        SetShootingActiveState(false);
    }

    private void HandleShooting(bool isShooting)
    {
        if (!this.enabled) { return; }
        SetShootingActiveState(isShooting);
    }

    private void SetShootingActiveState(bool isShooting)
    {
        projectileShooter.EnableShooting(isShooting);
    }

    public void ShootingInput(InputAction.CallbackContext value)
    {
        bool isShooting = value.ReadValueAsButton();
        Debug.Log("Shooting: " + isShooting);
        HandleShooting(isShooting);
    }
}
