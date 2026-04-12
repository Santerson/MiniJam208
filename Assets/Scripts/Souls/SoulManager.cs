using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
    [Header("Soul Stuff")]
    [SerializeField] GameObject SoulStorage;

    [Header("Soul Display")]
    [SerializeField] GameObject[] SpawnUIPlacesParents = new GameObject[5];
    [SerializeField] LineRenderer[] DecayLineCovers = new LineRenderer[5];
    [SerializeField] TextMeshProUGUI[] StatTextDisplays = new TextMeshProUGUI[5];

    [Header("Soul Effect Displays")]
    [SerializeField] string[] StatTypeDisplayNames = new string[System.Enum.GetValues(typeof(SoulData.StatType)).Length];

    [Header("Colors")]
    [SerializeField] Color PositiveSoulColor = Color.white;
    [SerializeField] Color InvertedSoulColor = Color.black;
    [SerializeField] Color NoSoulBackgroundColor = Color.yellow;
    [SerializeField] Color PositiveSoulBackgroundColor = Color.yellow;
    [SerializeField] Color InvertedSoulBackgroundColor = Color.black;

    // Currently active souls, from oldest to newest. soul 0 is the newest, soul 4 is the oldest
    SoulData[] Souls = new SoulData[5];
    // A list of the currently spawned ui items for the souls, so they can be easily destroyed when refreshing the ui
    List<GameObject> SpawnedItems = new List<GameObject>();
    // A list of the base spriterenderers of every soul
    List<SpriteRenderer> SpawnedSoulSprites = new List<SpriteRenderer>();
    // A list of the initial x positions of the decay line covers, used to calculate how much to show of the line when refreshing the ui
    List<float> DecayLinesInitialXPoses = new List<float>();

    // References to player scripts to apply soul effects to
    PlayerAttack refPlayerAttack;
    PlayerMovemenmt refPlayerMovement;
    PlayerHealth refPlayerHealth;
    GameManager refGameManager;

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
        refPlayerMovement = GetComponent<PlayerMovemenmt>();
        refPlayerHealth = GetComponent<PlayerHealth>();
        refGameManager = FindFirstObjectByType<GameManager>();
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
            Debug.LogError("The soul passed does not have a SoulData component");
            return;
        }
        // Duplicate this data into another component on the souldata storage object
        SoulData newData = DuplicateSoulData(refData);
        // Save onto the gamemanager
        refGameManager.AddPickedUpSoul();
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
        // Refresh the ui
        RefreshSoulUI();
    }

    void ApplyTargetSoul(List<SoulData.Effects> effects)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            SoulData.Effects effect = effects[i];
            // Apply the effect to the player here, using effect.statType and effect.statChange
            // Get the current soul effect
            SoulData.StatType statType = effect.statType;
            // Add conditions through a hideous switch statement
            switch (statType)
            {
                // Depending on the stat type, add the stat change to the corresponding player stat

                // Atk Dmg
                case SoulData.StatType.AttackDamage:
                    // Add the stat change to the player's attack damage
                    refPlayerAttack.currentAttackDamage += effect.statChange;
                    break;
                // Atk Range
                case SoulData.StatType.AttackRange:
                    // Add the stat change to the player's attack range
                    refPlayerAttack.currentAttackRange += effect.statChange;
                    break;
                // Asp
                case SoulData.StatType.AttackSpeed:
                    // Add the stat change to the player's attack speed
                    refPlayerAttack.currentAttackSpeed += effect.statChange;
                    break;
                // Player Move Speed
                case SoulData.StatType.MoveSpeed:
                    // Add the stat change to the player's move speed
                    refPlayerMovement.ChangeMoveSpeed(effect.statChange);
                    break;
                // If the soul damages the player on attack
                case SoulData.StatType.PlayerDamageSelf:
                    refPlayerAttack.currentSelfAttack += effect.statChange;
                    break;
                // Heal the player over time
                case SoulData.StatType.HealPlayerOverTime:
                    refPlayerHealth.ChangeHealthRegneration(effect.statChange);
                    break;
                // Damage Player over Time
                case SoulData.StatType.DamagePlayerOverTime:
                    refPlayerHealth.ChangeHealthRegneration(-effect.statChange);
                    break;
                // Reduce damage taken by the player
                case SoulData.StatType.DamageReduction:
                    refPlayerHealth.ChangeDamageMultiplier(-effect.statChange);
                    break;
                case SoulData.StatType.DamageAmplification:
                    refPlayerHealth.ChangeDamageMultiplier(effect.statChange);
                    break;
                // Heal nearby enemies on kill
                case SoulData.StatType.EnemyHeal:
                    refPlayerAttack.currentEnemyDeathAOEDamage -= effect.statChange;
                    break;
                // Explode nearby enemies on kill
                case SoulData.StatType.ExplodingDamade:
                    refPlayerAttack.currentEnemyDeathAOEDamage += effect.statChange;
                    break;
                // Aoe scale of enemy death aoe
                case SoulData.StatType.EnemyDeathAOEScale:
                    refPlayerAttack.currentEnemyDeathAOEScale += effect.statChange;
                    break;
                // Inverts the player's controls
                case SoulData.StatType.InvertPlayerControls:
                    refPlayerMovement.InvertMovement();
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
        refPlayerMovement.ResetMoveSpeed();
        refPlayerHealth.ResetStats();
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
        // Clear the spawned items
        foreach (GameObject item in SpawnedItems)
        {
            if (item != null)
                Destroy(item);
        }
        SpawnedItems.Clear();
        SpawnedSoulSprites.Clear();
        // Spawn each object
        // Loop through the soul array and spawn the corresponding ui element for each soul in the correct position
        for (int i = 0; i < SpawnUIPlacesParents.Length; i++)
        {
            // Spawn the soul object
            if (Souls[i] != null)
            {
                SpawnedItems.Add(Instantiate(Souls[i].UIGameobject, SpawnUIPlacesParents[i].transform));
                SpawnedSoulSprites.Add(SpawnedItems[i].GetComponentInChildren<SpriteRenderer>());
            }
            // For each slot, set the corresponding background color for the soul
            if (Souls[i] != null)
            {
                // Check if the soul is inverted, and set the color accordingly
                if (Souls[i].IsSoulInverted)
                {
                    SpawnUIPlacesParents[i].GetComponent<SpriteRenderer>().color = InvertedSoulBackgroundColor;
                    SpawnedItems[i].GetComponentInChildren<SpriteRenderer>().color = SpawnedSoulSprites[i].color * InvertedSoulColor;
                }
                else
                {
                    SpawnUIPlacesParents[i].GetComponent<SpriteRenderer>().color = PositiveSoulBackgroundColor;
                    SpawnedItems[i].GetComponentInChildren<SpriteRenderer>().color = SpawnedSoulSprites[i].color * PositiveSoulColor;
                }
            }
            else
            {
                // Otherwise, default to the no soul color
                SpawnUIPlacesParents[i].GetComponent<SpriteRenderer>().color = NoSoulBackgroundColor;
            }
        }
        // Spawn the lines for decay amount
        UpdateDecayLines();

        // Update the text for each soul's stats
        for (int i = 0; i < Souls.Length; i++)
        {
            SoulData soul = Souls[i];
            if (soul != null)
            {
                // Get the text component for this soul
                TextMeshProUGUI textDisplay = StatTextDisplays[i];
                // Create a string to display the stats of the soul
                string displayString = "";
                List<SoulData.Effects> effects = soul.IsSoulInverted ? soul.invertedSoulEffects : soul.soulEffects;
                for (int j = 0; j < effects.Count; j++)
                {
                    SoulData.Effects effect = effects[j];
                    // Get the sign of the stat change, and add it to the display string along with the stat type
                    string sign;
                    // (im committing a sin)
                    if ((int)effect.statType == 6 || (int)effect.statType == 11)
                    {
                        sign = "x";
                    }
                    else if ((int) effect.statType == 5)
                    {
                        sign = "/";
                    }
                    else if ((int)effect.statType == 12)
                    {
                        sign = "";
                    }
                    else
                    {
                        sign = effect.statChange > 0 ? "+" : "";
                    }
                    // Assign the text to the textbox
                    displayString += StatTypeDisplayNames[(int)effect.statType] + " " + sign + (effect.statChange != 0 ? effect.statChange.ToString() : "") + "\n";
                }
                // Set the text of the text component to the display string
                textDisplay.text = displayString;
            }
            else
            {
                // If there is no soul, set the text to empty
                StatTextDisplays[i].text = "";
            }
        }
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
