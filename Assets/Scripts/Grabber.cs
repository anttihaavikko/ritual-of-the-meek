using System;
using System.Collections;
using System.Collections.Generic;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Managers;
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
    [SerializeField] private Color waterColor, edgeColor;
    [SerializeField] private ParticleSystem pickEffect;

    private Rigidbody2D connected;
    private Vector3 start;
    private Tile held;
    private Tile stored;
    private Tile preview;
    private Vector3 grabPosition;
    private Vector3 offset;
    private Vector3 shape, storedShape;

    private void Update()
    {
        var mp = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);
        body.MovePosition(mp);

        if (held)
        {
            grabPosition = held.transform.position + offset;
        }
        
        if (preview)
        {
            grabPosition = preview.transform.position;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (preview)
            {
                characterMover.Channel(false);
                PickSound(preview.transform.position);
                preview.Solidify();
                shape = storedShape;
                Splash(mp);
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

                    var bt = block.transform;
                    shape = bt.localScale;
                    Splash(bt.position);

                    AudioManager.Instance.TargetPitch = 1.2f;
                    
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
                    
                    PickSound(mp);
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
                storedShape = shape;
                Drop();
                return;
            }

            if (!held && stored)
            {
                Splash(stored.transform.position);
                PickSound(mp);
                game.HideBubbleIf(BubbleType.Release);
                stored.gameObject.SetActive(true);
                stored.Ghost(waterColor, edgeColor);
                preview = stored;
                stored = null;
                characterMover.Channel(true);
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

    private void Splash(Vector3 pos)
    {
        var shapeModule = pickEffect.shape;
        shapeModule.scale = shape;
        pickEffect.transform.position = pos;
        pickEffect.Play();
    }

    private void Drop()
    {
        var tile = connected.GetComponent<Tile>();
        if (tile && !tile.CanMove)
        {
            tile.transform.position = start;
            Splash(start);
            game.ShowMessage("I can't (place) the platform (too far) outside of my (immediate vicinity).", BubbleType.None, 4f);
        }

        var p = tile.transform.position;
        Splash(p);
        held = null;
        characterMover.Locked = false;
        joint.connectedBody = null;
        connected.bodyType = RigidbodyType2D.Kinematic;
        connected.velocity = Vector2.zero;
        connected = null;
        characterMover.Channel(false);
        
        AudioManager.Instance.TargetPitch = 1f;
        PickSound(p);
    }

    private void PickSound(Vector3 pos)
    {
        AudioManager.Instance.PlayEffectAt(5, pos, 1.5f);
        AudioManager.Instance.PlayEffectAt(6, pos, 1f);
        AudioManager.Instance.PlayEffectFromCollection(2, pos);
        AudioManager.Instance.PlayEffectAt(7, pos, 1.5f);
    }

    public Vector3 GetGrabPosition()
    {
        return grabPosition;
    }
}
