using System;
using AnttiStarterKit.Extensions;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask groundMask;

    public Vector3 GetDirection()
    {
        var mp = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);
        var diff = mp - transform.position;
        var dir = diff.magnitude is > 0.3f and < 5f ? diff.normalized : Vector3.zero;
        if (Mathf.Abs(dir.x) > 0.2f)
        {
            var t = transform;
            var scale = Mathf.Abs(t.localScale.x);
            t.localScale = new Vector3((dir.x > 0 ? -1 : 1) * scale, t.localScale.y, 1);
        }
        return dir;
    }

    private void Update()
    {
        var hit = Physics2D.OverlapPoint(transform.position, groundMask);
        if (hit)
        {
            transform.parent = hit.transform;
        }
    }
}