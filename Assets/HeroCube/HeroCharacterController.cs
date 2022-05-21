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
    [SerializeField] float dashTimeDuration = 0.3f;
    [Space]


    [Header("Tiny State")]
    [SerializeField] [Range (0.1f, 1f)] float tinyScale = .3f;
    [Space]

    [Header("Shooting")]
    [SerializeField] ProjectileShooter projectileShooter;


    // member variables
    public Vector3 motion;
    float charaControllerDefaultHeight;

    // states
    private bool dashing = false;

    private bool holdingStill = false;  // if I dash while holding still, I will dash


    // cache
    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        // cache
        characterController = GetComponent<CharacterController>();

        // initialise
        charaControllerDefaultHeight = characterController.height;
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

    public float GetGravity() { return gravity; }
    public float GetForwardSpeed() { return forwardSpeed; }

}

[Serializable]
public class Lane
{
    public float xPos = 0;
}