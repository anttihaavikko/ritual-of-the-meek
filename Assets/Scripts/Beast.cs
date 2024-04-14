using AnttiStarterKit.Managers;
using UnityEngine;

public class Beast : MonoBehaviour
{
    public void Slit()
    {
        AudioManager.Instance.PlayEffectAt(12, Vector3.zero, 3f);
    }

    public void Splash()
    {
        AudioManager.Instance.PlayEffectAt(7, Vector3.zero, 3f);
    }
    
    public void Eye()
    {
        AudioManager.Instance.PlayEffectAt(13, Vector3.zero, 1f);
    }
    
    public void Screech()
    {
        AudioManager.Instance.PlayEffectAt(14, Vector3.zero, 3f);
    }
}