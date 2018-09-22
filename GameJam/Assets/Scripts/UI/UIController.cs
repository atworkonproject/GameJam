using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	[Header("to link")]
	public Text CreditsText;
	public Text InfoForPlayerText0;
	public Text InfoForPlayerText1;
	[Header("no need to link")]
	public float InfoForPlayer_LastDisplayedTime0;
	public float InfoForPlayer_LastDisplayedTime1;
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

	public static void DisplayInfoForPlayer0(string messageToDisplay)
	{
		UIController UIc = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<UIController>();
		UIc.InfoForPlayerText0.gameObject.SetActive(true);//show
		UIc.InfoForPlayerText0.text = messageToDisplay;
		UIc.InfoForPlayer_LastDisplayedTime0 = Time.timeSinceLevelLoad;
	}
	public static void DisplayInfoForPlayer1(string messageToDisplay)
	{
		UIController UIc = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<UIController>();
		UIc.InfoForPlayerText1.gameObject.SetActive(true);//show
		UIc.InfoForPlayerText1.text = messageToDisplay;
		UIc.InfoForPlayer_LastDisplayedTime1 = Time.timeSinceLevelLoad;
	}

	void Start () {
		nieUzywac_CreditsTextStatic = CreditsText;
		uzywac_Credits = ConfigController.Config.startPlayerCredits;

		InfoForPlayerText0.gameObject.SetActive(false);//hide
		InfoForPlayerText1.gameObject.SetActive(false);//hide
	}

	void Update () {
		float creditIncrement = 0;
		foreach (FarmBase farm in PlayerBases.PlayerFarmsStatic)
			creditIncrement += ConfigController.Config.FarmEarnPerSecond * Time.deltaTime;
		uzywac_Credits += creditIncrement;

		uzywac_Credits = Mathf.Clamp(uzywac_Credits, 0.0f, ConfigController.Config.maxPlayerCredits);

		if (InfoForPlayerText0.gameObject.activeSelf)
			if (Time.timeSinceLevelLoad - InfoForPlayer_LastDisplayedTime0 > ConfigController.Config.InfoTextDisplayTime)
				InfoForPlayerText0.gameObject.SetActive(false);//hide
		if (InfoForPlayerText1.gameObject.activeSelf)
			if (Time.timeSinceLevelLoad - InfoForPlayer_LastDisplayedTime1 > ConfigController.Config.InfoTextDisplayTime)
				InfoForPlayerText1.gameObject.SetActive(false);//hide
	}
}
