using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Area : MonoBehaviour
{
    [SerializeField] private Color offColor;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        ColorizeTile(col, Color.white);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        ColorizeTile(col, offColor);
    }

    private void ColorizeTile(Collider2D from, Color color)
    {
        var tile = from.GetComponentInParent<Tile>();
        if (tile)
        {
            tile.Color(color);
        }
    }
}