using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Progression : MonoBehaviour {

	public LevelParameters[] levelParamsArray;
	public IntReference levelNum;
	public LevelParameters currentlevel;

	// Use this for initialization
	void Awake () {
		levelNum.value = 0;
		updateLevelParams (levelParamsArray [levelNum.value]);
		DontDestroyOnLoad (transform.gameObject);
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
	void LoadLevel(){
		if (levelNum > levelParamsArray.Length) {
			currentlevel.numEnemies += 10;
			currentlevel.numBeacons += 1;

		} else {
			updateLevelParams (levelParamsArray [levelNum.value]);
		}
		SceneManager.LoadScene ("Main.unity");
	}
}
