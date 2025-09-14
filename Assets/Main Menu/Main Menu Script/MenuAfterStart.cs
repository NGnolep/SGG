using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuAfterStart : MonoBehaviour
{
    public AudioClip[] startSounds;         // Array of audio clips to choose from
    public GameObject firstGameObject;      // First GameObject to activate
    public GameObject secondGameObject;     // Second GameObject to activate after the fade-out
    public GameObject bg;
    public float displayDuration = 5f;     // Duration to display the first GameObject in seconds
    public float fadeOutDuration = 1f;     // Duration for fading out the GameObject

    private CanvasGroup canvasGroup;       // Used to control the fade out effect
    public AudioSource audioSource;       // Audio source component to play the sound

    void Start()
    {
        canvasGroup = firstGameObject.GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

        // Choose a random sound clip from the array
        if (startSounds.Length > 0)
        {
            AudioClip chosenClip = startSounds[Random.Range(0, startSounds.Length)];
            audioSource.clip = chosenClip;
            audioSource.Play();
        }

        // Start the start sequence
        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        // Display the first GameObject
        firstGameObject.SetActive(true);

        // Wait for the display duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out the first GameObject
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            float alpha = 1 - (elapsedTime / fadeOutDuration);
            canvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;

        // Deactivate the first GameObject
        firstGameObject.SetActive(false);

        // Activate the second GameObject
        if (secondGameObject != null)
        {
            secondGameObject.SetActive(true);
           
            bg.SetActive(false);
        }
    }
}
