using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Area : MonoBehaviour
{
    [SerializeField] private Color offColor;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        MarkTile(col, 1);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        MarkTile(col, -1);
    }

    private void MarkTile(Component from, int dir)
    {
        var tile = from.GetComponentInParent<Tile>();
        if (tile)
        {
            tile.AddOrRemove(dir, offColor);
        }
    }
}