using System.Collections.Generic;
using UnityEngine;

public class SoulData : MonoBehaviour
{
    [SerializeField] List<Effects> soulEffects = new List<Effects>();
    [SerializeField] List<Effects> invertedSoulEffects = new List<Effects>();
    [SerializeField] float soulLifespan = 30f;

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
}
