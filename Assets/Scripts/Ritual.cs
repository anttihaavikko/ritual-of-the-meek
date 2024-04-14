using System;
using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Managers;
using UnityEngine;

public class Ritual : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private GameObject vessel;
    [SerializeField] private List<GameObject> candles;
    [SerializeField] private Dog dog;
    [SerializeField] private GameObject flames;
    [SerializeField] private Blinders blinders;
    [SerializeField] private GameObject beast;

    private bool started;

    private void Start()
    {
        beast.transform.parent = SceneChanger.Instance.root;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (started) return;
        
        var litCandles = candles.Take(game.CandleCount).ToList();
        var p = transform.position;

        if (!game.HasPower && game.HasVessel || litCandles.Any(c => !c.activeSelf))
        {
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

        if (game.HasPower && game.CandleCount >= 10 && game.HasDagger)
        {
            if (Vector3.Distance(dog.transform.position, p) > 5f)
            {
                game.ShowMessage("Where did that (pesky creature) go now...", BubbleType.None, 4f, true);
                return;
            }
            
            AudioManager.Instance.PlayEffectAt(11, p);
            started = true;
            flames.SetActive(true);
            AudioManager.Instance.TargetPitch = 0f;
            game.ShowMessage("It's (show time)...", BubbleType.None, 3.5f);
            this.StartCoroutine(() => blinders.Close(), 5f);
            this.StartCoroutine(() => dog.gameObject.SetActive(false), 5.5f);
            this.StartCoroutine(() => beast.SetActive(true), 7f);
        }
    }
}