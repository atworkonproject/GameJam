using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	[Header("to link")]
	public Text CreditsText;
	public Text InfoForPlayerText0;//player = text 0
    public Text InfoForPlayerText1;//ai = text I
	public Text BuyBarracks01PriceText;
    public Text BuyBarracks03PriceText;
    public Text BuyFarmPriceText;
	[Header("no need to link")]
	public float InfoForPlayer_LastDisplayedTime0;
	public float InfoForPlayer_LastDisplayedTime1;
	public static Text nieUzywac_CreditsTextStatic;//pierdole robie statiki

    public static void DisplayUserInfo(string messageToDisplay, UserData user)
	{
		UIController UIc = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<UIController>();
        if(user.amIPlayer)
        {
            UIc.InfoForPlayerText0.gameObject.SetActive(true);//show
            UIc.InfoForPlayerText0.text = "[Player] " + messageToDisplay;
        }
        else
        {
            UIc.InfoForPlayerText1.gameObject.SetActive(true);//show
            UIc.InfoForPlayerText1.text = "[AI] " + messageToDisplay;
        }
		UIc.InfoForPlayer_LastDisplayedTime0 = Time.timeSinceLevelLoad;
	}

	void Start () {
		nieUzywac_CreditsTextStatic = CreditsText;

		BuyBarracks01PriceText.text = Mathf.RoundToInt(ConfigController.Config.Barracks01BuyCost).ToString();
        BuyBarracks03PriceText.text = Mathf.RoundToInt(ConfigController.Config.Barracks03BuyCost).ToString();
        BuyFarmPriceText.text = Mathf.RoundToInt(ConfigController.Config.FarmBuyCost).ToString();

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

			UIController.nieUzywac_CreditsTextStatic.text = Mathf.Round(gameController.playerData.Credits).ToString();
		}
    }
}
