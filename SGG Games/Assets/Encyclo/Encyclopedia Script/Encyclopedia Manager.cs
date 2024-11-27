using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EncyclopediaManager : MonoBehaviour
{
    public RadarController radarController;  // Reference to the RadarController script
    public GameObject encyclopediaPanel;    // Reference to the Encyclopedia panel

    public TMP_Text fishNameText;           // Text for the fish name
    public TMP_Text fishDescriptionText;    // Text for the fish description
    public TMP_Text fishFactsText;          // Text for unlocked facts

    public Button nextButton;               // Button to go to the next fish
    public Button previousButton;           // Button to go to the previous fish
    public Button closeButton;              // Button to close the Encyclopedia

    private int currentFishIndex = 0;       // Tracks which fish is currently being displayed

    void Start()
    {
        // Set up button listeners
        nextButton.onClick.AddListener(GoToNextFish);
        previousButton.onClick.AddListener(GoToPreviousFish);
        closeButton.onClick.AddListener(CloseEncyclopedia);

        // Show the first fish when the Encyclopedia opens
        UpdateFishDetails();
    }

    // Update the UI to show the current fish's data
    private void UpdateFishDetails()
    {
        // Fetch the current fish data
        if (currentFishIndex >= 0 && currentFishIndex < radarController.fishDataList.Length)
        {
            FishData currentFish = radarController.fishDataList[currentFishIndex];

            // Update UI elements
            fishNameText.text = currentFish.fishName;
            fishDescriptionText.text = currentFish.fishDescription;

            // Display unlocked facts
            string facts = "";
            for (int i = 0; i < currentFish.dataUnlocked.Length; i++)
            {
                if (currentFish.dataUnlocked[i])
                {
                    // Optionally use Rich Text to style unlocked facts
                    facts += $"<b>{currentFish.fishFacts[i]}</b>\n";  // Bold the unlocked facts
                }
            }
            fishFactsText.text = string.IsNullOrEmpty(facts) ? "No facts unlocked yet!" : facts;

            // Enable/Disable navigation buttons based on the current index
            previousButton.interactable = currentFishIndex > 0; // Disable Previous if on the first index
            nextButton.interactable = currentFishIndex < radarController.fishDataList.Length - 1; // Disable Next if on the last index
        }
    }

    // Go to the next fish
    private void GoToNextFish()
    {
        Debug.Log("button pressed");
        if (currentFishIndex < radarController.fishDataList.Length - 1)
        {
            currentFishIndex++;
            UpdateFishDetails();
        }
    }

    // Go to the previous fish
    private void GoToPreviousFish()
    {
        if (currentFishIndex > 0)
        {
            currentFishIndex--;
            UpdateFishDetails();
        }
    } 

// Close the Encyclopedia
    private void CloseEncyclopedia()
    {
        encyclopediaPanel.SetActive(false);
    }
}
