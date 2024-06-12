using System.Globalization;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
public class Compass : MonoBehaviour
{
	[SerializeField] private RawImage CompassImage;
	[SerializeField] private TextMeshProUGUI CompassDirectionText;
    
	private Transform Player;
	
	private void Start()
	{
		Player = PlayerManager.playerReference.transform;
	}

	private void Update()
	{
		//Get a handle on the Image's uvRect
		CompassImage.uvRect = new Rect(Player.localEulerAngles.y / 360, 0, 1, 1);

		// Get a copy of your forward vector
		Vector3 forward = Player.transform.forward;

		// Zero out the y component of your forward vector to only get the direction in the X,Z plane
		forward.y = 0;

		//Clamp our angles to only 5 degree increments
		float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
		headingAngle = 5 * (Mathf.RoundToInt(headingAngle / 5.0f));

		//Convert float to int for switch
		int displayangle;
		displayangle = Mathf.RoundToInt(headingAngle);

		//Set the text of Compass Degree Text to the clamped value, but change it to the letter if it is a True direction
		CompassDirectionText.text = displayangle switch
		{
			0 => "N",
			360 => "N",
			45 => "NE",
			90 => "E",
			130 => "SE",
			180 => "S",
			225 => "SW",
			270 => "W",
			_ => headingAngle.ToString(CultureInfo.InvariantCulture)
		};
	}
}
