using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    [Header("UI References")]
    public Image lineImage;           // Line to animate
    public TMP_Text hoverText;        // Text to display above the line

    public float lineAnimationSpeed = 2f;  // Speed of the line animation
    private bool isAnimating = false;

    private Coroutine currentAnimation;    // Store current animation to stop it if needed

    private void Start()
    {
        // Ensure line and text are hidden at the start
        lineImage.enabled = false;
        hoverText.enabled = false;
    }

    public void OnButtonHover(Button button, string displayText)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation); // Stop the previous animation
        }

        currentAnimation = StartCoroutine(AnimateLine(button, displayText));
    }

    public void OnButtonExit()
    {
        // Stop animation and reset UI
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        isAnimating = false;
        lineImage.enabled = false;
        hoverText.enabled = false;

        RectTransform lineRect = lineImage.rectTransform;
        lineRect.sizeDelta = new Vector2(0, lineRect.sizeDelta.y);
    }

    private IEnumerator AnimateLine(Button button, string displayText)
    {
        isAnimating = true;

        // Enable UI elements
        lineImage.enabled = true;
        hoverText.enabled = true;
        hoverText.text = displayText;

        RectTransform buttonRect = button.GetComponent<RectTransform>();
        RectTransform lineRect = lineImage.rectTransform;

        // Start line position from bottom of button
        Vector3 startPos = new Vector3(buttonRect.position.x, buttonRect.position.y - buttonRect.rect.height / 2, 0);
        lineRect.position = startPos;

        float targetWidth = Screen.width; // Full screen width
        float currentWidth = 0;

        while (currentWidth < targetWidth)
        {
            currentWidth += lineAnimationSpeed * Time.deltaTime * targetWidth;
            lineRect.sizeDelta = new Vector2(currentWidth, lineRect.sizeDelta.y);

            // Update hover text position above the line
            hoverText.rectTransform.position = new Vector3(startPos.x + currentWidth / 2, startPos.y + 20, 0);

            yield return null;
        }

        // Ensure it reaches the full width
        lineRect.sizeDelta = new Vector2(targetWidth, lineRect.sizeDelta.y);
    }
}
