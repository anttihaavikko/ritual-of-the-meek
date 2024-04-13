using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> colorSprites;

    public void Color(Color color)
    {
        colorSprites.ForEach(s => s.color = color);
    }
}