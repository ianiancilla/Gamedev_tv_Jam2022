using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HeroCharacterController))]
public class Ability_Dash : CharacterAbility
{
    // properties
    [SerializeField] float dashTimeDuration = 0.3f;

    // cache
    HeroCharacterController heroController;

    void Start()
    {
        // cache
        heroController = GetComponent<HeroCharacterController>();
    }

    private IEnumerator HandleDash()
    {
        heroController.Dashing = true;
        yield return new WaitForSeconds(dashTimeDuration);
        heroController.Dashing = false;
    }
    public void DashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Dash Input");
            if (!heroController.Dashing)
            {
                StartCoroutine(HandleDash());
            }
        }
    }
}
