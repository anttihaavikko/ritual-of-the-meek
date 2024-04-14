using System;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Managers;
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
        UpdateLightning();
    }

    private void UpdateLightning()
    {
        Zap(leftLightning, Vector3.left * 0.3f);
        Zap(rightLightning, Vector3.right * 0.3f);
        Invoke(nameof(UpdateLightning), 0.025f);
    }

    private void Zap(LineRenderer lightning, Vector3 offset)
    {
        var start = lightning.transform.position;
        var end = grabber.GetGrabPosition() + offset;
        lightning.SetPosition(0, start);
        lightning.SetPosition(1, Vector3.Lerp(start, end, 0.25f).RandomOffset(0.45f));
        lightning.SetPosition(2, Vector3.Lerp(start, end, 0.5f).RandomOffset(0.45f));
        lightning.SetPosition(3, Vector3.Lerp(start, end, 0.75f).RandomOffset(0.45f));
        lightning.SetPosition(4, end);
    }
}