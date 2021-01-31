using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static int CurrentDay = 0;
    public static bool PlayerShaved;
    public static bool AteFood; // TODO: set to true when meal from the fridge has been chosen
    public static bool ComputerActionDone; // TOOD: set to true when the required computer action has been completed 
    public static bool LaundryDone;
    public static bool SofaCleaned;
    public static bool TrashThrownOut;
    public static bool WorkedOut;
    public static bool WorkedOutTwice;
    public static bool WearingNewClothes;

    public static bool TimeToLeave;

    public static bool DayCompleted;

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
        PlayerShaved = false;
        WorkedOutTwice = false;
        WearingNewClothes = false;
        _gymEquipment.SetActive(false);
        _newClothes.SetActive(false);
    }

    void Update()
    {
        SpawnNewObjects(4, _gymEquipment);
        SpawnNewObjects(7, _newClothes);
        DayManagement();

        if (CurrentDay == 7 && WearingNewClothes && AteFood && !TimeToLeave)
            TimeToLeave = true;
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
        DayCompleted = false;
    }

    public static void PurchaseFinished()
    {
        ComputerActionDone = true;
    }

    public static void AteFoodFromFridge()
    {
        AteFood = true;
    }

    public void SpawnNewObjects(int day, GameObject obj)
    {
        if (CurrentDay == day && obj != null)
            obj.SetActive(true);
    }

    public void DayManagement()
    {
        if (CurrentDay == 0 && PlayerShaved && AteFood && !DayCompleted)
        {
            DayCompleted = true;
            Debug.Log("Day 0 finished");
        }
        else if (CurrentDay == 1 && ComputerActionDone && AteFood && !DayCompleted)
        {
            DayCompleted = true;
            Debug.Log("Day 1 finished");
        }
        else if (CurrentDay == 2 && LaundryDone && AteFood && !DayCompleted)
        {
            DayCompleted = true;
            Debug.Log("Day 2 finished");
        }
        else if (CurrentDay == 3 && SofaCleaned && TrashThrownOut && AteFood && !DayCompleted)
        {
            DayCompleted = true;
            Debug.Log("Day 3 finished");
        }
        else if (CurrentDay == 4 && WorkedOut && AteFood)
        {
            DayCompleted = true;
            Debug.Log("Day 4 finished");
        }
        else if (CurrentDay == 5 && WorkedOut && AteFood)
        {
            DayCompleted = true;
            Debug.Log("Day 5 finished");
        }
        else if (CurrentDay == 6 && WorkedOut && ComputerActionDone && AteFood)
        {
            DayCompleted = true;
            Debug.Log("Day 6 finished");
        }
    }
}
