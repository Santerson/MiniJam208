using System.Collections.Generic;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
    GameObject[] Souls = new GameObject[5];

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
        // Remove the last soul
        Souls[4] = null;
        // Move all souls up one slot
        for (int i = 3; i > 0; i--)
        {
            Souls[i] = Souls[i - 1];
        }
        // Add the new soul to the first slot
        Souls[0] = Soul;
    }
}
