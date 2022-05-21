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
    [Space]
    [SerializeField] float jumpHeight = 10f;
    [Space]
    [SerializeField] float dashSpeedMultiplier = 2f;
    [SerializeField] float dashTimeDuration = 0.3f;
    [Space]

    [Header("Lane swapping")]
    [SerializeField] Lane[] AvailableLanes;
    [SerializeField] float laneSwappingCoolDown = 0.2f;
    [SerializeField] private int startingLane = 1;
    [SerializeField] private float laneSwappingSpeed = 1f;
    [Space]

    [Header("Tiny State")]
    [SerializeField] [Range (0.1f, 1f)] float tinyScale = .3f;
    [Space]

    [Header("Shooting")]
    [SerializeField] ProjectileShooter projectileShooter;


    // member variables
    Vector3 motion;
    float charaControllerDefaultHeight;

    // states
    private bool jumping = false;

    private bool dashing = false;

    private bool holdingStill = false;  // if I dash while holding still, I will dash

    private float laneSwapInput = 0f;
    private bool laneSwapCooldown = false;
    public int currentLane;

    // cache
    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        // cache
        characterController = GetComponent<CharacterController>();

        // initialise
        currentLane = startingLane;
        charaControllerDefaultHeight = characterController.height;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveForward(ref motion);

        HandleJump(ref motion);

        HandleLaneSwap();

        MoveToTargetX(ref motion);

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

    private void HandleBeTiny(bool isTiny)
    {
        if (isTiny)
        {
            transform.localScale = Vector3.one * tinyScale;
            characterController.height = charaControllerDefaultHeight * tinyScale;
        }
        else
        {
            transform.localScale = Vector3.one;
            characterController.height = charaControllerDefaultHeight;
        }
    }

    private void HandleShooting(bool isShooting)
    {
        projectileShooter.EnableShooting(isShooting);
    }
    private void HandleLaneSwap()
    {
        // if there is no moving input, do not start a new swap
        if (laneSwapInput == 0 || laneSwapCooldown) { return; }

        // if input goes left and you are not in the leftmost alley
        if (laneSwapInput < 0 && currentLane > 0)
        {
            //move left
            currentLane--;
        }
        else if (laneSwapInput > 0 && currentLane < (AvailableLanes.Length -1))
        {
            // move right
            currentLane++;
        }
        StartCoroutine(LaneSwapCooldown());
    }

    private IEnumerator LaneSwapCooldown()
    {
        laneSwapCooldown = true;
        yield return new WaitForSeconds(laneSwappingCoolDown);
        laneSwapCooldown = false;
    }

    private void MoveToTargetX(ref Vector3 motion)
    {
        float targetX = AvailableLanes[currentLane].xPos;
        float xOffset = targetX - transform.position.x;


        if (Mathf.Abs(xOffset) <= Mathf.Epsilon) { return; }

        motion.x = laneSwappingSpeed * xOffset;
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
        if (holdingStill && !dashing)
        {
            motion.z = 0;
            return;
        }

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

    public void HoldStillInput(InputAction.CallbackContext value)
    {
        bool val = value.ReadValueAsButton();
        Debug.Log("Dash Input: " + val);
        holdingStill = val;
    }

    public void LaneSwapInput(InputAction.CallbackContext value)
    {
        float val = value.ReadValue<float>();
        Debug.Log("L/R input: " + val);
        laneSwapInput = val;
    }

    public void BeTinyInput(InputAction.CallbackContext value)
    {
        bool isTiny = value.ReadValueAsButton();
        Debug.Log("Being Tiny: " + isTiny);
        HandleBeTiny(isTiny);
    }
    public void ShootingInput(InputAction.CallbackContext value)
    {
        bool isShooting = value.ReadValueAsButton();
        Debug.Log("Shooting: " + isShooting);
        HandleShooting(isShooting);
    }

}

[Serializable]
public class Lane
{
    public float xPos = 0;
}