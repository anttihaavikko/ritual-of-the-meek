using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject contents;
    [SerializeField] private bool startState;

    private void Start()
    {
        contents.SetActive(startState);
    }

    public void Activate()
    {
        contents.SetActive(true);
    }
}