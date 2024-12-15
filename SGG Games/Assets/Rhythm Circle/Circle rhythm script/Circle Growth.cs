using UnityEngine;
using System.Collections;

public class CircleGrowth : MonoBehaviour
{
    [Header("Circle Reference")]
    public Transform innerCircle;
    public Transform outerCircle;

    [Header("Circle Settings")]
    public float growSpeed = 0.8f;
    public float perfectRange = 0.29f;
    public float maxScaleThreshold = 1.1f;

    [Header("Scoring")]
    public int scanned = 0;
    public int missScan = 0;

    private bool hasScored = false;
    private bool canScore = false;
    private bool hasMissed = false;
    private bool isGameOver = false;
    private bool isActive = false; // To control when the script starts/stops

    [Header("Feedback")]
    public VisualFeedback visualFeedback;
    private SpriteRenderer innerCircleRenderer;
    public float fadeInDuration = 1f;

    public RadarController scanningManager;
    public EncyclopediaManager encyclopediaManager;

    void Start()
    {
        innerCircleRenderer = innerCircle.GetComponent<SpriteRenderer>();
        ResetCircle();
        StopAllCoroutines(); // Prevent anything from starting until explicitly told to
    }

    void Update()
    {
        if (!isActive || isGameOver) return; // Ensure the script only runs when active

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
        if (isActive) return; // Prevent multiple starts

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
        ResetCircle(); // Reset the circle when stopping
    }

    private void CheckForScore()
    {
        float distance = Mathf.Abs(innerCircle.localScale.x - outerCircle.localScale.x);
        if (distance <= perfectRange)
        {
            scanned++;
            hasScored = true;
            Debug.Log("Perfect");
            if (scanned >= 10)
            {
                scanningManager.RhythmComplete();
                StopRhythm(); // Stop the rhythm game when the goal is reached
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
        Debug.Log(message);

        ResetCircle();
        visualFeedback.ShowMiss();

        if (missScan >= 5)
        {
            isGameOver = true;
            scanningManager.OnMissedScan();
            StopRhythm(); // Stop the rhythm game when too many misses occur
        }
    }

    void ResetCircle()
    {
        StopAllCoroutines();

        if (innerCircleRenderer != null)
        {
            Color resetColor = innerCircleRenderer.color;
            resetColor.a = 1; // Fully transparent
            innerCircleRenderer.color = resetColor;
        }

        innerCircle.localScale = Vector3.one * 0.1f;

        canScore = false;
        hasMissed = false;
        hasScored = false;
        visualFeedback.outerRenderer.material.color = visualFeedback.normalColor;
        StartCoroutine(StartGrowthDelay());
    }
    IEnumerator StartGrowthDelay()
    {
        yield return new WaitForSeconds(0.5f); // Small delay before resuming growth
        canScore = true; // Allow scoring again
    }
    IEnumerator FadeInInnerCircle()
    {
        Color startColor = innerCircleRenderer.color;
        startColor.a = 0; // Start fully transparent
        innerCircleRenderer.color = startColor;

        Color targetColor = startColor;
        targetColor.a = 1; // Fully visible
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            innerCircleRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeInDuration);
            yield return null;
        }

        innerCircleRenderer.color = targetColor; // Ensure it's fully visible
    }
}
