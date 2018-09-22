using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public Text CreditsText;
	public static Text nieUzywac_CreditsTextStatic;//pierdole robie statiki
	public static float nieUzywac_Credits;
	public static float uzywac_Credits
	{
		get
		{
			return UIController.nieUzywac_Credits;
		}
		set
		{
			UIController.nieUzywac_Credits = value;
			UIController.nieUzywac_CreditsTextStatic.text = "$ " + Mathf.Round(nieUzywac_Credits).ToString();
		}
	}

	void Start () {
		nieUzywac_CreditsTextStatic = CreditsText;
		uzywac_Credits = 100;
	}
	
	void Update () {
		float creditIncrement = 0;
		foreach (FarmBase farm in PlayerBases.PlayerFarmsStatic)
			creditIncrement += 10*Time.deltaTime;//10 for a farm
		uzywac_Credits += creditIncrement;
		Debug.Log(ConfigController.Config.maxPlayerCredits);

		uzywac_Credits = Mathf.Clamp(uzywac_Credits, 0.0f, ConfigController.Config.maxPlayerCredits);
	}
}
