using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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


        InfoForPlayerText0.gameObject.SetActive(false);//hide
		InfoForPlayerText1.gameObject.SetActive(false);//hide
	}

	void Update () {
		if (SceneManager.GetActiveScene().buildIndex == 1)//1 for main game scene
		{
			if (InfoForPlayerText0.gameObject.activeSelf)
				if (Time.timeSinceLevelLoad - InfoForPlayer_LastDisplayedTime0 > ConfigController.Config.InfoTextDisplayTime)
					InfoForPlayerText0.gameObject.SetActive(false);//hide
			if (InfoForPlayerText1.gameObject.activeSelf)
				if (Time.timeSinceLevelLoad - InfoForPlayer_LastDisplayedTime1 > ConfigController.Config.InfoTextDisplayTime)
					InfoForPlayerText1.gameObject.SetActive(false);//hide

			UIController.nieUzywac_CreditsTextStatic.text = "$ " + Mathf.Round(gameController.playerData.Credits).ToString();
		}
    }
}
