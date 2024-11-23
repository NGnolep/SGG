using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualFeedback : MonoBehaviour
{
    public Transform inner;
    public Transform outer;
    public SpriteRenderer outerRenderer;

    public Color normalColor = Color.white;
    public Color hitColor = Color.green;
    public Color missColor = Color.red;
    public float hitRange = 0.5f;

    private bool isMiss = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMiss) return;

        float distance = Mathf.Abs(inner.localScale.x - outer.localScale.x);

        if(distance < hitRange)
        {
            outerRenderer.material.color = hitColor;
        }
        else
        {
            outerRenderer.material.color = normalColor;
        }
    }

    public void ShowMiss()
    {
        isMiss = true;
        Debug.Log("ShowMiss triggered!");
        outerRenderer.material.color = missColor;

        StartCoroutine(ResetMiss());
    }
    private IEnumerator ResetMiss()
    {
        yield return new WaitForSeconds(0.1f); // Adjust delay as needed
        isMiss = false;
    }
}
