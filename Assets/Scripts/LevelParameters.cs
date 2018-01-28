using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelParams", menuName = "Parameters/LevelParameters", order = 1)]
public class LevelParameters : ScriptableObject {
	public int numBeacons;
	public int numEnemies;
	public int tileSize;
	public int mapCol;
	public int mapRows;
	public int idealNumExits;
	public float minFloorRatio; //Should be 0.4 to 0.6
	public float maxFloorRatio; //Should be 0.4 to 1
	public float scale;
	public int[] branchWeights;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
