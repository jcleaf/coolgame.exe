﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
public class Beacon : MonoBehaviour
{
	[SerializeField] private bool _activated;
    [SerializeField] private Sprite activatedSprite;
    [SerializeField] private Renderer[] activatedVfx;
    [SerializeField] private IntReference beaconsLit;

	private AudioSource src;
    public bool activated { get { return _activated; } }

    private SpriteRenderer spriteRenderer;

	void Awake ()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		src = GetComponent<AudioSource> ();

        foreach (Renderer r in activatedVfx)
        {
            r.enabled = false;
        }
	}

    void OnTriggerEnter(Collider col)
    {
        TryUpdateEnemy(col);
        //TryAddInstructions();
    }

    void OnTriggerStay(Collider col)
    {
        TryUpdateEnemy(col);
    }

    private void OnTriggerExit(Collider other)
    {
        //TryRemoveInstructions();
    }

    private void TryUpdateEnemy(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && _activated)
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            enemy.playerDetected = true;
            // The enemy will first check the beacon, so if they were on the other side of a wall they'll pathfind to get to the right place
            enemy.lastPlayerSighting = transform.position;
        }
    }

    public void Activate()
    {
		if (!_activated) {
			src.Play ();
            _activated = true;
            spriteRenderer.sprite = activatedSprite;
            beaconsLit.value += 1;

            foreach (Renderer r in activatedVfx) {
                r.enabled = true;
            }
		}

    }
}
