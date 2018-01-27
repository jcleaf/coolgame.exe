using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Represents one level, so like, a map, a player, all the enemies and the
 * beacons, and the current state of the level.
 */
public class Level : MonoBehaviour
{
    public Transform playerPrefab;
    public Transform enemyPrefab;
    public Transform beaconPrefab;

    public int numEnemies;
    public int numBeacons;

    public Transform player;
    public List<Transform> enemies;
	public List<Beacon> beacons;

	public bool levelFinished;

	// Use this for initialization
	void Start()
	{
		levelFinished = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (BeaconsAreLitGondorCallsForAid ()) {
			levelFinished = true;
		}
	}

	bool BeaconsAreLitGondorCallsForAid(){
		foreach (var beacon in beacons) {
			if (!beacon.activated) {
                return false;
			}
		}
		return true;
	}

	void openAirlock(){
	
	}

    public void SpawnEntities(List<Transform> spawnPoints)
    {
        player = Instantiate(playerPrefab, spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);

        for (int i = 0; i < numEnemies; i++)
        {
            int index = Random.Range(0, spawnPoints.Count);
            Transform enemy = Instantiate(enemyPrefab, spawnPoints[index].position, Quaternion.identity);
            enemies.Add(enemy);
        }

        for (int i = 0; i < numBeacons; i++)
        {
            int index = Random.Range(0, spawnPoints.Count);
            Transform beacon = Instantiate(beaconPrefab, spawnPoints[index].position, Quaternion.identity);
            enemies.Add(beacon);
        }
    }

    public void DestroyEnemeies()
    {

    }
}
