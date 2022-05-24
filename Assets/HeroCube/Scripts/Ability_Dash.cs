using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HeroCharacterController))]
public class Ability_Dash : MonoBehaviour, ICharacterAbility
{
    // properties
    public string AbilityName { get; } = "Dash";

    [Header("Mechanics")]
    [SerializeField] float dashTimeDuration = 0.3f;

    [Header("Feedback")]
    [SerializeField] public AudioSource BGM;

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

            if (!this.isActiveAndEnabled) { return; }

            if (!heroController.Dashing)
            {
                StartCoroutine(HandleDash());
            }
        }
    }
}
