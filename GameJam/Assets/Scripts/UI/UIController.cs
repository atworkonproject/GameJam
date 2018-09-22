using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public Text CreditsText;
	public static Text nieUzywac_CreditsTextStatic;//pierdole robie statiki
	public static float nieUzywac_Credits;
	public static float player_Credits
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
    public static float AI_Credits;

	void Start () {
		nieUzywac_CreditsTextStatic = CreditsText;
		player_Credits = ConfigController.Config.maxPlayerCredits;
        AI_Credits = ConfigController.Config.maxPlayerCredits;
    }
	
	void Update () {
		float creditIncrement = 0;
		foreach (FarmBase farm in Bases.PlayerFarmsStatic)
			creditIncrement += 10*Time.deltaTime;//10 for a farm
		player_Credits += creditIncrement;
        player_Credits = Mathf.Clamp(player_Credits, 0.0f, ConfigController.Config.maxPlayerCredits);

        creditIncrement = 0;
        foreach (FarmBase farm in Bases.OpponentFarmsStatic)
            creditIncrement += 10 * Time.deltaTime;//10 for a farm
        AI_Credits += creditIncrement;
        AI_Credits = Mathf.Clamp(AI_Credits, 0.0f, ConfigController.Config.maxPlayerCredits);
    }
}
