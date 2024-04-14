using System;
using UnityEngine;

public class FakeWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        gameObject.SetActive(false);
    }
}