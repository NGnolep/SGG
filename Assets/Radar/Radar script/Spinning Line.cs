using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class SpinningLine : MonoBehaviour
{
    public RectTransform radarLine;
    public Image myImage;
    [Range(0.1f, 60f)]  public float rotationTime = 5f;
    private float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        ChangeToGreen();
        rotationSpeed = 360f / rotationTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(radarLine != null)
        {
            radarLine.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    public void ChangeToRed()
    {
        if (myImage != null)
        {
            myImage.color = Color.red;
        }
    }

    public void ChangeToBlue()
    {
        if (myImage != null)
        {
            myImage.color = Color.blue;
        }
    }

    public void ChangeToGreen()
    {
        if (myImage != null)
        {
            myImage.color = Color.green;
        }
    }
}
