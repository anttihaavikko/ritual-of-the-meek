using System;
using AnttiStarterKit.Extensions;
using UnityEngine;

public class LookPoint : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private float range = 3f;
    
    
    private void Update()
    {
        var mp = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);
        var distance = Vector3.Distance(target.position, mp);
        transform.position = Vector3.MoveTowards(target.position, mp, Mathf.Min(range, distance));
    }
}