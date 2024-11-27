using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningLine : MonoBehaviour
{
    public RectTransform radarLine;
    [Range(0.1f, 60f)]  public float rotationTime = 5f;
    private float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
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
}
