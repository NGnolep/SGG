using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] // To allow it to show up in the Unity Inspector
public class FishData
{
    public string fishName;
    public string fishDescription;
    public Sprite fishImage;
    public string[] fishFacts; // Array of facts for each fish
    public bool[] dataUnlocked; // Tracks which facts are unlocked (5 facts per fish)

    // Constructor to initialize dataUnlocked to false for all facts
    public FishData(int factCount)
    {
        dataUnlocked = new bool[factCount];
        fishFacts = new string[factCount];
    }

    public bool IsDataComplete()
    {
        // Check if all data has been unlocked
        foreach (var unlocked in dataUnlocked)
        {
            if (!unlocked) return false;
        }
        return true;
    }
}