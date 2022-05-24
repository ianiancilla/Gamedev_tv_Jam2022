using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbilityManager : MonoBehaviour
{
    // properties
    [SerializeField] List<abiType> startingAbilities;
    [SerializeField] int maxAbilityNumber = 2;

    // variables
    private List<ICharacterAbility> possibleAbilities = new List<ICharacterAbility>();
    private List<ICharacterAbility> activeAbilities = new List<ICharacterAbility>();

    // cache



    // Start is called before the first frame update
    void Start()
    {
        // intialise
        Initialise();

        StartCoroutine(testSwapping());
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
        RefreshActiveAbilitiesList();
    }

    // activating and checking abilities
    private void FindPossibleAbilities(ref List<ICharacterAbility> possibleAbilities)
    {
        GetComponents(possibleAbilities);
        Debug.Log(possibleAbilities.Count + " possible abilities on character");
    }

    private void RefreshActiveAbilitiesList()
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

    private void SetAbilityActive(abiType abiType, bool newState)
    {
        foreach (ICharacterAbility ability in possibleAbilities)
        {
            if (ability.AbilityType == abiType)
            {
                MonoBehaviour abiAsMonobehaviour = ability as MonoBehaviour;
                abiAsMonobehaviour.enabled = newState;
                break;
            }
        }
    }

    public void ChangeAbility()
    {
        // find and activate new ability
        SetAbilityActive(RandomiseNewAbility(), true);

        // remove an old ability if limit was reached
        if (activeAbilities.Count >= maxAbilityNumber)
        {
            int randomIndex = Random.Range(0, activeAbilities.Count);
            abiType type = activeAbilities[randomIndex].AbilityType;
            SetAbilityActive(type, false);
        }
        RefreshActiveAbilitiesList();
    }

    private abiType RandomiseNewAbility()
    {
        var abilitiesToChooseFrom = new List<ICharacterAbility>(possibleAbilities);
        foreach (ICharacterAbility ability in activeAbilities)
        {
            abilitiesToChooseFrom.Remove(ability);
        }
        
        int randomIndex = Random.Range(0, abilitiesToChooseFrom.Count);
        ICharacterAbility nextAbility = abilitiesToChooseFrom[randomIndex];

        return nextAbility.AbilityType;
    }

    // test ability swapper
    IEnumerator testSwapping()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            ChangeAbility();
        }
    }
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
