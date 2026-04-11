using System.Collections.Generic;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
    [Header("Soul Stuff")]
    [SerializeField] GameObject SoulStorage;

    [Header("Soul Display")]
    [SerializeField] GameObject[] SpawnUIPlacesParents = new GameObject[5];
    [SerializeField] LineRenderer[] DecayLineCovers = new LineRenderer[5];

    SoulData[] Souls = new SoulData[5];
    List<GameObject> SpawnedItems = new List<GameObject>();
    List<float> DecayLinesInitialXPoses = new List<float>();
    PlayerAttack refPlayerAttack;

    /// <summary>
    /// initializes variables
    /// </summary>
    private void Start()
    {
        refPlayerAttack = GetComponent<PlayerAttack>();
        foreach (LineRenderer decayline in DecayLineCovers)
        {
            DecayLinesInitialXPoses.Add(decayline.GetPosition(1).x);
        }
    }

    private void Update()
    {
        UpdateDecayLines();
    }

    /// <summary>
    /// Adds a soul to the player's list. This also shifts all other souls and reapplies all their effects.
    /// </summary>
    /// <param name="Soul">The gameobject of the soul to apply. NOTE: must have a SoulData component.</param>
    public void AddSoul(GameObject Soul)
    {
        // Check if the soul passed has a souldata component
        SoulData refData = Soul.GetComponent<SoulData>();
        // Do nothing if it does not have one
        if (refData == null)
        {
            UnityEngine.Debug.LogError("The soul passed does not have a SoulData component");
            return;
        }
        // Duplicate this data into another component on the souldata storage object
        SoulData newData = DuplicateSoulData(refData);
        // Remove the last soul from the array and storage object
        RemoveSoulData(4);

        // Move all souls up one slot
        for (int i = 4; i > 0; i--)
        {
            Souls[i] = Souls[i - 1];
        }
        // Add the new soul to the first slot
        Souls[0] = newData;
        // Apply them
        ApplyAllSouls();
        // Refresh the display
        RefreshSoulUI();
    }

    /// <summary>
    /// Apply the souls of the player to their stats
    /// </summary>
    public void ApplyAllSouls()
    {
        // Wipe the stats of the player
        ResetPlayerStats();
        Debug.Log("Applied all soul effects");

        // Loop through all souls and apply their effects
        for (int i = 0; i < Souls.Length; i++)
        {
            if (Souls[i] != null)
            {
                // Apply the soul's effects here, using refData
                ApplyTargetSoul(Souls[i].IsSoulInverted ? Souls[i].invertedSoulEffects : Souls[i].soulEffects);
            }
        }
    }

    void ApplyTargetSoul(List<SoulData.Effects> effects)
    {
        foreach (SoulData.Effects effect in effects)
        {
            // Apply the effect to the player here, using effect.statType and effect.statChange
            // Get the current soul effect
            SoulData.StatType statType = effect.statType;
            // Add conditions through a hideous switch statement
            switch (statType)
            {
                case SoulData.StatType.AttackDamage:
                    // Add the stat change to the player's attack damage
                    refPlayerAttack.currentAttackDamage += effect.statChange;
                    break;
                case SoulData.StatType.AttackRange:
                    // Add the stat change to the player's attack range
                    refPlayerAttack.currentAttackRange += effect.statChange;
                    break;
                case SoulData.StatType.AttackSpeed:
                    // Add the stat change to the player's attack speed
                    refPlayerAttack.currentAttackSpeed += effect.statChange;
                    break;
                case SoulData.StatType.MoveSpeed:
                    // Add the stat change to the player's move speed
                    Debug.Log("This doesn't exist yes :/");
                    break;
                case SoulData.StatType.PlayerDamageSelf:
                    refPlayerAttack.currentSelfAttack += effect.statChange;
                    break;
                case SoulData.StatType.HealPlayer:
                    Debug.Log("This doesn't exist yet :/");
                    break;
                case SoulData.StatType.DamageReduction:
                    Debug.Log("This doesn't exist yet :/");
                    break;
                case SoulData.StatType.EnemyHeal:
                    Debug.Log("This doesn't exist yet :/");
                    break;
                case SoulData.StatType.ExplodingDamade:
                    Debug.Log("This doesn't exist yet :/");
                    break;
            }
        }
    }

    /// <summary>
    /// Resets all stats of the player
    /// </summary>
    void ResetPlayerStats()
    {
        refPlayerAttack.ResetStats();
    }

    /// <summary>
    /// Saves a souldata to the souldata storage object, and returns a reference to the new object
    /// </summary>
    /// <param name="refSoulData">The SoulData to be duplicated</param>
    /// <returns>The new souldata</returns>
    SoulData DuplicateSoulData(SoulData refSoulData)
    {
        // Create a new soul data component on the soul storage object
        SoulData newSoulData = SoulStorage.AddComponent<SoulData>();
        // Copy all values from the reference soul data to the new one
        newSoulData.soulEffects = new List<SoulData.Effects>(refSoulData.soulEffects);
        newSoulData.invertedSoulEffects = new List<SoulData.Effects>(refSoulData.invertedSoulEffects);
        newSoulData.soulLifespan = refSoulData.soulLifespan;
        newSoulData.UIGameobject = refSoulData.UIGameobject;
        return newSoulData;
    }

    /// <summary>
    /// Removes a soul at a specified index
    /// </summary>
    /// <param name="index">The index of the array of the souldata component</param>
    void RemoveSoulData(int index)
    {
        if (Souls[index] != null)
        {
            RemoveSoulData(Souls[index]);
            Souls[index] = null;
        }
    }

    /// <summary>
    /// Removes a soul
    /// </summary>
    /// <param name="removeSoul"></param>
    void RemoveSoulData(SoulData removeSoul)
    {
        // Loop through the soul storage object and find the soul data that matches the one to be removed
        foreach (SoulData soulData in SoulStorage.GetComponents<SoulData>())
        {
            if (soulData == removeSoul)
            {
                // If a match is found, destroy the soul data component and return
                Destroy(soulData);
                return;
            }
        }
        // If no match is found, return null
        Debug.LogError("The soul data to be removed was not found in the soul storage object");
        return;
    }

    /// <summary>
    /// Refreshes the ui for the ui souls
    /// </summary>
    void RefreshSoulUI()
    {
        // Spawn each object
        SpawnedItems.Clear();
        for (int i = 0; i < SpawnUIPlacesParents.Length; i++)
        {
            if (Souls[i] != null)
                SpawnedItems.Add(Instantiate(Souls[i].UIGameobject, SpawnUIPlacesParents[i].transform));
        }

        // Spawn the lines for decay amount
        UpdateDecayLines();
    }

    /// <summary>
    /// Updates the lines for each soul until it decays
    /// </summary>
    void UpdateDecayLines()
    {
        for (int i = 0; i < SpawnUIPlacesParents.Length; i++)
        {
            if (Souls[i] != null)
            {
                // Activate the object if it is not enabled
                if (!DecayLineCovers[i].transform.parent.gameObject.activeSelf)
                    DecayLineCovers[i].transform.parent.gameObject.SetActive(true);
                // Get the time to decay for the line
                float timeToDecay = Souls[i].SoulLifeSpanLeft;
                // Set the line renderer COVER to show the time to decay
                float xPos = Mathf.Lerp(0, DecayLinesInitialXPoses[i], timeToDecay / Souls[i].soulLifespan);
                DecayLineCovers[i].SetPosition(1, new Vector3(xPos, DecayLineCovers[i].GetPosition(1).y, DecayLineCovers[i].GetPosition(1).z));
            }
            else
            {
                // Otherwise, deactavate the gameobject
                DecayLineCovers[i].transform.parent.gameObject.SetActive(false);
            }
        }

    }
}
