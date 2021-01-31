using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static int CurrentDay = 0;
    public static bool AteFood; // TODO: set to true when meal from the fridge has been chosen
    public static bool ComputerActionDone; // TOOD: set to true when the required computer action has been completed 
    public static bool LaundryDone;
    public static bool SofaCleaned;
    public static bool TrashThrownOut;
    public static bool WorkedOut;

    public static bool TimeToLeave;

    [SerializeField] GameObject _gymEquipment;
    [SerializeField] GameObject _newClothes;

    void Start()
    {
        AteFood = false;
        LaundryDone = false;
        SofaCleaned = false;
        TrashThrownOut = false;
        CurrentDay = 0;
        WorkedOut = false;
        TimeToLeave = false;
        _gymEquipment.SetActive(false);
        _newClothes.SetActive(false);
    }

    void Update()
    {
        SpawnNewObjects(4, _gymEquipment);
        SpawnNewObjects(7, _newClothes);
    }

    public static GameManager Instance()
    {
        if (_instance == null)
        {
            _instance = new GameManager();
        }

        return _instance;
    }

    public static void UpdateDay()
    {
        CurrentDay++;
    }

    public static void ResetDay() // call this method to reset stuff for the next day
    {
        AteFood = false;
        ComputerActionDone = false;
        WorkedOut = false;
    }

    public void SpawnNewObjects(int day, GameObject obj)
    {
        if (CurrentDay == day && obj != null)
            obj.SetActive(true);
    }
}
