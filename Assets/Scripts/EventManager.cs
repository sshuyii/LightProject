using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;
    private void Awake()
    {
        current = this;
    }

    public event Action<int> OnItemPickUp;
    public void ItemPickUp(int player)
    {
        //called by other scripts
        OnItemPickUp?.Invoke(player);
    }

    public event Action OnLevelEnd;
    public void LevelEnd()
    {
        OnLevelEnd?.Invoke();
        Debug.Log("Level ends!");
    }
}
