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
    [SerializeField] float gravity = -10f;

    [Header("Collisions and Layers")]
    [SerializeField] LayerMask groundLayers;

    // member variables
    public Vector3 motion;
    private bool isGrounded;

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
        // reset motion
        motion = Vector3.zero;
        GroundCheck();
        Debug.Log(isGrounded);

        // gravity
        if (!isGrounded) { ApplyGravity(ref motion); }

        // forward move
        MoveForward(ref motion);

        // move
        characterController.Move(motion);
    }

    private void GroundCheck()
    {
        isGrounded = characterController.isGrounded;
            //Physics.CheckSphere(transform.position, 0.01f, groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void ApplyGravity(ref Vector3 motion)
    {
        motion.y += gravity * Time.deltaTime;
    }

    private void MoveForward(ref Vector3 motion)
    {
        motion.z += forwardSpeed * Time.deltaTime;
    }

    // input handling
    public void Jump(InputAction.CallbackContext context)
    {
        // can only jump if on track
        if (!isGrounded) { return; }


    }

}
