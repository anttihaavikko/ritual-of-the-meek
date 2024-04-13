using System;
using AnttiStarterKit.Animations;
using AnttiStarterKit.Extensions;
using UnityEngine;

public class RoomChanger : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        Tweener.MoveToQuad(cam.transform, col.transform.position.WhereZ(-10), 0.5f);
    }
}