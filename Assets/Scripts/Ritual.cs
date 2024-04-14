using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ritual : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private GameObject vessel;
    [SerializeField] private List<GameObject> candles;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (game.HasVessel)
        {
            game.ShowMessage("This (vessel) now surges with (power). I feel like I could move (heavy platforms).", BubbleType.None, 5f);
            vessel.SetActive(true);
            game.ShowHorns();
            game.HasPower = true;
        }
        
        candles.Take(game.CandleCount).ToList().ForEach(c => c.SetActive(true));
    }
}