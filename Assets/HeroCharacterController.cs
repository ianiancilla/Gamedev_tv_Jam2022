using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class HeroCharacterController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Velocity on y axis due to gravity")]
    [SerializeField] float forwardSpeed = 10f;
    [SerializeField] float gravity = 10f;
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float dashSpeedMultiplier = 2f;
    [SerializeField] float dashTimeDuration = 0.3f;


    [Header("Collisions and Layers")]

    // member variables
    public Vector3 motion;
    private bool jumping = false;
    private bool dashing = false;


    // cache
    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveForward(ref motion);

        HandleJump(ref motion);

        ApplyGravity(ref motion);

        characterController.Move(motion * Time.deltaTime);
    }

    private void HandleJump(ref Vector3 motion)
    {
        if (!jumping || !characterController.isGrounded) { return; }
        motion.y = Mathf.Sqrt(jumpHeight * gravity);
        jumping = false;
    }

    private IEnumerator HandleDash()
    {
        dashing = true;
        yield return new WaitForSeconds(dashTimeDuration);
        dashing = false;
    }


    private void ApplyGravity(ref Vector3 motion)
    {
        // no gravity while dashing!
        if (dashing)
        {
            motion.y = 0;
        }

        float minGravityPush = -characterController.minMoveDistance;

        // Always keep pushing down to maintain contact
        if (characterController.isGrounded && motion.y > minGravityPush)
        {
            motion.y += minGravityPush;
        }
        // Gravity push if not grounded
        else if (!characterController.isGrounded)
        {
            motion.y -= gravity *Time.deltaTime;

            motion.y = Mathf.Clamp(motion.y, -gravity, gravity);
        }
    }

    private void MoveForward(ref Vector3 motion)
    {
        // move at fwd speed, multiplied if dashing
        motion.z = dashing ? (forwardSpeed * dashSpeedMultiplier) : forwardSpeed;
    }

    // input handling
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Jump Input");
            jumping = true;
        }
    }

    public void DashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Dash Input");
            if (!dashing)
            {
                StartCoroutine(HandleDash());
            }
        }
    }



}
