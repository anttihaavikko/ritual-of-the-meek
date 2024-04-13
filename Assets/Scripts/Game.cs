using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject map;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            map.SetActive(!map.activeSelf);
        }
    }
}