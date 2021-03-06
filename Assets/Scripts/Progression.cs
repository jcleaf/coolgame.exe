﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Progression : MonoBehaviour {

	public LevelParameters[] levelParamsArray;
	public IntReference levelNum;
	public LevelParameters currentlevel;
	private static Progression instanceRef;
	public MusicLoop _musicloop;
	// Use this for initialization
	void Awake () {
		if(instanceRef == null)
		{
			instanceRef = this;
			DontDestroyOnLoad(gameObject);
			levelNum.value = 0;
		}else
		{
			DestroyImmediate(gameObject);
		}
		GameObject music = GameObject.Find("Audio Source");
		_musicloop = music.GetComponent<MusicLoop> ();
		updateLevelParams (levelParamsArray [levelNum.value]);

	}
		
	// Update is called once per frame
	void Update () {
		
	}

	void updateLevelParams(LevelParameters level){
		currentlevel.branchWeights = level.branchWeights;
		currentlevel.idealNumExits = level.idealNumExits;
		currentlevel.mapCol = level.mapCol;
		currentlevel.mapRows = level.mapRows;
		currentlevel.maxFloorRatio = level.maxFloorRatio;
		currentlevel.minFloorRatio = level.minFloorRatio;
		currentlevel.numBeacons = level.numBeacons;
		currentlevel.numEnemies = level.numEnemies;
		currentlevel.scale = level.scale;
		currentlevel.tileSize = level.tileSize;
	}

	public void NextLevel(){
		levelNum.value += 1;
		LoadLevel ();
    }
	public void Reset(){
		levelNum.value = 0;
		LoadLevel ();
	}
	public void RestartLevel(){
		LoadLevel ();
	}
    void LoadLevel()
    {
        if (levelNum > levelParamsArray.Length)
        {
            currentlevel.numEnemies += 10;
            currentlevel.numBeacons += 1;

        }
        else
        {
            updateLevelParams(levelParamsArray[levelNum.value]);
        }

        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
		_musicloop.otherSoundPlaying = false;
    }

    }
