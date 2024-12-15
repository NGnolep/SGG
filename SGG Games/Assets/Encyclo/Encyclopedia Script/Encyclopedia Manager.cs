using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EncyclopediaManager : MonoBehaviour
{
    public FishData[] fishDataList;
    public TMP_Text fishNameText;     
    public TMP_Text fishDescriptionText;  
    public TMP_Text fishFactsText;   
    public Button[] factButtons;    
    public Button nextButton;        
    public Button prevButton;        
    public Button closeButton;

    public Image fishImage;

    private int currentFishIndex = 0;
    private int collectedData = 0;

    void Start()
    {
        InitializeFishData();
        UpdateEncyclopediaUI();

        nextButton.onClick.AddListener(NextFish);
        prevButton.onClick.AddListener(PrevFish);
        closeButton.onClick.AddListener(CloseEncyclopedia);

        foreach (Button button in factButtons)
        {
            int index = System.Array.IndexOf(factButtons, button);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnFactButtonPressed(index));
            button.interactable = false;
        }
    }

    private void InitializeFishData()
    {
        for (int i = 0; i < fishDataList.Length; i++)
        {
            fishDataList[i] = new FishData(5);
            fishDataList[i].fishName = "Fish " + (i + 1);
            fishDataList[i].fishDescription = "Description of Fish " + (i + 1);
            for (int j = 0; j < 5; j++)
            {
                fishDataList[i].fishFacts[j] = $"Fact {j + 1} about Fish {i + 1}";
            }
        }
    }

    public void CollectFishData()
    {
        if (collectedData < 5)
        {
            fishDataList[currentFishIndex].dataUnlocked[collectedData] = true;
            factButtons[collectedData].interactable = true;
            collectedData++;

            if (collectedData == 5)
            {
                Debug.Log($"All data unlocked for {fishDataList[currentFishIndex].fishName}!");

                if (currentFishIndex < fishDataList.Length - 1)
                {
                    currentFishIndex++;
                    collectedData = 0;
                    UpdateEncyclopediaUI();
                }
                else
                {
                    Debug.Log("All fish data unlocked! Game Complete!");
                }
            }
        }
    }

    private void UpdateEncyclopediaUI()
    {
        if (currentFishIndex < fishDataList.Length)
        {
            FishData currentFish = fishDataList[currentFishIndex];

            // Update text fields
            fishNameText.text = currentFish.fishName;
            fishDescriptionText.text = currentFish.fishDescription;

            if (fishImage != null)
            {
                fishImage.sprite = currentFish.fishImage; // Update the image
            }
            // Enable fact buttons that are unlocked
            for (int i = 0; i < factButtons.Length; i++)
            {
                if (i < currentFish.fishFacts.Length && currentFish.dataUnlocked[i])
                {
                    factButtons[i].interactable = true;
                    factButtons[i].onClick.RemoveAllListeners();
                    int factIndex = i;
                    factButtons[i].onClick.AddListener(() => OnFactButtonPressed(factIndex));
                }
                else
                {
                    factButtons[i].interactable = false;  // Disable if not unlocked
                }
            }

            // Disable Prev button if we're at the first fish
            prevButton.interactable = currentFishIndex > 0;

            // Disable Next button if we're at the last fish
            nextButton.interactable = currentFishIndex < fishDataList.Length - 1;
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

    // Go to the previous fish in the list
    private void PrevFish()
    {
        if (currentFishIndex > 0)
        {
            currentFishIndex--;
            UpdateEncyclopediaUI();
        }
    }

    // Close the encyclopedia UI
    private void CloseEncyclopedia()
    {
        gameObject.SetActive(false); // Deactivate the encyclopedia panel
    }

    // Handle fact button press and show the fact in the UI
    public void OnFactButtonPressed(int index)
    {
        if (index >= 0 && index < fishDataList[currentFishIndex].fishFacts.Length)
        {
            string fact = fishDataList[currentFishIndex].fishFacts[index];
            fishFactsText.text = fact; // Show the fact
        }
    }
}
