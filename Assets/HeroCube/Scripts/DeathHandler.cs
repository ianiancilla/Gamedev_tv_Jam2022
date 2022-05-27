using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(HeroCharacterController))]
[RequireComponent(typeof(ActiveAbilityManager))]
public class DeathHandler : MonoBehaviour
{
    [SerializeField] float deathPlaneY = -2f;
    [SerializeField] LayerMask killerLayers;
    [SerializeField] float deathPauseTime = 0.5f;

    // cache
    HeroCharacterController characterController;
    ActiveAbilityManager activeAbilityManager;

    // variables
    bool dying = false;

    public UnityEvent Death;

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
        if (dying) { return; }

        Debug.Log("YOU DIED");

        Death.Invoke();
        StartCoroutine(PauseAndResetPosOnDeath());

        activeAbilityManager.ChangeAbility();
    }

    private void CheckForFall()
    {
        if (transform.position.y < deathPlaneY)
        {
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

    IEnumerator PauseAndResetPosOnDeath()
    {
        characterController.Paused = true;
        dying = true;

        yield return new WaitForEndOfFrame();
        characterController.ResetPositionOnDeath();

        yield return new WaitForSeconds(deathPauseTime);

        characterController.Paused = false;
        dying=false;
    }
}
