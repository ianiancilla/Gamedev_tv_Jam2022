using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HeroCharacterController))]
[RequireComponent(typeof(ActiveAbilityManager))]
public class DeathHandler : MonoBehaviour
{
    [SerializeField] float deathPlaneY = -2f;
    [SerializeField] LayerMask killerLayers;

    // cache
    HeroCharacterController characterController;
    ActiveAbilityManager activeAbilityManager;

    // Start is called before the first frame update
    void Start()
    {
        activeAbilityManager = GetComponent<ActiveAbilityManager>();
        characterController = GetComponent<HeroCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForFall();
    }

    public void Die()
    {
        Debug.Log("YOU DIED");
        // TODO pause and reset position to int closer to death point
        activeAbilityManager.ChangeAbility();
    }

    private void CheckForFall()
    {
        if (transform.position.y < deathPlaneY)
        {
            Debug.Log("you fell");
            characterController.PickUpFromFall();
            Die();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (killerLayers == (killerLayers | (1 << hit.gameObject.layer)))
        {
            hit.gameObject.SetActive(false);
            Die();
        }
    }
}
