using System;
using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dog : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private List<Transform> fakeWalls;
    [SerializeField] private Animator anim;

    private bool barking;
    private static readonly int Bark = Animator.StringToHash("bark");

    public Vector3 GetDirection()
    {
        var spot = GetSpot();
        var diff = spot - transform.position;
        var dir = diff.magnitude > 0.5f && (diff.magnitude < 5f || barking) ? diff.normalized : Vector3.zero;
        if (Mathf.Abs(dir.x) > 0.2f)
        {
            var t = transform;
            var scale = Mathf.Abs(t.localScale.x);
            t.localScale = new Vector3((dir.x > 0 ? -1 : 1) * scale, t.localScale.y, 1);
        }
        return dir;
    }

    private Vector3 GetSpot()
    {
        var p = transform.position;
        var mp = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);
        var wall = fakeWalls.Where(f => f.gameObject.activeSelf).OrderBy(f => Vector3.Distance(f.position, p)).FirstOrDefault();
        var wp = wall ? wall.position : Vector3.zero;
        barking = wall && Vector3.Distance(p, wp) < Mathf.Min(Vector3.Distance(p, mp), 10f);
        return barking ? wp : mp;
    }

    private void Update()
    {
        var hit = Physics2D.OverlapPoint(transform.position, groundMask);
        if (hit)
        {
            transform.parent = hit.transform;
        }

        if (barking && Random.value < 0.1f)
        {
            anim.SetTrigger(Bark);
        }
    }
}