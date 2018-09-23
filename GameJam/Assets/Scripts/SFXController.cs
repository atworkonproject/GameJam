using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOUNDS
{
    HIT,
    SELECT_BASE,
    PLACE_BUILDING,
    //BUILD_COMPLETE,
    BASE_DESTROY,

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
        SoundManager.PlaySound(SFX[(int)s]);
    }
}
