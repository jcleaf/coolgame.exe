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

    public Transform player;
    public List<Transform> enemies;
    public List<Transform> beacons;

	// Use this for initialization
	void Start()
	{
        player = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        enemies = new List<Transform>();
        for (int i = 0; i < 10; i++)
        {
            Vector2 pos = 10 * Random.insideUnitCircle;
            Transform enemy = Instantiate(enemyPrefab, new Vector3(pos.x, 1, pos.y), Quaternion.identity);
            enemies.Add(enemy);
        }
        beacons = new List<Transform>();
        for (int i = 0; i < 10; i++)
        {
            Vector2 pos = 10 * Random.insideUnitCircle;
            Transform beacon = Instantiate(enemyPrefab, new Vector3(pos.x, 1, pos.y), Quaternion.identity);
            beacons.Add(beacon);
        }
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
