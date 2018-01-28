using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
public class Beacon : MonoBehaviour {
	public bool activated;
	Enemy enemy;
	// Use this for initialization
	void Start () {
		activated = false;
		enemy = null;
	}

    void OnTriggerEnter(Collider col){
        TryUpdateEnemy(col);

    }

    void OnTriggerStay(Collider col)
    {
        TryUpdateEnemy(col);
    }

    private void TryUpdateEnemy(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && activated)
        {
            enemy = col.gameObject.GetComponent<Enemy>();
            enemy.playerDetected = true;
        }
    }
}
