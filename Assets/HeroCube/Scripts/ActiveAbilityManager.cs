using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbilityManager : MonoBehaviour
{
    // properties
    [SerializeField] List<abiType> startingAbilities;

    // variables
    private List<ICharacterAbility> possibleAbilities = new List<ICharacterAbility>();
    private List<ICharacterAbility> activeAbilities = new List<ICharacterAbility>();

    // cache



    // Start is called before the first frame update
    void Start()
    {
        // intialise
        Initialise();

    }

    private void Initialise()
    {
        FindPossibleAbilities(ref possibleAbilities);

        foreach (var ability in possibleAbilities)
        {
            (ability as MonoBehaviour).enabled = false;
        }

        foreach (abiType type in startingAbilities)
        {
            SetAbilityActive(type, true);
        }
    }

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

    public void SetAbilityActive(abiType abiType, bool newState)
    {
        foreach (ICharacterAbility ability in possibleAbilities)
        {
            if (ability.AbilityType == abiType)
            {
                MonoBehaviour abiAsMonobehaviour = ability as MonoBehaviour;
                abiAsMonobehaviour.enabled = newState;
                break;
            }
            Debug.Log("Could not deactivate " + abiType);
        }
        FindActiveAbilities(ref activeAbilities);
    }



    //private void InitialiseAbilities()
    //{
    //    abilityComponents = GetComponents<ICharacterAbility>() as MonoBehaviour[];
    //    Debug.Log(GetComponents<ICharacterAbility>() as MonoBehaviour[]);
    //    Debug.Log(abilityComponents.Length + "cmponenti trovati");

    //    existingAbilities = new abiType[abilityComponents.Length];

    //    // deactivate all abilities and record possible ones
    //    for (int i = 0; i < abilityComponents.Length; i++)
    //    {
    //        existingAbilities[i] = (abilityComponents[i] as ICharacterAbility).AbilityType;
    //        abilityComponents[i].enabled = false;
    //    }

    //    //// reactivate only abilities set to be enabled
    //    //foreach (abiType type in activeAbilities)
    //    //{
    //    //    SetAbilityActive(type, true);
    //    //}
    //}

    //private void SetAbilityActive(abiType abiType, bool activeState)
    //{
    //    foreach(MonoBehaviour abiComponent in abilityComponents)
    //    {
    //        if ((abiComponent as ICharacterAbility).AbilityType == abiType)
    //        {
    //            abiComponent.enabled = activeState;
    //        }
    //    }
    //}
}

public enum abiType
{
    LaneSwap,
    Jump,
    Dash,
    Tiny,
    HoldStill,
    Shoot
}
