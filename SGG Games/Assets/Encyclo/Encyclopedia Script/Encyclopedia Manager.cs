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

    private int currentFishIndex = 0;

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
            fishNameText.text = currentFish.fishName;
            fishDescriptionText.text = currentFish.fishDescription;
            fishImage.sprite = currentFish.fishImage;

            bool anyFactsUnlocked = false;

            // Update fact buttons based on unlock state
            for (int i = 0; i < factButtons.Length; i++)
            {
                if (i < currentFish.fishFacts.Length && currentFish.dataUnlocked[i])
                {
                    factButtons[i].interactable = true;
                    anyFactsUnlocked = true;
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

    private void CloseEncyclopedia()
    {
        gameObject.SetActive(false); // Deactivate the encyclopedia panel
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
    public void UnlockFact(int factIndex)
    {
        FishDataScriptableObject currentFish = fishDataList[currentFishIndex];
        if (factIndex >= 0 && factIndex < currentFish.dataUnlocked.Length)
        {
            currentFish.dataUnlocked[factIndex] = true;
            UpdateEncyclopediaUI();
        }
    }
}
