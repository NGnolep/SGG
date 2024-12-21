using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    public Image lineImage;           // The line that animates
    public TMP_Text hoverText;        // Text to display above the line

    public string displayText;        // Customizable hover text for the button
    public float lineAnimationSpeed = 2f;

    private Coroutine currentAnimation;
    private bool isAnimating = false;

    private RectTransform buttonRect;

    private void Start()
    {
        // Ensure line and text are hidden at the start
        lineImage.enabled = false;
        hoverText.enabled = false;

        // Cache the button RectTransform
        buttonRect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isAnimating)
        {
            StartHover(displayText);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopHover();
    }

    private void StartHover(string text)
    {
        if (currentAnimation != null) StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(AnimateLine(text));
    }

    private void StopHover()
    {
        if (currentAnimation != null) StopCoroutine(currentAnimation);

        isAnimating = false;
        lineImage.enabled = false;
        hoverText.enabled = false;

        // Reset line size
        lineImage.rectTransform.sizeDelta = new Vector2(0, lineImage.rectTransform.sizeDelta.y);
    }

    private IEnumerator AnimateLine(string text)
    {
        isAnimating = true;

        // Enable UI elements
        lineImage.enabled = true;
        hoverText.enabled = true;
        hoverText.text = text;

        RectTransform lineRect = lineImage.rectTransform;

        // Anchor the line to the bottom-left (left-to-right animation)
        lineRect.anchorMin = new Vector2(0, 0.8f); // Start at the left edge
        lineRect.anchorMax = new Vector2(0, 0.8f);
        lineRect.pivot = new Vector2(0, 0.8f);

        // Position the line below the button
        Vector2 buttonPosition = buttonRect.anchoredPosition;
        float buttonHeight = buttonRect.rect.height;
        lineRect.anchoredPosition = new Vector2(buttonPosition.x, buttonPosition.y - buttonHeight / 2 - 10); // Offset below the button

        // Target width: Go beyond the button width to stretch further
        Canvas canvas = lineImage.canvas;
        float targetWidth = canvas.pixelRect.width * 1.5f; // 1.5x the canvas width for extra stretch

        float currentWidth = 0;

        // Animate line width from 0 to targetWidth
        while (currentWidth < targetWidth)
        {
            currentWidth += lineAnimationSpeed * Time.deltaTime * targetWidth;
            lineRect.sizeDelta = new Vector2(currentWidth, lineRect.sizeDelta.y);

            // Position the hover text at the center of the line
            hoverText.rectTransform.anchoredPosition = new Vector2(currentWidth / 8, lineRect.anchoredPosition.y + 40);

            yield return null;
        }

        lineRect.sizeDelta = new Vector2(targetWidth, lineRect.sizeDelta.y);
        isAnimating = false;
    }
}
