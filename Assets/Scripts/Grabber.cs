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
    [SerializeField] private CharacterMover characterMover;

    private Rigidbody2D connected;
    private Vector3 start;

    private void Update()
    {
        var mp = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);
        body.MovePosition(mp);
        
        if (Input.GetMouseButtonDown(0))
        {
            var current = Physics2D.OverlapPoint(characterMover.transform.position, layerMask);
            var block = Physics2D.OverlapPoint(mp, layerMask);
            if (block && block != current)
            {
                var body = block.GetComponentInParent<Rigidbody2D>();
                if (!body) return;
                var tile = body.GetComponent<Tile>();
                if (tile && tile.CanMove)
                {
                    start = tile.transform.position;
                    connected = body;
                    characterMover.Locked = true;
                    connected.bodyType = RigidbodyType2D.Dynamic;
                    connected.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                    joint.connectedBody = connected;
                    joint.connectedAnchor = connected.transform.InverseTransformPoint(mp);
                    joint.anchor = Vector2.zero;   
                }
            }

            return;
        }

        if (connected && Input.GetMouseButtonUp(0))
        {
            var tile = connected.GetComponent<Tile>();
            if(tile && !tile.CanMove) tile.transform.position = start;
            characterMover.Locked = false;
            joint.connectedBody = null;
            connected.bodyType = RigidbodyType2D.Kinematic;
            connected.velocity = Vector2.zero;
            connected = null;
        }
    }
}
