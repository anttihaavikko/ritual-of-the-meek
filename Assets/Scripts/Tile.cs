using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> colorSprites, outlineSprites;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private bool heavy;

    public bool CanMove { get; private set; }

    public bool IsHeavy => heavy;
    
    private int marks;

    private void Colorize(Color color)
    {
        colorSprites.ForEach(s => s.color = color);
    }

    public void AddOrRemove(int dir, Color off)
    {
        marks += dir;
        var state = marks > 0;
        Colorize(state ? Color.white : off);
        CanMove = state;
    }

    public void Ghost(Color waterColor, Color edgeColor)
    {
        outlineSprites.ForEach(s => s.color = edgeColor);
        body.bodyType = RigidbodyType2D.Static;
        Colorize(waterColor);
    }

    public void Solidify()
    {
        marks = 0;
        body.bodyType = RigidbodyType2D.Dynamic;
        outlineSprites.ForEach(s => s.color = Color.black);
        Colorize(Color.white);
        Invoke(nameof(MakeKinematic), 0.1f);
    }

    private void MakeKinematic()
    {
        body.bodyType = RigidbodyType2D.Kinematic;
    }
}