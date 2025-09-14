using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] // To allow it to show up in the Unity Inspector
public class FishData
{
    public string fishName;          // Fish name
    public string fishDescription;   // Fish description
    public Sprite fishImage;         // Fish image
    public string[] fishFacts;       // Array of facts for each fish
    public bool[] dataUnlocked;      // Tracks which facts are unlocked (5 facts per fish)

    // Constructor to initialize dataUnlocked to false for all facts
    public FishData(int factCount)
    {
        dataUnlocked = new bool[factCount];
        fishFacts = new string[factCount];

        // Initialize fishFacts with default values (empty strings or custom placeholder)
        for (int i = 0; i < factCount; i++)
        {
            fishFacts[i] = $"Fact {i + 1} about {fishName}";
        }
    }

    // Check if all facts have been unlocked
    public bool IsDataComplete()
    {
        foreach (var unlocked in dataUnlocked)
        {
            if (!unlocked) return false; // Return false if any fact is not unlocked
        }
        return true; // Return true if all facts are unlocked
    }

    // Mark all facts as unlocked (to be called when the player has fully unlocked a fish)
    public void UnlockAllData()
    {
        for (int i = 0; i < dataUnlocked.Length; i++)
        {
            dataUnlocked[i] = true; // Mark all facts as unlocked
        }
    }

    // Optionally, a helper function to unlock a single fact based on index
    public void UnlockFact(int factIndex)
    {
        if (factIndex >= 0 && factIndex < dataUnlocked.Length)
        {
            dataUnlocked[factIndex] = true;
        }
    }

    // Optionally, a function to reset data (in case you want to reset the fish data during the game)
    public void ResetData()
    {
        for (int i = 0; i < dataUnlocked.Length; i++)
        {
            dataUnlocked[i] = false; // Reset all facts to locked
        }
    }
}