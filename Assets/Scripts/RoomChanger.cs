using System;
using AnttiStarterKit.Animations;
using AnttiStarterKit.Extensions;
using UnityEngine;

public class RoomChanger : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Game game;

    private void OnTriggerEnter2D(Collider2D col)
    {
        var room = col.GetComponent<Room>();
        if (room)
        {
            // Tweener.MoveToQuad(cam.transform, col.transform.position.WhereZ(-10), 0.5f);
            room.Activate();
        }
        
        var talker = col.GetComponent<Talker>();
        if (talker)
        {
            game.ShowMessage(talker.message, talker.hideWith, talker.delay);
            talker.gameObject.SetActive(false);
        }
    }
}