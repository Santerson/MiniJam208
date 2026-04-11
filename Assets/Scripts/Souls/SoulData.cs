using System;
using System.Collections.Generic;
using UnityEngine;

public class SoulData : MonoBehaviour
{
    [Tooltip("The effects of this soul")]
    public List<Effects> soulEffects = new List<Effects>();
    [Tooltip("The inverted effects of the soul")]
    public List<Effects> invertedSoulEffects = new List<Effects>();
    [Tooltip("The lifespan of the soul in seconds")]
    public float soulLifespan = 30f;
    [Tooltip("The Gameobject this object spawns in on the ui")] 
    public GameObject UIGameobject = null;
    /// <summary>
    /// Whether or not this soul is inverted
    /// </summary>
    [HideInInspector] public bool IsSoulInverted { get; private set; } = false;

    // Time until this soul becomes corrupt
    float soulLifeSpanLeft;

    /// <summary>
    /// Soul containing an effect of a soul
    /// </summary>
    [System.Serializable]
    public struct Effects
    {
        public StatType statType;
        public float statChange;
    }

    /// <summary>
    /// An enum consisting of different stats that can be affected by souls.
    /// </summary>
    public enum StatType
    {
        AttackDamage,
        AttackRange,
        AttackSpeed,
        MoveSpeed
    }

    private void Start()
    {
        // Set the soul's lifespan
        soulLifeSpanLeft = soulLifespan;
    }

    void Update()
    {
        // Decrease the soul's lifespan
        if (!IsSoulInverted)
        {
            soulLifeSpanLeft -= Time.deltaTime;
            // If the soul's lifespan is up, invert the soul
            if (soulLifeSpanLeft <= 0)
            {
                InvertSoul();
            }
        }
    }

    void InvertSoul()
    {
        // Invert the soul
        IsSoulInverted = true;
        // Reapply soul effects
        FindFirstObjectByType<SoulManager>().ApplyAllSouls();
        // Do something cool here too
    }
}
