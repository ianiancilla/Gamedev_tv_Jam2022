using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HeroCharacterController))]
public class Ability_LaneSwap : CharacterAbility
{
    [SerializeField] Lane[] AvailableLanes;
    [SerializeField] float laneSwappingCoolDown = 0.2f;
    [SerializeField] private int startingLane = 1;
    [SerializeField] private float laneSwappingSpeed = 1f;

    // cache
    HeroCharacterController heroController;

    // variables
    private float laneSwapInput = 0f;
    private bool laneSwapCooldown = false;
    private int currentLane;


    // Start is called before the first frame update
    void Start()
    {
        heroController = GetComponent<HeroCharacterController>();

        currentLane = startingLane;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleLaneSwap();
        MoveToTargetX();
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
        else if (laneSwapInput > 0 && currentLane < (AvailableLanes.Length - 1))
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

    private void MoveToTargetX()
    {
        float targetX = AvailableLanes[currentLane].xPos;
        float xOffset = targetX - transform.position.x;


        if (Mathf.Abs(xOffset) <= Mathf.Epsilon) { return; }

        float motionX = laneSwappingSpeed * xOffset;

        heroController.ChangeMotion(new Vector3(motionX,
                                                heroController.motion.y,
                                                heroController.motion.z));
    }

    public void LaneSwapInput(InputAction.CallbackContext value)
    {
        float val = value.ReadValue<float>();
        Debug.Log("L/R input: " + val);
        laneSwapInput = val;
    }


}
