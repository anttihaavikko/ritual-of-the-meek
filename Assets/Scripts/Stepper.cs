using AnttiStarterKit.Managers;
using UnityEngine;

public class Stepper : MonoBehaviour
{
    [SerializeField] private int sound;
    [SerializeField] private float volume;
    
    public void StepLeft()
    {
        Step(Vector3.left * 0.2f);
    }
    
    public void StepRight()
    {
        Step(Vector3.right * 0.2f);
    }

    private void Step(Vector3 dir)
    {
        AudioManager.Instance.PlayEffectAt(sound, transform.position + dir, volume);
    }      
}