using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction : MonoBehaviour {

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite exclaimSprite;
    [SerializeField] Sprite questionSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Exclaim()
    {
        spriteRenderer.sprite = exclaimSprite;
        spriteRenderer.enabled = true;
    }

    public void Question()
    {
        spriteRenderer.sprite = questionSprite;
        spriteRenderer.enabled = true;
    }
}
