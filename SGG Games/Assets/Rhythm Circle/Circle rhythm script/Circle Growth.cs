using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Feedback")]
    public VisualFeedback visualFeedback;
    private SpriteRenderer innerCircleRenderer; 
    public float fadeInDuration = 1f;

    public RadarController scanningManager;
    void Start()
    {
        innerCircleRenderer = innerCircle.GetComponent<SpriteRenderer>();
        if (innerCircleRenderer != null)
        {
            // Start with the inner circle fully transparent
            Color startColor = innerCircleRenderer.color;
            startColor.a = 0;
            innerCircleRenderer.color = startColor;

            // Start the fade-in coroutine
            StartCoroutine(FadeInInnerCircle());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (innerCircle.localScale.x < outerCircle.localScale.x)
        {
            innerCircle.localScale += Vector3.one * growSpeed * Time.deltaTime;
            canScore = true;
        }
        else if (!hasMissed && !hasScored)
        {
            HandleMiss("Too Slow");
        }

        if(Input.GetKeyDown(KeyCode.Space) && canScore && !hasMissed)
        {
            CheckForScore();
            
        }
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
                scanningManager.CollectFishData(); 
                scanned = 0; 
            }
        }
        else
        {
            HandleMiss("Too Fast!");
        }

        StartCoroutine(DelayedResetCircle());
    }

    private void HandleMiss(string message)
    {
        missScan++;
        hasMissed = true;
        Debug.Log(message);
        if (missScan >= 5)
        {
            scanningManager.OnMissedScan(); 
            missScan = 0; 
        }

        visualFeedback.ShowMiss(); 
        StartCoroutine(DelayedResetCircle());
    }
    IEnumerator DelayedResetCircle()
    {
        yield return new WaitForSeconds(0.1f); // Wait before resetting, adjust delay as needed
        ResetCircle();
    }
    void ResetCircle()
    {
        StopAllCoroutines();

        if (innerCircleRenderer != null)
        {
            Color resetColor = innerCircleRenderer.color;
            resetColor.a = 0; // Fully transparent
            innerCircleRenderer.color = resetColor;
        }

        innerCircle.localScale = Vector3.one * 0.1f;
        canScore = false;
        hasMissed = false;
        hasScored = false;
        visualFeedback.outerRenderer.material.color = visualFeedback.normalColor;

        StartCoroutine(FadeInInnerCircle());
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

        // Ensure the final alpha is fully visible
        innerCircleRenderer.color = targetColor;
    }
}
