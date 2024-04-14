using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleType type;
    [SerializeField] private Game game;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        game.Collect(type, transform.position);
        gameObject.SetActive(false);
    }
}

public enum CollectibleType
{
    Candle,
    Map,
    Dagger,
    Bag,
    Vessel
}