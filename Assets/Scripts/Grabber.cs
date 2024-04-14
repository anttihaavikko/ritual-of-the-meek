using System;
using System.Collections;
using System.Collections.Generic;
using AnttiStarterKit.Extensions;
using Unity.VisualScripting;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] private FixedJoint2D joint;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private CharacterMover characterMover;
    [SerializeField] private Game game;

    private Rigidbody2D connected;
    private Vector3 start;
    private Tile held;
    private Tile stored;
    private Tile preview;
    private Vector3 grabPosition;
    private Vector3 offset;

    private void Update()
    {
        var mp = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);
        body.MovePosition(mp);

        if (held)
        {
            var p = held.transform.position;
            grabPosition = p + offset;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (preview)
            {
                preview.Solidify();
                preview = null;
                return;
            }
            
            var current = Physics2D.OverlapPoint(characterMover.transform.position, layerMask);
            var block = Physics2D.OverlapPoint(mp, layerMask);
            if (block && block != current)
            {
                var body = block.GetComponentInParent<Rigidbody2D>();
                if (!body) return;
                var tile = body.GetComponent<Tile>();
                if (tile && tile.CanMove)
                {
                    if (tile.IsHeavy && !game.HasPower)
                    {
                        game.ShowMessage("That (platform) is (too heavy) for me to move.", BubbleType.None, 3f, true);
                        return;
                    }
                    
                    game.HideBubbleIf(BubbleType.Grab);
                    game.ShowMessage(InfoMessage.Grabbed);
                    held = tile;
                    start = tile.transform.position;
                    connected = body;
                    characterMover.Locked = true;
                    connected.bodyType = RigidbodyType2D.Dynamic;
                    connected.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                    joint.connectedBody = connected;
                    joint.connectedAnchor = connected.transform.InverseTransformPoint(mp);
                    joint.anchor = Vector2.zero;
                    offset = mp - start;

                    characterMover.Channel(true);
                }
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!game.HasBag) return;
            
            if (held && !stored)
            {
                game.ShowMessage("I can (pull that out) any time I want by pressing (SPACE) again.", BubbleType.Release);
                held.gameObject.SetActive(false);
                stored = held;
                Drop();
                return;
            }

            if (!held && stored)
            {
                game.HideBubbleIf(BubbleType.Release);
                stored.gameObject.SetActive(true);
                stored.Ghost();
                preview = stored;
                stored = null;
            }
        }

        if (preview)
        {
            preview.transform.position = mp;
        }

        if (connected && Input.GetMouseButtonUp(0))
        {
            Drop();
        }
    }

    private void Drop()
    {
        var tile = connected.GetComponent<Tile>();
        if (tile && !tile.CanMove)
        {
            tile.transform.position = start;
            game.ShowMessage("I can't (place) the platform (too far) outside of my (immediate vicinity).", BubbleType.None, 4f);
        }
        held = null;
        characterMover.Locked = false;
        joint.connectedBody = null;
        connected.bodyType = RigidbodyType2D.Kinematic;
        connected.velocity = Vector2.zero;
        connected = null;
        characterMover.Channel(false);
    }

    public Vector3 GetGrabPosition()
    {
        return grabPosition;
    }
}
