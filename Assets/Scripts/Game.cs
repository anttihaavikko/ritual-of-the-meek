using System;
using System.Collections.Generic;
using AnttiStarterKit.Animations;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private SpeechBubble bubble;
    [SerializeField] private GameObject bag;

    private BubbleType hideWith;
    private readonly List<string> shownMessages = new ();
    private int candles;
    private readonly List<CollectibleType> collection = new();
    
    public bool HasBag { get; private set; }
    public bool HasMap { get; private set; }
    public bool HasDagger { get; private set; }
    public bool HasVessel{ get; private set; }

    private void Start()
    {
        Invoke(nameof(StartMessage), 0.5f);
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
                
            collection.Add(type);
        }
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