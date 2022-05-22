using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(HeroCharacterController))]
public class Ability_Shoot : MonoBehaviour, ICharacterAbility
{
    // properties
    public string AbilityName { get; } = "Shoot";
    [SerializeField] public AudioSource BGM;
    [SerializeField] ProjectileShooter projectileShooter;

    //private void Start()
    //{
    //    // just need it so the Monobehaviour can have an activation toggle T_T
    //}

    private void HandleShooting(bool isShooting)
    {
        if (!this.isActiveAndEnabled) { return; }
        projectileShooter.EnableShooting(isShooting);
    }

    public void ShootingInput(InputAction.CallbackContext value)
    {
        bool isShooting = value.ReadValueAsButton();
        Debug.Log("Shooting: " + isShooting);
        HandleShooting(isShooting);
    }
}
