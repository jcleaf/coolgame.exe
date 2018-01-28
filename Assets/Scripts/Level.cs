using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/**
 * Represents one level, so like, a map, a player, all the enemies and the
 * beacons, and the current state of the level.
 */
public class Level : MonoBehaviour
{
    private const string UI_SCENE_NAME = "UIScene";

    public Transform playerPrefab;
    public Transform enemyPrefab;
    public Beacon beaconPrefab;

    public int numEnemies;
    public int numBeacons;

    public BoolReference beaconsLitRef;

    public Transform player;
    public List<Transform> enemies;
	public List<Beacon> beacons;
	private AudioSource winaudio;
	public bool levelFinished;

	public Progression progress;

    void Awake()
    {
		numBeacons = progress.currentlevel.numBeacons;
		numEnemies = progress.currentlevel.numEnemies;
		winaudio = GetComponent<AudioSource> ();
        SceneManager.LoadScene(UI_SCENE_NAME, LoadSceneMode.Additive);

        beaconsLitRef.value = false;
    }

	// Update is called once per frame
	void Update()
	{
		bool wasFinished = levelFinished;
        levelFinished = BeaconsAreLitGondorCallsForAid();
        beaconsLitRef.value = levelFinished;
		if (wasFinished != levelFinished) {
			winaudio.Play ();
		}
    }

	bool BeaconsAreLitGondorCallsForAid()
    {
        if (beacons == null || beacons.Count == 0)
        {
            return false;
        }

		foreach(Beacon beacon in beacons)
        {
			if (!beacon.activated)
            {
                return false;
			}
		}

		return true;
	}

    public void SpawnEntities(List<Transform> spawnPoints)
    {
        Vector3 heightOffset = 1f * Vector3.up;

        Transform entities = (new GameObject("Entities")).transform;
        player = Instantiate(
            playerPrefab,
            spawnPoints[Random.Range(0, spawnPoints.Count)].position + heightOffset + GetRandomOffset(),
            Quaternion.identity,
            entities);

        Transform enemyParent = (new GameObject("Enemies")).transform;
        enemyParent.parent = entities;
        for (int i = 0; i < numEnemies; i++)
        {
            int index = Random.Range(0, spawnPoints.Count);
            Transform enemy = Instantiate(
                enemyPrefab,
                spawnPoints[index].position + heightOffset + GetRandomOffset(),
                Quaternion.identity,
                enemyParent);
            enemies.Add(enemy);
        }


        Transform beaconParent = (new GameObject("Beacons")).transform;
        beaconParent.parent = entities;
        for (int i = 0; i < numBeacons; i++)
        {
            int index = Random.Range(0, spawnPoints.Count);
            Beacon beacon = Instantiate<Beacon>(
                beaconPrefab,
                spawnPoints[index].position + heightOffset + GetRandomOffset(),
                Quaternion.identity,
                beaconParent);
            beacons.Add(beacon);
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
