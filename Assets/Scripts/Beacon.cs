using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
public class Beacon : MonoBehaviour
{
	[SerializeField] private bool _activated;
    [SerializeField] private Sprite activatedSprite;
    [SerializeField] private SpriteRenderer ring;

	private AudioSource src;
    public bool activated { get { return _activated; } }

    private SpriteRenderer spriteRenderer;

	void Awake ()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		src = GetComponent<AudioSource> ();
	}

    void OnTriggerEnter(Collider col)
    {
        TryUpdateEnemy(col);
    }

    void OnTriggerStay(Collider col)
    {
        TryUpdateEnemy(col);
    }

    private void TryUpdateEnemy(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && _activated)
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            enemy.playerDetected = true;
        }
    }

    public void Activate()
    {
		if (!_activated) {
			src.Play ();
            _activated = true;
            spriteRenderer.sprite = activatedSprite;
            ring.enabled = true;
		}

    }
}
