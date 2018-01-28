using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour {

	private static MusicLoop instanceRef;
	public AudioSource Src;
	int Step;
	public float maxTime;
	public float minTime;
	public bool otherSoundPlaying;
	public float startVol;
	// Use this for initialization
	void Start () {
		if(instanceRef == null)
		{
			instanceRef = this;
			DontDestroyOnLoad(gameObject);
			Src = GetComponent<AudioSource> ();
			otherSoundPlaying = false;
			startVol = 0.54f;
		}else
		{
			DestroyImmediate(gameObject);
		}


	}


	private void Update(){
		if (otherSoundPlaying) {
			Src.volume = startVol * 0.2f;
		} else {
			Src.volume = startVol;
		}
	}

	private void LateUpdate()
	{
		
		if (Src.time >= maxTime)
		{
			Src.time = minTime;
			Src.Play();
		}


	}
}
