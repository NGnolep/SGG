using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RadarController : MonoBehaviour
{
    public GameObject radar;
    public float animationDuration = 7f;
    [Range(0, 1)] public float successProbability = 1f;

    public GameObject rhythmGame;
    public GameObject searchScreen;
    public GameObject encycloScreen;

    public Button[] locationButtons; // Array of buttons for location
    public Button scanButton;

    public TMP_Text locationText;

    private bool isSuccess = false;
    private string currentFishLocation = "";

    private SpinningLine spiningLine;
    private EncyclopediaManager encyclopediaManager;
    private bool isScanning = false;
    public CircleGrowth circleManager;
    public HoverAnim buttonAnimation;
    public HoverAnimS buttonAnimationS;
    public HoverAnimKP buttonAnimationKP;
    public HoverAnimT buttonAnimationT;
    public AnimGo animGo;

    public AudioSource audioSource;
    public AudioClip clip;
    public AudioClip radarBeep;
    void Start()
    {
        encyclopediaManager = FindObjectOfType<EncyclopediaManager>();
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
            }
        }

        // Set up scan button
        scanButton.gameObject.SetActive(false);
        scanButton.onClick.RemoveAllListeners();
        scanButton.onClick.AddListener(StartSearch);
    }
    public void StartSearch()
    {

        if (isScanning) return;  // Prevent further clicks if already scanning
        isScanning = true;
        StartCoroutine(WaitTwoSeconds());

        searchScreen.SetActive(false);
        radar.SetActive(true);
        audioSource.PlayOneShot(radarBeep);
        StartCoroutine(RadarAnimation());
    }

    private IEnumerator WaitTwoSeconds()
    {
        yield return new WaitForSeconds(1f);
        // Code to execute after 2 seconds
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

        Debug.Log("Scan result: " + (isSuccess ? "Success" : "Failure"));

        if (isSuccess)
        {
            // spiningLine.ChangeToBlue();
            radar.SetActive(false);
            StartRhythm();
        }
        else
        {
            Debug.Log("Scan failed! Try scanning again.");
            // spiningLine.ChangeToRed();
            radar.SetActive(false);

            // Reactivate the search screen
            searchScreen.SetActive(true);
            locationText.text = $"Scan failed! The creature is still at {currentFishLocation}. Press Scan to try again.";
            foreach (Button button in locationButtons)
            {
                button.interactable = true; // Disable all buttons again
            }
            scanButton.gameObject.SetActive(false);
            isScanning = false; 
        }
    }

    private void StartRhythm()
    {
        if(circleManager != null)
        {
            scanButton.gameObject.SetActive(false);
            rhythmGame.SetActive(true);
            circleManager.StartRhythm();
        }
        else
        {
            Debug.LogError("CircleGrowth reference is not assigned!");
        }

    }

    public void RhythmComplete()
    {
        isScanning = false;
        rhythmGame.SetActive(false);
        scanButton.gameObject.SetActive(false);
        searchScreen.SetActive(true);
        
        if (encyclopediaManager != null)
        {
            if (encyclopediaManager.UnlockNextFact())
            {
                Debug.Log("Fact unlocked!");
            }
        }

        RelocateFish();

        foreach (Button button in locationButtons)
        {
            button.interactable = true;
        }
    }

    private void RelocateFish()
    {
        // Move the fish to a random button and update the location text
        string[] buttonLabels = { "The Reefs", "The Seabed", "The Trench", "The Kelp Forest" };
        int randomIndex = Random.Range(0, buttonLabels.Length);

        // Locations text for the fish's new position
        currentFishLocation = buttonLabels[randomIndex]; // Store the current fish location

        locationText.text = $"The creature is now at {currentFishLocation}!";
        Debug.Log($"Fish relocated to: {currentFishLocation}");
    }
    public void OnMissedScan()
    {
        isScanning = false;
        scanButton.gameObject.SetActive(false);

        // Relocate fish and update UI
        RelocateFish();
        locationText.text = $"Scan missed! The creature has relocated. Try again!";

        // Reactivate the search screen and location buttons
        searchScreen.SetActive(true);
        foreach (Button button in locationButtons)
        {
            button.interactable = true; // Re-enable buttons
        }
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
            locationText.text = $"The creature is at {trimmedButtonLabel}. Press Scan to continue!";
            scanButton.gameObject.SetActive(true); // Enable the scan button if the location is correct
            buttonAnimation.OnClick();
        }
        else
        {
            Debug.Log($"The fish is not at {trimmedButtonLabel}. Try again!");
            locationText.text = $"The creature is not at {trimmedButtonLabel}. Try again!";
            scanButton.gameObject.SetActive(false); // Disable the scan button if the location is incorrect

            foreach (Button button in locationButtons)
            {
                button.interactable = true;
            }
        }
    }

    public void Clicked()
    {
        audioSource.PlayOneShot(clip);
        buttonAnimation.OnClick();
    }
    public void Clicked2()
    {
        audioSource.PlayOneShot(clip);
        buttonAnimationS.OnClick();
    }
    public void Clicked3()
    {
        audioSource.PlayOneShot(clip);
        buttonAnimationKP.OnClick();
    }
    public void Clicked4()
    {
        audioSource.PlayOneShot(clip);
        buttonAnimationT.OnClick();
    }

    public void ClickedGo()
    {
        audioSource.PlayOneShot(clip);
        animGo.OnClick();
    }

    public void OpenEncy()
    {
        searchScreen.SetActive(false);
        encycloScreen.SetActive(true);
    }
}
