using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimGo : MonoBehaviour, IPointerEnterHandler
{
    
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered!");
        animator.SetTrigger("Hover");
    }

    public void OnClick()
    {
        // Play the pop animation
        animator.SetTrigger("Click");

        // Wait 2 seconds, then play the regenerate animation
        Invoke(nameof(TriggerRegenerate), 2f);
    }

    private void TriggerRegenerate()
    {
        animator.SetTrigger("Regenerate");
    }
}
