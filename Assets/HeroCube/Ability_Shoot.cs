using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(HeroCharacterController))]
public class Ability_Shoot : CharacterAbility
{
    // properties
    [SerializeField] ProjectileShooter projectileShooter;

    private void HandleShooting(bool isShooting)
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
