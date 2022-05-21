using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HeroCharacterController))]
public class Ability_Jump : CharacterAbility
{
    [SerializeField] float jumpHeight = 10f;

    // cache
    HeroCharacterController heroController;
    CharacterController characterController;

    // states
    private bool jumpInput = false;


    // Start is called before the first frame update
    void Start()
    {
        heroController = GetComponent<HeroCharacterController>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleJump();
    }

    private void HandleJump()
    {
        if (!jumpInput || !characterController.isGrounded) { return; }

        float motionY = Mathf.Sqrt(jumpHeight * heroController.GetGravity());

        heroController.ChangeMotion(new Vector3(heroController.motion.x,
                                                motionY,
                                                heroController.motion.z));

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
