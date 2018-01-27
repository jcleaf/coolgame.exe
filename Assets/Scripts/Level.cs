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
        Vector3 heightOffset = 1f * Vector3.up;
        player = Instantiate(
            playerPrefab,
            spawnPoints[Random.Range(0, spawnPoints.Count)].position + heightOffset + GetRandomOffset(),
            Quaternion.identity);

        for (int i = 0; i < numEnemies; i++)
        {
            int index = Random.Range(0, spawnPoints.Count);
            Transform enemy = Instantiate(
                enemyPrefab,
                spawnPoints[index].position + heightOffset + GetRandomOffset(),
                Quaternion.identity);
            enemies.Add(enemy);
        }

        for (int i = 0; i < numBeacons; i++)
        {
            int index = Random.Range(0, spawnPoints.Count);
            Transform beacon = Instantiate(
                beaconPrefab,
                spawnPoints[index].position + heightOffset + GetRandomOffset(),
                Quaternion.identity);
            enemies.Add(beacon);
        }
    }

    /**
     * Some 2D random offset, just for moving along the plane
     */
    private Vector3 GetRandomOffset() {
        Vector2 randomOffset2d = Random.insideUnitCircle;
        Vector3 randomOffset = new Vector3(randomOffset2d.x, 0, randomOffset2d.y);
        return randomOffset;
    }

    public void DestroyEnemeies()
    {

    }
}
