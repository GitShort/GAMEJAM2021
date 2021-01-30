using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public int currentDay;

    void Start()
    {
        currentDay = 0;
    }

    void Update()
    {
        
    }

    public static GameManager Instance()
    {
        if (_instance == null)
        {
            _instance = new GameManager();
        }

        return _instance;
    }

    public void UpdateDay()
    {
        currentDay++;
    }
}
