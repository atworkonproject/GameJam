using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFXController : MonoBehaviour
{
    [Header("to link")]
    public AudioClip[] BFX_DemonClips;
    public AudioClip[] BFX_AngelClips;

    private bool isBGPlaying;
    private AudioClip[] CurrentPlaylist;
    int currIDx;
    // Use this for initialization
    void Start()
    {
        isBGPlaying = false;
        currIDx = 0;

        RestartAudio();
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameController.playerData.fallen)
        //    if(!SoundManager.GetMusicAudio(BFX_DemonClips[currIDx % BFX_DemonClips.Length]).audioSource.isPlaying)
        //        PlayNextBackgroundClip();
        //else
        //    if (!SoundManager.GetMusicAudio(BFX_AngelClips[currIDx % BFX_AngelClips.Length]).audioSource.isPlaying)
        //        PlayNextBackgroundClip();
    }

    public void RestartAudio()
    {
        SoundManager.StopAllMusic();
        if (gameController.playerData.fallen)
        {
            currIDx = UnityEngine.Random.Range(0, BFX_DemonClips.Length);
            SoundManager.PlayMusic(BFX_DemonClips[currIDx], 1, true, false);
        }
        else
        {
            currIDx = UnityEngine.Random.Range(0, BFX_AngelClips.Length);
            SoundManager.PlayMusic(BFX_AngelClips[currIDx], 1, true, false);
        }
    }

    public void PlayNextBackgroundClip()
    {
        currIDx++;
        if (gameController.playerData.fallen)
            SoundManager.PlayMusic(BFX_DemonClips[currIDx % BFX_DemonClips.Length], 1, true, false);
        else
            SoundManager.PlayMusic(BFX_AngelClips[currIDx % BFX_AngelClips.Length], 1, true, false);
    }
}
