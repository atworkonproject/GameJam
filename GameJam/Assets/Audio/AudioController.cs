using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
	[Header("to link")]
	public Transform SFXDamage;
	public Transform BFX_Demon;
	public Transform BFX_Angel;
	[Header("others")]
	public AudioSource[] BFX_DemonClips;
	public AudioSource[] BFX_AngelClips;
	public AudioSource SFXDamageClip;

	public AudioSource CurrentClipBFX;
	public AudioSource[] CurrentPlaylist;
	public int CurrentClipIndex;
	// Use this for initialization
	void Start () {
		BFX_DemonClips = BFX_Demon.GetComponentsInChildren<AudioSource>();
		BFX_AngelClips = BFX_Angel.GetComponentsInChildren<AudioSource>();
		SFXDamageClip = SFXDamage.GetComponent<AudioSource>();
		RestartAudio();
	}

	// Update is called once per frame
	void Update () {
		if (!CurrentClipBFX.isPlaying)
		{
			PlayNextBackgroundClip();
		}
	}

	public void RestartAudio()
	{
		if (gameController.playerData.fallen)
			CurrentPlaylist = BFX_DemonClips;
		else
			CurrentPlaylist = BFX_AngelClips;
		CurrentClipIndex = UnityEngine.Random.Range(0, CurrentPlaylist.Length);
		PlayNextBackgroundClip();
	}

	public void PlayNextBackgroundClip()
	{
		CurrentClipIndex++;
		if (CurrentClipIndex >= CurrentPlaylist.Length)
			CurrentClipIndex = 0;
		CurrentClipBFX = CurrentPlaylist[CurrentClipIndex];
		EazyTools.SoundManager.SoundManager.PlayMusic(CurrentClipBFX.clip);
	}

	public static void PlayDamageSound()
	{
		GameObject go = GameObject.FindWithTag("_SCRIPTS_");
		AudioController ac = go.GetComponentInChildren<AudioController>();
		EazyTools.SoundManager.SoundManager.PlaySound(ac.SFXDamageClip.clip);
	}
}
