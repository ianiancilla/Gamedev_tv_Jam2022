using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class HeroCharacterController : MonoBehaviour
{
    [SerializeField] float forwardSpeed = 3f;
    [SerializeField] float gravity = 25f;
    [Space]
    [SerializeField] float dashSpeedMultiplier = 2f;


    // member variables
    public Vector3 motion;

    // states
    public bool Dashing { get; set; } = false;
    public bool HoldingStill { get; set; } = false;  // if I dash while holding still, I will dash


    // cache
    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        // cache
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveForward(ref motion);

        ApplyGravity(ref motion);

        characterController.Move(motion * Time.deltaTime);
    }

    public void ChangeMotion(Vector3 newMotion)
    {
        motion = newMotion;
    }



    private void ApplyGravity(ref Vector3 motion)
    {
        // no gravity while dashing!
        if (Dashing)
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
        if (HoldingStill && !Dashing)
        {
            motion.z = 0;
            return;
        }

        motion.z = Dashing ? (forwardSpeed * dashSpeedMultiplier) : forwardSpeed;
    }

    // input handling

    public float GetGravity() { return gravity; }
    public float GetForwardSpeed() { return forwardSpeed; }

}

