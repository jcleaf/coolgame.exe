using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private Exit exit;

    void Awake()
    {
        exit = GetComponentInParent<Exit>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            exit.Open();
        }
    }
}
