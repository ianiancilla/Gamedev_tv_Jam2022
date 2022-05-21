using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HeroCharacterController))]
public class CharacterAbility : MonoBehaviour
{
    HeroCharacterController heroCharacterController;

    private void Start()
    {
        heroCharacterController = GetComponent<HeroCharacterController>();
    }
}
