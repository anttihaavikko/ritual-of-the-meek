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
        var position = transform.position;
        var camPos = cam.transform.position;
        var flipped = position.x > camPos.x;
        var flipVertical = position.y > camPos.y + 3;
        transform.localScale = new Vector3(flipped ? -scale : scale, flipVertical ? -scale : scale, 1f);
        text.localScale = new Vector3(flipped ? -1f : 1f, flipVertical ? -1f : 1f, 1f);
    }
}