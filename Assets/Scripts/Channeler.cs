using System;
using AnttiStarterKit.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

public class Channeler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D leftCape, rightCape, cape;
    [SerializeField] private LineRenderer leftLightning, rightLightning;
    [SerializeField] private Camera cam;
    [SerializeField] private Grabber grabber;
    [SerializeField] private Rigidbody2D scarfTip;

    public void FlutterCape()
    {
        leftCape.AddForce(new Vector2(-1, 0), ForceMode2D.Impulse);
        rightCape.AddForce(new Vector2(1, 0), ForceMode2D.Impulse);
        cape.AddForce(new Vector2(Random.Range(-1f, 1f), 0), ForceMode2D.Impulse);
        scarfTip.AddForce(new Vector2(Random.Range(-1f, 1f), 0), ForceMode2D.Impulse);
    }

    private void Start()
    {
        scarfTip.transform.parent = null;
        Zap(leftLightning, Vector3.left * 0.3f);
        Zap(rightLightning, Vector3.right * 0.3f);
    }

    private void Update()
    {
        if (Random.value < 0.8f) return;
        Zap(leftLightning, Vector3.left * 0.3f);
        Zap(rightLightning, Vector3.right * 0.3f);
    }

    private void Zap(LineRenderer lightning, Vector3 offset)
    {
        var mp = grabber.GetGrabPosition() + offset;
        var end = lightning.transform.InverseTransformPoint(mp);
        lightning.SetPosition(1, (end * 0.25f).RandomOffset(3f));
        lightning.SetPosition(2, (end * 0.5f).RandomOffset(3f));
        lightning.SetPosition(3, (end * 0.75f).RandomOffset(3f));
        lightning.SetPosition(4, lightning.transform.InverseTransformPoint(mp));
    }
}