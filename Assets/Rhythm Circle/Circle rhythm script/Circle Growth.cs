using UnityEngine;
using System.Collections;
using TMPro;

public class CircleGrowth : MonoBehaviour
{
    [Header("Circle Reference")]
    public Transform innerCircle;
    public Transform outerCircle;

    [Header("Circle Settings")]
    private float growSpeed = 0.7f;
    private float perfectRange = 0.2f;
    private float maxScaleThreshold = 1.1f;

    [Header("Scoring")]
    public int scanned = 0;
    public int missScan = 0;

    private bool hasScored = false;
    private bool canScore = false;
    private bool hasMissed = false;
    private bool isGameOver = false;
    private bool isActive = false;

    [Header("Feedback")]
    public VisualFeedback visualFeedback;
    private SpriteRenderer innerCircleRenderer;
    public float fadeInDuration = 1f;

    public RadarController scanningManager;
    public EncyclopediaManager encyclopediaManager;
    public GameObject feedbackTextPrefab;
    public AudioClip hitSoundClip;
    public AudioClip missSoundClip;
    private AudioSource audioSource;
    void Start()
    {
        innerCircleRenderer = innerCircle.GetComponent<SpriteRenderer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        ResetCircle();
        StopAllCoroutines();
    }

    void Update()
    {
        if (!isActive || isGameOver) return;

        if (innerCircle.localScale.x < outerCircle.localScale.x)
        {
            innerCircle.localScale += Vector3.one * growSpeed * Time.deltaTime;
            canScore = true;
        }
        else if (!hasMissed && !hasScored)
        {
            HandleMiss("Too Slow");
        }

        if (Input.GetKeyDown(KeyCode.Space) && canScore && !hasMissed)
        {
            CheckForScore();
        }
    }

    public void StartRhythm()
    {
        if (isActive) return;

        isActive = true;
        isGameOver = false;
        ResetCircle();

        if (innerCircleRenderer != null)
        {
            StartCoroutine(FadeInInnerCircle());
        }
    }

    public void StopRhythm()
    {
        scanned = 0;
        missScan = 0;
        isActive = false;
        StopAllCoroutines();
        ResetCircle();
    }

    private void CheckForScore()
    {
        float distance = Mathf.Abs(innerCircle.localScale.x - outerCircle.localScale.x);
        if (distance <= perfectRange)
        {
            scanned++;
            hasScored = true;
            PlayFeedback("Hit");
            if (scanned >= 10)
            {
                EndGameFeedback(true);
                scanningManager.RhythmComplete();
                StopRhythm();
            }
        }
        else
        {
            HandleMiss("Too Fast!");
        }

        ResetCircle();
    }

    private void HandleMiss(string message)
    {
        missScan++;
        hasMissed = true;
        PlayFeedback("Miss");
        ResetCircle();

        if (missScan >= 5)
        {
            isGameOver = true;
            EndGameFeedback(false);
            scanningManager.OnMissedScan();
            StopRhythm();
        }
    }

    void ResetCircle()
    {
        StopAllCoroutines();

        if (innerCircleRenderer != null)
        {
            Color resetColor = innerCircleRenderer.color;
            resetColor.a = 1;
            innerCircleRenderer.color = resetColor;
        }

        innerCircle.localScale = Vector3.one * 0.1f;

        canScore = false;
        hasMissed = false;
        hasScored = false;
        visualFeedback.outerRenderer.material.color = visualFeedback.normalColor;
        growSpeed = Random.Range(0.4f, 1f);
        StartCoroutine(StartGrowthDelay());
    }

    IEnumerator StartGrowthDelay()
    {
        yield return new WaitForSeconds(0.5f);
        canScore = true;
    }

    IEnumerator FadeInInnerCircle()
    {
        Color startColor = innerCircleRenderer.color;
        startColor.a = 0;
        innerCircleRenderer.color = startColor;

        Color targetColor = startColor;
        targetColor.a = 1;
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            innerCircleRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeInDuration);
            yield return null;
        }

        innerCircleRenderer.color = targetColor;
    }

    private void PlayFeedback(string type)
    {
        if (type == "Hit")
        {
            if (hitSoundClip != null)
            {
                audioSource.PlayOneShot(hitSoundClip);
            }

            ShowFeedbackText("Hit!");
        }
        else if (type == "Miss")
        {
            if (missSoundClip != null)
            {
                audioSource.PlayOneShot(missSoundClip);
            }

            ShowFeedbackText("Miss!");
        }
    }

    private void ShowFeedbackText(string message)
    {
        if (feedbackTextPrefab != null)
        {
            // Find the canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Canvas not found!");
                return;
            }

            // Instantiate the feedback text under the canvas
            GameObject feedbackText = Instantiate(feedbackTextPrefab, canvas.transform);

            // Configure the text component
            TMP_Text feedbackTextComponent = feedbackText.GetComponent<TMP_Text>();
            if (feedbackTextComponent != null)
            {
                feedbackTextComponent.text = message;
            }

            // Set a random position near the center
            RectTransform feedbackRectTransform = feedbackText.GetComponent<RectTransform>();
            if (feedbackRectTransform != null)
            {
                feedbackRectTransform.anchoredPosition = new Vector2(
                    Random.Range(-250f, 250f), // Random X near center
                    Random.Range(-250f, 250f) // Random Y near center
                );
            }

            // Destroy the feedback text after 1 second
            Destroy(feedbackText, 0.5f);
        }
    }

    private void EndGameFeedback(bool isSuccess)
    {
        // Create and configure feedback text dynamically
        CreateDynamicFeedbackText(isSuccess ? "Data Unlocked!" : "The fish ran away!");

        // Deactivate the background after 1 secon
    }

    private void CreateDynamicFeedbackText(string message)
    {
        if (feedbackTextPrefab == null)
        {
            Debug.LogError("Feedback text prefab not assigned!");
            return;
        }

        // Instantiate the text prefab as a child of the background
        GameObject feedbackText = Instantiate(feedbackTextPrefab, FindObjectOfType<Canvas>().transform);

        // Configure the text component
        TMP_Text feedbackTextComponent = feedbackText.GetComponent<TMP_Text>();
        if (feedbackTextComponent != null)
        {
            feedbackTextComponent.text = message;
        }
        else
        {
            Debug.LogError("TMP_Text component missing on feedback text prefab!");
        }

        // Adjust its position to be centered
        RectTransform feedbackRectTransform = feedbackText.GetComponent<RectTransform>();
        if (feedbackRectTransform != null)
        {
            feedbackRectTransform.anchoredPosition = Vector2.zero; // Centered in the background
        }

        // Destroy the dynamic text after 1 second
        Destroy(feedbackText, 1.5f);
    }

    //private IEnumerator DeactivateBackgroundWithDelay(GameObject background, float delay)
    //{
        //yield return new WaitForSeconds(delay);
        //background.SetActive(false);
    //}
}
