using System;
using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Managers;
using UnityEngine;

public class Ritual : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private GameObject vessel;
    [SerializeField] private List<GameObject> candles;

    private void OnTriggerEnter2D(Collider2D col)
    {
        var litCandles = candles.Take(game.CandleCount).ToList();

        if (!game.HasPower && game.HasVessel || litCandles.Any(c => !c.activeSelf))
        {
            var p = transform.position;
            AudioManager.Instance.PlayEffectAt(9, p, 1f);
            AudioManager.Instance.PlayEffectAt(10, p, 2f);
        }
        
        if (game.HasVessel)
        {
            game.ShowMessage("This (vessel) now surges with (power). I feel like I could move (heavy platforms).", BubbleType.None, 7f);
            vessel.SetActive(true);
            game.ShowHorns();
            game.HasPower = true;
        }
        
        litCandles.ForEach(c => c.SetActive(true));
    }
}