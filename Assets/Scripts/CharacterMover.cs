using System;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Managers;
using AnttiStarterKit.Utils;
using AnttiStarterKit.Visuals;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    [SerializeField] private LayerMask walkMask, blockMask;
    [SerializeField] private float speed = 1f;
    [SerializeField] private AutoSpriteOrder autoSorter;
    [SerializeField] private GameObject jumpTrail;
    [SerializeField] private Dog dog;

    public bool Locked { get; set; }

    private Transform t;
    private Animator anim;
    private bool jumping, flying;
    
    private static readonly int Walking = Animator.StringToHash("walking");
    private static readonly int Channeling = Animator.StringToHash("channeling");

    private void Start()
    {
        anim = GetComponent<Animator>();
        t = transform;
    }

    private void FixedUpdate()
    {
        var input = GetInput();
        var moving = WillMove(input);
        anim.SetBool(Walking, moving);
    }

    private bool WillMove(Vector3 input)
    {
        return input.magnitude > 0.1f && TryMoves(input.normalized * speed);
    }

    private bool TryMoves(Vector3 input)
    {
        if (TryMove(input)) return true;
        // if (TryMove(input, true)) return true;
        if (TryMove(input.WhereX(0))) return true;
        // if (TryMove(input.WhereX(0), true)) return true;
        if (TryMove(input.WhereY(0))) return true;
        // if (TryMove(input.WhereY(0), true)) return true;
        return false;
    }

    private bool TryMove(Vector3 direction)
    {
        if (Locked || direction.magnitude < 0.1f) return false;
        
        var pos = t.position + direction * 0.03f;
        const float distance = 0.25f;
        const float vertical = 0.5f;
        
        // var leftBlocked = Check(pos + Vector3.left * distance, blockMask);
        // var rightBlocked = Check(pos + Vector3.right * distance, blockMask);
        // if (leftBlocked || rightBlocked) return false;
        
        var left = Check(pos + Vector3.left * distance, walkMask);
        var right = Check(pos + Vector3.right * distance, walkMask);
        var up = Check(pos + Vector3.up * distance * vertical, walkMask);
        var down = Check(pos + Vector3.down * distance * vertical, walkMask);

        // DebugDraw.Square(pos + Vector3.left * distance, left ? Color.green : Color.red, 0.3f);
        // DebugDraw.Square(pos + Vector3.right * distance, right ? Color.green : Color.red, 0.3f);
        // DebugDraw.Square(pos + Vector3.up * distance * vertical, up ? Color.green : Color.red, 0.3f);
        // DebugDraw.Square(pos + Vector3.down * distance * vertical, down ? Color.green : Color.red, 0.3f);

        if (left && right && up && down)
        {
            t.position += direction * 0.06f;
            return true;
        }

        return false;
    }

    private Collider2D Check(Vector3 pos, LayerMask layerMask, float radius = 0.01f)
    {
        return Physics2D.OverlapCircle(pos, radius, layerMask);
    }

    private Vector3 GetInput()
    {
        if (dog)
        {
            return dog.GetDirection();
        }
        
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        return new Vector3(h, v, 0);
    }

    public void Channel(bool state)
    {
        anim.SetBool(Channeling, state);
    }
}