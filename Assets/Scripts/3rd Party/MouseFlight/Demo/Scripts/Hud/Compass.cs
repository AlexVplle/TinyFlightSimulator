using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public RawImage compass;
    public Transform plane;

    private void Update()
    {
        compass.uvRect = new Rect(plane.localEulerAngles.y / 360f, 0f, 1f, 1f);
    }
}
