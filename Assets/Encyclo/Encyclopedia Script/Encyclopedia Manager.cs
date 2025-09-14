using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EncyclopediaManager : MonoBehaviour
{
    [Header("Fish Data")]
    public FishDataScriptableObject[] fishDataList; // Array of fish ScriptableObjects

    [Header("UI Elements")]
    public TMP_Text fishNameText;
    public TMP_Text fishDescriptionText;
    public TMP_Text fishFactsText;  // Display for fact title and description
    public Button[] factButtons;    // Buttons for fish facts
    public Button nextButton;
    public Button prevButton;
    public Button closeButton;
    public Image fishImage;

    public GameObject encycloScreen;
    public GameObject searchScreen;
    private int currentFishIndex = 0;
    private RadarController radarController;
    void Start()
    {
        // Set up button listeners
        nextButton.onClick.AddListener(NextFish);
        prevButton.onClick.AddListener(PrevFish);
        closeButton.onClick.AddListener(CloseEncyclopedia);

        for (int i = 0; i < factButtons.Length; i++)
        {
            int index = i; // Prevent closure issue
            factButtons[i].onClick.AddListener(() => OnFactButtonPressed(index));
        }

        UpdateEncyclopediaUI();
    }

    private void UpdateEncyclopediaUI()
    {
        if (currentFishIndex >= 0 && currentFishIndex < fishDataList.Length)
        {
            FishDataScriptableObject currentFish = fishDataList[currentFishIndex];

            // Update fish name, description, and image
            fishNameText.text = "???";
            fishDescriptionText.text = "???";
            fishImage.sprite = currentFish.lockedImage;

            bool anyFactsUnlocked = false;

            // Update fact buttons based on unlock state
            for (int i = 0; i < factButtons.Length; i++)
            {
                if (i < currentFish.fishFacts.Length && currentFish.dataUnlocked[i])
                {
                    factButtons[i].interactable = true;
                    anyFactsUnlocked = true;
                    if (i == 0)
                    {
                        fishNameText.text = currentFish.fishName;
                        fishDescriptionText.text = currentFish.fishDescription;
                        fishImage.sprite = currentFish.fishImage; // Replace with the unlocked image
                    }
                }
                else
                {
                    factButtons[i].interactable = false;
                }
            }

            // Update navigation buttons
            prevButton.interactable = currentFishIndex > 0;
            nextButton.interactable = currentFishIndex < fishDataList.Length - 1;

            // Update facts display text
            fishFactsText.text = anyFactsUnlocked ? "Press a fact button" : "???";
        }
    }

    private void NextFish()
    {
        if (currentFishIndex < fishDataList.Length - 1)
        {
            currentFishIndex++;
            UpdateEncyclopediaUI();
        }
    }

    private void PrevFish()
    {
        if (currentFishIndex > 0)
        {
            currentFishIndex--;
            UpdateEncyclopediaUI();
        }
    }

    public void CloseEncyclopedia()
    {
        encycloScreen.SetActive(false);
        searchScreen.SetActive(true);
    }

    private void OnFactButtonPressed(int index)
    {
        FishDataScriptableObject currentFish = fishDataList[currentFishIndex];
        if (index >= 0 && index < currentFish.fishFacts.Length && currentFish.dataUnlocked[index])
        {
            string title = currentFish.fishFacts[index].title;
            string description = currentFish.fishFacts[index].description;

            fishFactsText.text = $"<b>{title}</b>\n{description}";
        }
    }

    // Unlock a new fact for the current fish
    public bool UnlockNextFact()
    {
        FishDataScriptableObject currentFish = fishDataList[currentFishIndex];

        for (int i = 0; i < currentFish.dataUnlocked.Length; i++)
        {
            if (!currentFish.dataUnlocked[i])
            {
                currentFish.dataUnlocked[i] = true;
                UpdateEncyclopediaUI();
                return true; // Successfully unlocked a fact
            }
        }

        return false; // No more facts to unlock
    }

    public bool IsFishComplete(FishDataScriptableObject fish)
    {
        foreach (bool unlocked in fish.dataUnlocked)
        {
            if (!unlocked) return false;
        }
        return true;
    }

    public void MoveToNextFish()
    {
        // Generate a random index and ensure the fish is not fully unlocked
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, fishDataList.Length);
        } while (IsFishComplete(fishDataList[randomIndex]));

        currentFishIndex = randomIndex;
        UpdateEncyclopediaUI();
    }
}
