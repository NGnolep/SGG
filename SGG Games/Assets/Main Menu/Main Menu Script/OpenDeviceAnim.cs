using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDeviceAnim : MonoBehaviour
{
    public Animator animator;
    public MainMenuBlinker mainMen;
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playAnim()
    {
        animator.Play("OpenDeviceAnim");
        StartCoroutine(WaitForAnimationToComplete(animator, () => mainMen.device.SetActive(false)));
    }

    private IEnumerator WaitForAnimationToComplete(Animator animator, System.Action callback)
    {
        if (animator == null) yield break;

        // Wait until the animation is complete
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        // Execute callback after animation is don
        
        callback?.Invoke();
        mainMen.mainMenu.SetActive(true);
    }
}
