using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> colorSprites;

    public bool CanMove { get; private set; }
    
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
}