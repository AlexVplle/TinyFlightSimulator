using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class AltitudeTracker : MonoBehaviour
{
    public Transform plane;
    public TextMeshProUGUI altitudeText;

    private void Update()
    {
        if (plane == null || altitudeText == null)
        {
            Debug.LogError("Veuillez d�finir la r�f�rence de l'avion et du texte d'altitude dans l'inspecteur.");
            return;
        }

        float altitude = plane.position.y;

        altitudeText.text =  altitude.ToString("F2");
    }
}
