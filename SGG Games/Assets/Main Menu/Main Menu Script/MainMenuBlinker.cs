using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBlinker : MonoBehaviour
{
    public float blinkSpeed = 1f;

    public GameObject startText;
    public GameObject mainMenu;
    public GameObject startMenu;

    public Button start;
    public Button tutorial;
    public Button setting;
    public Button quit;

    private CanvasGroup textCanvasGroup;
    private bool isBlinking = true;
    // Start is called before the first frame update
    void Start()
    {
        if(startMenu != null)
        {
            startMenu.SetActive(true);
        }
        if (mainMenu != null)
        {
            mainMenu.SetActive(false);
        }

        textCanvasGroup = startText.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null)
        {
            textCanvasGroup = startText.AddComponent<CanvasGroup>();
        }

        StartCoroutine(BlinkText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnScreenClicked();
        }
    }

    private IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            for (float t = 0; t < 1; t += Time.deltaTime * blinkSpeed)
            {
                textCanvasGroup.alpha = Mathf.Lerp(1, 0, t);
                yield return null;
            }

            for (float t = 0; t < 1; t += Time.deltaTime * blinkSpeed)
            {
                textCanvasGroup.alpha = Mathf.Lerp(0, 1, t);
                yield return null;
            }
        }
    }

    private void OnScreenClicked()
    {
        isBlinking = false;

        textCanvasGroup.alpha = 1;

        if(mainMenu != null)
        {
            mainMenu.SetActive(true);
        }

        if(startText != null)
        {
            startMenu.SetActive(false);
        }
    }
}
