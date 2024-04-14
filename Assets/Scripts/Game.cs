using System;
using System.Collections.Generic;
using AnttiStarterKit.Animations;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Managers;
using AnttiStarterKit.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private SpeechBubble bubble;
    [SerializeField] private GameObject bag, dagger, horns;
    [SerializeField] private GameObject mouth;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator mapAnim;
    [SerializeField] private Dog dog;
    [SerializeField] private Appearer titleTop, titleBottom;
    [SerializeField] private Volume volume;
    

    private bool mapShown;
    private BubbleType hideWith;
    private readonly List<string> shownMessages = new ();
    private int candles;
    private readonly List<CollectibleType> collection = new();
    private bool started;
    private ColorAdjustments colors;
    
    private static readonly int Talk = Animator.StringToHash("talk");
    private static readonly int TiltRight = Animator.StringToHash("tilt-right");
    private static readonly int TiltLeft = Animator.StringToHash("tilt-left");
    private static readonly int Open = Animator.StringToHash("open");
    private static readonly int Close = Animator.StringToHash("close");

    public bool HasBag { get; private set; }
    public bool HasMap { get; private set; }
    public bool HasDagger { get; private set; }
    public bool HasVessel{ get; private set; }
    public bool HasPower { get; set; }

    public int CandleCount => candles;

    private void Start()
    {
        bubble.onVocal += OpenMouth;
        bubble.onWord += Tilt;
        volume.profile.TryGet(out colors);
    }

    private void Tilt()
    {
        var p = mouth.transform.position;
        // AudioManager.Instance.PlayEffectAt(0, p, 2f);
        AudioManager.Instance.PlayEffectAt(2, p, 1.5f);
        AudioManager.Instance.PlayEffectFromCollection(0, p);
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

    public void ShowMessage(string message, BubbleType type = BubbleType.None, float delay = 0f, bool canRepeat = false)
    {
        if(shownMessages.Contains(message) && !canRepeat) return;
        
        if(!canRepeat) shownMessages.Add(message);
        
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
        if (!started && Input.anyKeyDown)
        {
            started = true;
            titleTop.Hide();
            titleBottom.Hide();
            Invoke(nameof(StartMessage), 0.5f);
        }

        if (started)
        {
            colors.saturation.value = Mathf.MoveTowards(colors.saturation.value, 0, Time.deltaTime * 180f);
        }
        
        if (Input.GetKeyDown(KeyCode.Tab) && HasMap)
        {
            // map.SetActive(!map.activeSelf);
            var p = transform.position;
            AudioManager.Instance.PlayEffectAt(8, p, 7f);
            AudioManager.Instance.PlayEffectAt(6, p, 1f);
            mapShown = !mapShown;
            map.SetActive(true);
            mapAnim.ResetTrigger(Open);
            mapAnim.ResetTrigger(Close);
            mapAnim.SetTrigger(mapShown ? Open : Close);
            if (!mapShown) this.StartCoroutine(() => map.SetActive(false), 0.2f);
            HideBubbleIf(BubbleType.Map);
        }

        if (DevKey.Down(KeyCode.Q))
        {
            HasMap = true;
            HasDagger = true;
            dagger.SetActive(true);
            HasBag = true;
            bag.SetActive(true);
            HasPower = true;
            horns.SetActive(true);
            candles = 10;
        }

        if (DevKey.Down(KeyCode.E))
        {
            var dt = dog.transform;
            dt.position = transform.position.RandomOffset(1f);
            dt.parent = null;
        }
    }

    public void Collect(CollectibleType type, Vector3 p)
    {
        AudioManager.Instance.PlayEffectAt(9, p, 1f);
        AudioManager.Instance.PlayEffectAt(10, p, 2f);
        
        if (type == CollectibleType.Candle)
        {
            candles++;
            ShowMessage(GetCandleMessage(), BubbleType.None, 5f);
            return;
        }

        if (!collection.Contains(type))
        {
            if (type == CollectibleType.Dagger)
            {
                HasDagger = true;
                dagger.SetActive(true);
                ShowMessage("The most (essential tool) needed for the (ritual). Lucky find!", BubbleType.None, 5f);
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
                ShowMessage("I should go place this (vessel) to the (ritual site).", BubbleType.None, 5f);
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
            1 => "Exactly what I need for the (ritual), except total of (ten) of them.",
            2 => "Yes, (candle)! But I still need to find (eight) more.",
            3 => "Sweet, (another candle)! Still need (seven) more.",
            4 => "A ha, (a candle)! Only (six) more.",
            5 => "Yes, (another candle)! Half way there!",
            6 => "The glass is (half full). I've got (six) of these (candles) now.",
            7 => "I have (seven candles) now! The time of the (ritual) draws near...",
            8 => "Almost there, I now have (eight candles) in total.",
            9 => "Yes, (another candle)! Only (one) more to find.",
            10 => "Finally, all (ten candles) gathered!",
            _ => "Lorem ipsum dolor"
        };
    }

    public void PlaySound(int soundIndex)
    {
        AudioManager.Instance.PlayEffectAt(soundIndex, transform.position);
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