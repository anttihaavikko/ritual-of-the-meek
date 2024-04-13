using System;
using System.Collections;
using System.Collections.Generic;
using AnttiStarterKit.Extensions;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] private FixedJoint2D joint;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask layerMask;

    private Rigidbody2D connected;

    private void Update()
    {
        var mp = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);
        body.MovePosition(mp);
        
        if (Input.GetMouseButtonDown(0))
        {
            var block = Physics2D.OverlapPoint(mp, layerMask);
            if (block)
            {
                Debug.Log($"Connecting to {block.name}");
                connected = block.GetComponentInParent<Rigidbody2D>();
                connected.bodyType = RigidbodyType2D.Dynamic;
                connected.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                joint.connectedBody = connected;
                joint.connectedAnchor = mp;
                joint.anchor = mp;
            }

            return;
        }

        if (connected && Input.GetMouseButtonUp(0))
        {
            joint.connectedBody = null;
            connected.bodyType = RigidbodyType2D.Static;
            connected = null;
        }
    }
}
