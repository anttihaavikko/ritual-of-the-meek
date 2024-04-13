using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        gameObject.SetActive(false);
    }
}