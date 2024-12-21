using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBlinker : MonoBehaviour
{
    public float blinkSpeed = 1f;

    [Header("UI Elements")]
    public GameObject startText;
    public GameObject mainMenu;
    public GameObject startMenu;

    [Header("Buttons")]
    public Button startButton;
    public Button tutorialButton;
    public Button settingButton;
    public Button quitButton;
    public Button quitGameButton;

    [Header("Tutorial and Settings Menus")]
    public GameObject tutorialMenu;
    public GameObject settingsMenu;
    public GameObject quitPrompt;
    public GameObject main;
    public GameObject device;

    [Header("Sound Settings")]
    public Slider soundSlider; // Slider to adjust volume
    public AudioSource backgroundMusic; // Background music to test sound
    public AudioClip buttonClickSound;
    public AudioSource audioSource;

    public OpenDeviceAnim open;
    
    private CanvasGroup textCanvasGroup;
    private bool isBlinking = true;
    private bool hasPlayed = false;
    private void Start()
    {
        // Initialize Start Menu visibility
        if (startMenu != null) startMenu.SetActive(true);
        if (mainMenu != null) mainMenu.SetActive(false);
        if (tutorialMenu != null) tutorialMenu.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(false);
        if (quitPrompt != null) quitPrompt.SetActive(false);
        if (device != null) device.SetActive(false);
        // Setup blinking text
        textCanvasGroup = startText.GetComponent<CanvasGroup>();
        StartCoroutine(BlinkText());

        // Button listeners
        startButton.onClick.AddListener(StartGame);
        tutorialButton.onClick.AddListener(() => OpenMenu(tutorialMenu));
        settingButton.onClick.AddListener(() => OpenMenu(settingsMenu));
        quitButton.onClick.AddListener(() => OpenMenu(quitPrompt));
        quitGameButton.onClick.AddListener(OutGame);
        // Set initial sound volume
        if (soundSlider != null)
        {
            soundSlider.value = PlayerPrefs.GetFloat("Volume", 1f); // Load saved volume
            AdjustVolume(soundSlider.value);
            soundSlider.onValueChanged.AddListener(AdjustVolume);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnScreenClicked();
        }
    }

    private IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            for (float t = 0; t < 1; t += Time.deltaTime * blinkSpeed)
            {
                textCanvasGroup.alpha = Mathf.Lerp(1, 0, t);
                yield return null;
            }

            for (float t = 0; t < 1; t += Time.deltaTime * blinkSpeed)
            {
                textCanvasGroup.alpha = Mathf.Lerp(0, 1, t);
                yield return null;
            }
        }
    }

    private void OnScreenClicked()
    {
        isBlinking = false;
        textCanvasGroup.alpha = 1;
        if (startText != null) startMenu.SetActive(false);
        if(hasPlayed != true)
        {
            audioSource.PlayOneShot(buttonClickSound);
            if (device != null) device.SetActive(true);
            open.playAnim();
            hasPlayed = true;
        }
}
    // --- Button Functions ---
    private IEnumerator waitSeconds()
    {
        yield return new WaitForSeconds(2f);
    }
    private void StartGame()
    {
        audioSource.PlayOneShot(buttonClickSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene
        hasPlayed = false;
    }
    private void OutGame()
    {
        audioSource.PlayOneShot(buttonClickSound);
        Debug.Log("Quit button pressed.");
        Application.Quit();
        hasPlayed = false;
    }
    private void OpenMenu(GameObject menu)
    {
        audioSource.PlayOneShot(buttonClickSound);
        menu.SetActive(true);
        SetMainInteractable(false); // Disable main buttons and hide main GameObject
    }

    public void CloseMenu(GameObject menu)
    {
        audioSource.PlayOneShot(buttonClickSound);
        menu.SetActive(false);
        SetMainInteractable(true); // Re-enable main buttons and show main GameObject
    }

    private void SetMainInteractable(bool state)
    {
        main.SetActive(state); // Show or hide the main GameObject

        // Enable or disable main buttons
        startButton.interactable = state;
        tutorialButton.interactable = state;
        settingButton.interactable = state;
        quitButton.interactable = state;
    }

    // --- Sound Settings ---
    private void AdjustVolume(float value)
    {
        AudioListener.volume = value; // Adjust global volume
        PlayerPrefs.SetFloat("Volume", value); // Save volume across scenes
        PlayerPrefs.Save();
    }
}
