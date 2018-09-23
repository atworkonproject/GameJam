using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOUNDS
{
    HIT,
    SELECT_BASE,
    PLACE_BUILDING,
    BUILD_COMPLETE,
    BASE_DESTROY,
    DIE_ANGEL,
    DIE_DEVIL,
    SPAWN,

    _COUNT
}

public class SFXController : MonoBehaviour {

    public static AudioClip[] SFX;
    public AudioClip[] SFXforInspector;

	void Start () {
        SFX = SFXforInspector;
    }
	
	void Update () {
		
	}

    static public void PlaySound(SOUNDS s)
    {
        if(s == SOUNDS.SPAWN)
            SoundManager.PlaySound(SFX[(int)s], 0.5f, false, null, Random.Range(0.95f, 1.07f));
        else if (s == SOUNDS.HIT)
            SoundManager.PlaySound(Random.Range(0.8f, 1.1f), SFX[(int)s]);
        else
            SoundManager.PlaySound(SFX[(int)s]);
    }
}
