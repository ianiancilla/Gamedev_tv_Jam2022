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
    private Vector3 motion;

    private List<ICharacterAbility> activeAbilities = new List<ICharacterAbility>();
    public List<ICharacterAbility> possibleAbilities = new List<ICharacterAbility>();

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

        // intialise
        FindPossibleAbilities(ref possibleAbilities);
        FindActiveAbilities(ref activeAbilities);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // these calculate necessary movement, while ability scripts
        // do the same.
        // These are all stored in the Vector3 motion, which is then
        // applied via Unity characontroller method
        MoveForward(ref motion);
        ApplyGravity(ref motion);

        characterController.Move(motion * Time.deltaTime);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(hit.gameObject.name);
    }

    private void ApplyGravity(ref Vector3 motion)
    {
        // no gravity while dashing
        if (Dashing)
        {
            motion.y = 0;
        }

        float minGravityPush = -characterController.minMoveDistance;

        // Always keep pushing down to maintain contact, needed by the
        // Unity characontroller to see itself as grounded
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

    // communicate movement speed for this frame, multiplied if dashing
    private void MoveForward(ref Vector3 motion)
    {
        if (HoldingStill && !Dashing)
        {
            motion.z = 0;
        }
        else
        {
            motion.z = Dashing ? (forwardSpeed * dashSpeedMultiplier) : forwardSpeed;
        }
    }

    public void ChangeMotion(Vector3 newMotion)
    {
        motion = newMotion;
    }


    // getters
    public float GetGravity() { return gravity; }
    public float GetForwardSpeed() { return forwardSpeed; }
    public Vector3 GetMotion() { return motion; }

    // activating and checking abilities
    public void FindPossibleAbilities(ref List<ICharacterAbility> possibleAbilities)
    {
        GetComponents(possibleAbilities);
        Debug.Log(possibleAbilities.Count + " possible abilities on character");
    }

    public void FindActiveAbilities(ref List<ICharacterAbility> activeAbilities)
    {
        activeAbilities.Clear();

        foreach (ICharacterAbility ability in possibleAbilities)
        {
            MonoBehaviour abiAsMonobehaviour = ability as MonoBehaviour;
            if (abiAsMonobehaviour.isActiveAndEnabled) { activeAbilities.Add(ability); }
        }

        // logging
        string activeAbilitiesNamesForLog = "";
        foreach (ICharacterAbility ability in activeAbilities)
        {
            activeAbilitiesNamesForLog += " " + ability.AbilityName + ",";
        }
        activeAbilitiesNamesForLog = activeAbilitiesNamesForLog.TrimEnd(',') + ".";
        Debug.Log(activeAbilities.Count + " abilities are activated:" + activeAbilitiesNamesForLog);

    }
}

