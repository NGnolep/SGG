using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RadarController : MonoBehaviour
{
    public FishData[] fishDataList;
    public GameObject encyclopediaPanel; 
    public Button[] factButtons;

    public GameObject radar;
    public float animationDuration = 5f;
    [Range(0, 1)] public float successProbability = 1f;

    public GameObject rhythmGame;
    public GameObject searchScreen;

    public Button[] locationButtons; // Array of buttons for location
    public Button scanButton;

    public TMP_Text locationText;

    private int currentFishIndex = 0;
    private int collectedData = 0;

    private bool isSuccess = false;
    private string currentFishLocation = "";

    void Start()
    {
        InitializeFishData();
        RelocateFish();

        // Set up the location buttons to check the fish location
        foreach (Button button in locationButtons)
        {
            button.onClick.RemoveAllListeners();
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                string buttonLabel = buttonText.text;
                button.onClick.AddListener(() => OnSearchButtonPressed(buttonLabel));
                Debug.Log($"Listener added for button: {buttonLabel}");
            }
        }
        foreach (Button button in factButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnFactButtonPressed(button));
            button.interactable = false; // Disable all buttons initially
        }
        // Disable the scan button initially until the location is found
        scanButton.gameObject.SetActive(false);
        scanButton.onClick.RemoveAllListeners();
        scanButton.onClick.AddListener(StartSearch);

        UpdateEncyclopediaUI();
    }

    private void InitializeFishData()
    {
        // Initialize the fish data list for all fish
        for (int i = 0; i < fishDataList.Length; i++)
        {
            fishDataList[i] = new FishData(5); // Initialize with 5 facts per fish
            fishDataList[i].fishName = "Fish " + (i + 1);
            fishDataList[i].fishDescription = "Description of Fish " + (i + 1);
            for (int j = 0; j < 5; j++)
            {
                fishDataList[i].fishFacts[j] = "Fact " + (j + 1) + " about " + fishDataList[i].fishName;
            }
        }
    }
    public void StartSearch()
    {
        searchScreen.SetActive(false);
        radar.SetActive(true);

        StartCoroutine(RadarAnimation());
    }

    private IEnumerator RadarAnimation()
    {
        // Animate radar for the specified duration
        yield return new WaitForSeconds(animationDuration);

        // Determine if the search is successful based on success probability
        isSuccess = Random.value <= successProbability;

        EndSearch();
    }

    private void EndSearch()
    {
        radar.SetActive(false);

        if (isSuccess)
        {
            StartRhythm();
        }
        else
        {
            Debug.Log("Scan failed! Try scanning again.");

            // Reactivate the search screen
            searchScreen.SetActive(true);

            locationText.text = $"Scan failed! The fish is still at {currentFishLocation}. Press Scan to try again.";

            scanButton.gameObject.SetActive(true); // Re-enable the scan button for retry

            foreach (Button button in locationButtons)
            {
                button.interactable = true; // Enable all buttons again
            }
        }
    }

    private void StartRhythm()
    {
        rhythmGame.SetActive(true);
    }

    public void RhythmComplete()
    {
        CollectFishData();
        rhythmGame.SetActive(false);
        searchScreen.SetActive(true);
    }

    private void RelocateFish()
    {
        // Move the fish to a random button and update the location text
        string[] buttonLabels = { "The Reefs", "The Seabed", "The Trench", "The Kelp Forest" };
        int randomIndex = Random.Range(0, buttonLabels.Length);

        // Locations text for the fish's new position
        currentFishLocation = buttonLabels[randomIndex]; // Store the current fish location

        locationText.text = $"The fish is now {currentFishLocation}!";
        Debug.Log($"Fish relocated to: {currentFishLocation}");
    }

    public void CollectFishData()
    {
        if (collectedData < 5)
        {
            // Unlock the next piece of data
            fishDataList[currentFishIndex].dataUnlocked[collectedData] = true;
            collectedData++;

            // Enable the corresponding fact button
            if (collectedData <= 5)
            {
                factButtons[collectedData - 1].interactable = true;
            }

            // Check if all data for the current fish is unlocked
            if (fishDataList[currentFishIndex].IsDataComplete())
            {
                Debug.Log($"All data unlocked for {fishDataList[currentFishIndex].fishName}!");

                // Transition to the next fish if there are more
                if (currentFishIndex < fishDataList.Length - 1)
                {
                    currentFishIndex++;
                    collectedData = 0; // Reset collected data for the next fish
                    UpdateEncyclopediaUI();
                }
                else
                {
                    Debug.Log("All fish data unlocked! Game Complete!");
                    // Handle game completion logic here (e.g., go to the next level or show a completion screen)
                }
            }
        }
    }

    private void UpdateEncyclopediaUI()
    {
        if (currentFishIndex < fishDataList.Length)
        {
            // Update fish name, description, and facts
            TMP_Text fishNameText = encyclopediaPanel.transform.Find("FishName").GetComponent<TMP_Text>();
            TMP_Text fishDescriptionText = encyclopediaPanel.transform.Find("FishDescription").GetComponent<TMP_Text>();
            fishNameText.text = fishDataList[currentFishIndex].fishName;
            fishDescriptionText.text = fishDataList[currentFishIndex].fishDescription;

            // Update the fact buttons' text
            for (int i = 0; i < factButtons.Length; i++)
            {
                TMP_Text buttonText = factButtons[i].GetComponentInChildren<TMP_Text>();
                if (i < fishDataList[currentFishIndex].fishFacts.Length)
                {
                    buttonText.text = fishDataList[currentFishIndex].fishFacts[i];
                }
            }
        }
    }

    public void OnFactButtonPressed(Button button)
    {
        // Find which button was clicked and display the corresponding fact
        int index = System.Array.IndexOf(factButtons, button);
        if (index >= 0 && index < fishDataList[currentFishIndex].fishFacts.Length)
        {
            string fact = fishDataList[currentFishIndex].fishFacts[index];
            Debug.Log("Fact: " + fact);

            // Display the fact (e.g., update a UI panel)
            TMP_Text fishFactsText = encyclopediaPanel.transform.Find("FishFacts").GetComponent<TMP_Text>();
            fishFactsText.text = fact;  // Show the fact when the button is pressed
        }
    }
    public void OnMissedScan()
    {
        // Logic for handling missed scans, e.g., penalize the player or show feedback
        Debug.Log("Missed Scan!");
        RelocateFish();
    }

    public void OnSearchButtonPressed(string buttonLabel)
    {
        // Trim the input strings to ensure no extra spaces and compare case-insensitively
        string trimmedButtonLabel = buttonLabel.Trim();
        string trimmedFishLocation = currentFishLocation.Trim();

        // Log for debugging to ensure correct button text comparison
        Debug.Log($"Button pressed: {trimmedButtonLabel}, Fish is at: {trimmedFishLocation}");

        // Disable all buttons
        foreach (Button button in locationButtons)
        {
            button.interactable = false;
        }

        // Compare the button label with the fish location (case-insensitive)
        if (string.Equals(trimmedButtonLabel, trimmedFishLocation, System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Fish found at this location, proceeding to scan...");
            locationText.text = $"The fish is at {trimmedButtonLabel}. Press Scan to continue!";
            scanButton.gameObject.SetActive(true); // Enable the scan button if the location is correct
        }
        else
        {
            Debug.Log($"The fish is not at {trimmedButtonLabel}. Try again!");
            locationText.text = $"The fish is not at {trimmedButtonLabel}. Try again!";
            scanButton.gameObject.SetActive(false); // Disable the scan button if the location is incorrect
        }
    }
}
