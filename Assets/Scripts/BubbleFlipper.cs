using System;
using AnttiStarterKit.Extensions;
using UnityEngine;

public class BubbleFlipper : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform text;

    private float scale;

    private void Awake()
    {
        scale = transform.localScale.x;
    }

    private void Update()
    {
        var flipped = transform.position.x > cam.transform.position.x;
        transform.localScale = new Vector3(flipped ? -scale : scale, scale, 1f);
        text.localScale = new Vector3(flipped ? -1f : 1f, 1f, 1f);
    }
}