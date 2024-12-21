using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverAnimKP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;
    public TMP_Text hoverText;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered!");
        animator.SetTrigger("Hover");

        ShowHoverText("The Kelp Forest");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetTrigger("Idle"); // Replace with your actual idle state trigger

        HideHoverText();
    }

    public void OnClick()
    {
        // Play the pop animation
        animator.SetTrigger("Click");

        // Wait 2 seconds, then play the regenerate animation
        Invoke(nameof(TriggerRegenerate), 0.6f);
    }

    private void TriggerRegenerate()
    {
        animator.SetTrigger("Regenerate");
    }

    private void ShowHoverText(string text)
    {
        hoverText.text = text;
        hoverText.gameObject.SetActive(true);
    }

    private void HideHoverText()
    {
        hoverText.gameObject.SetActive(false);
    }
}
