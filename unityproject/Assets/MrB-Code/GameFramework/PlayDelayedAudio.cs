using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDelayedAudio : MonoBehaviour
{
    public AudioSource clip;
    public float delayInSeconds;

	// Use this for initialization
	void Awake () 
    {
        clip.PlayDelayed(delayInSeconds);
    }
}
