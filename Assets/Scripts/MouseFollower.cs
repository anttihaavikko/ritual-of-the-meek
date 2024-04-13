using System;
using AnttiStarterKit.Extensions;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Vector3 scale = Vector3.one;
    [SerializeField] private Camera cam;

    private void Update()
    {
        var mp = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);
        var dir = (mp - root.position).normalized;
        transform.localPosition = new Vector3(dir.x * scale.x, dir.y * scale.y, dir.z * scale.z);
    }
}