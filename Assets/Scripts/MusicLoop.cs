using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour {

	public AudioSource Src;
	int Step;
	public float maxTime;
	public float minTime;
	// Use this for initialization
	void Start () {
		Src = GetComponent<AudioSource> ();
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
