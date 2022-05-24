using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HeroCharacterController))]
public class Ability_Jump : MonoBehaviour, ICharacterAbility
{
    //properties
    public string AbilityName { get; } = "Jump";
    public abiType AbilityType { get; } = abiType.Jump;


    [Header("Mechanics")]
    [SerializeField] float jumpHeight = 10f;

    [Header("Feedback")]
    [SerializeField] public AudioSource BGM;



    // cache
    HeroCharacterController heroController;
    CharacterController characterController;

    // variables
    private bool jumpInput = false;


    void Start()
    {
        // cache
        heroController = GetComponent<HeroCharacterController>();
        characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        HandleJump();
    }

    private void HandleJump()
    {
        if (!jumpInput || !characterController.isGrounded) { return; }

        float motionY = Mathf.Sqrt(jumpHeight * heroController.GetGravity());

        heroController.ChangeMotion(new Vector3(heroController.GetMotion().x,
                                                motionY,
                                                heroController.GetMotion().z));

        jumpInput = false;
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Jump Input");
            jumpInput = true;
        }
    }
}
