using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed;

    private MeshRenderer meshRenderer;
    private Vector3 originalPosition;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalPosition = transform.position;
    }

    void Update()
    {
        Vector3 posOffset = transform.position - originalPosition;
        Vector2 offset = new Vector2(posOffset.x, posOffset.z) * scrollSpeed;
        meshRenderer.material.SetTextureOffset("_MainTex", offset);
    }
}