using System;
using System.Collections.Generic;
using AnttiStarterKit.Animations;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private SpeechBubble bubble;
    [SerializeField] private GameObject bag, dagger, horns;
    [SerializeField] private GameObject mouth;
    [SerializeField] private Animator anim;

    private BubbleType hideWith;
    private readonly List<string> shownMessages = new ();
    private int candles;
    private readonly List<CollectibleType> collection = new();
    private static readonly int Talk = Animator.StringToHash("talk");
    private static readonly int TiltRight = Animator.StringToHash("tilt-right");
    private static readonly int TiltLeft = Animator.StringToHash("tilt-left");

    public bool HasBag { get; private set; }
    public bool HasMap { get; private set; }
    public bool HasDagger { get; private set; }
    public bool HasVessel{ get; private set; }
    public bool PlacedVessel { get; set; }

    public int CandleCount => candles;

    private void Start()
    {
        bubble.onVocal += OpenMouth;
        bubble.onWord += Tilt;
        Invoke(nameof(StartMessage), 0.5f);
    }

    private void Tilt()
    {
        anim.SetTrigger(Random.value < 0.5f ? TiltRight : TiltLeft);
    }
    
    private void OpenMouth()
    {
        // anim.SetTrigger(Talk);
        // anim.SetTrigger(Random.value < 0.5f ? TiltRight : TiltLeft);
        mouth.SetActive(true);
        this.StartCoroutine(() => mouth.SetActive(false), 0.1f);
    }

    public void HideBubbleIf(BubbleType type)
    {
        if(type == hideWith) bubble.Hide();
    }

    private void StartMessage()
    {
        ShowMessage("I should get this (ritual) started. Got to find some (necessary ingredients).");
    }

    public void ShowMessage(string message, BubbleType type = BubbleType.None, float delay = 0f)
    {
        if(shownMessages.Contains(message)) return;
        
        shownMessages.Add(message);
        
        hideWith = type;
        bubble.Show(message, true);

        if (delay > 0f)
        {
            Invoke(nameof(HideBubble), delay);
        }
    }

    public void ShowMessage(InfoMessage message)
    {
        ShowMessage(GetMessage(message), BubbleType.None, GetMessageDelay(message));
    }

    private string GetMessage(InfoMessage message)
    {
        return message switch
        {
            InfoMessage.None => "",
            InfoMessage.Grabbed => "Oh (yes), just (like that)!",
            _ => throw new ArgumentOutOfRangeException(nameof(message), message, null)
        };
    }

    private float GetMessageDelay(InfoMessage message)
    {
        return message switch
        {
            InfoMessage.None => 1f,
            InfoMessage.Grabbed => 2f,
            _ => 1f
        };
    }

    public void HideBubble()
    {
        bubble.Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && HasMap)
        {
            map.SetActive(!map.activeSelf);
            HideBubbleIf(BubbleType.Map);
        }

        if (DevKey.Down(KeyCode.Q))
        {
            HasMap = true;
            HasDagger = true;
            dagger.SetActive(true);
            HasBag = true;
            bag.SetActive(true);
            PlacedVessel = true;
            horns.SetActive(true);
        }
    }

    public void Collect(CollectibleType type)
    {
        if (type == CollectibleType.Candle)
        {
            candles++;
            ShowMessage(GetCandleMessage(), BubbleType.None, 3f);
            return;
        }

        if (!collection.Contains(type))
        {
            if (type == CollectibleType.Dagger)
            {
                HasDagger = true;
                dagger.SetActive(true);
                ShowMessage("The most (essential tool) needed for the (ritual). Lucky find!", BubbleType.None, 4f);
            }
            
            if (type == CollectibleType.Map)
            {
                HasMap = true;
                ShowMessage("Oh nice! This (map) is bound to be (useful). View it with (TAB) key.", BubbleType.Map);
            }
            
            if (type == CollectibleType.Bag)
            {
                HasBag = true;
                bag.SetActive(true);
                ShowMessage("This (bag) can hold (extra platforms)! Press (SPACE) while holding one to (store) it.", BubbleType.Hold);
            }
            
            if (type == CollectibleType.Vessel)
            {
                HasVessel = true;
                ShowMessage("I should go place this (vessel) to the (ritual site).", BubbleType.None, 4f);
            }
                
            collection.Add(type);
        }
    }

    public void ShowHorns()
    {
        horns.SetActive(true);
    }

    private string GetCandleMessage()
    {
        return candles switch
        {
            1 => "Exactly what I need for the (ritual), except (five) of them.",
            2 => "Yes, (another candle)! Only (three) more.",
            3 => "Yes, (another candle)! Only (two) more.",
            4 => "Yes, (another candle)! Only (one) more.",
            5 => "Finally, all (five candles)!",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum InfoMessage
{
    None,
    Grabbed,
    Release
}

public enum BubbleType
{
    None,
    Grab,
    Map,
    Hold,
    Release
}