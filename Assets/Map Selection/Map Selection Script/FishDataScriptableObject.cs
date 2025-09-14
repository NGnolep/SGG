using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFishData", menuName = "Encyclopedia/Fish Data")]
public class FishDataScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class Fact
    {
        public string title;          // Title of the fact
        public string description;    // Description of the fact
    }

    public string fishName;                // Fish name
    public string fishDescription;         // Fish description
    public Sprite fishImage;               // Fish image
    public Sprite lockedImage;
    public Fact[] fishFacts = new Fact[5]; // Facts about the fish
    public bool[] dataUnlocked = new bool[5];  // Track unlocked facts
}
 