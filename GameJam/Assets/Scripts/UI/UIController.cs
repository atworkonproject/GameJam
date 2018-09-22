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
			UIController.nieUzywac_CreditsTextStatic.text = "$ " + Mathf.Round(value).ToString();
		}
	}

	void Start () {
		UIController.nieUzywac_CreditsTextStatic = CreditsText;
		UIController.uzywac_Credits = 0;
	}
	
	void Update () {
		float creditIncrement = 0;
		foreach (FarmBase farm in PlayerBases.PlayerFarmsStatic)
			creditIncrement += 10*Time.deltaTime;//10 for a farm
		foreach (BarrackBase barrack in PlayerBases.PlayerBarracksStatic)
			creditIncrement -= 1 * Time.deltaTime;//1 to maintain barracks
		uzywac_Credits += creditIncrement;
		Mathf.Clamp(uzywac_Credits, 0, 200);
	}
}
