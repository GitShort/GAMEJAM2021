using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] GameObject[] Doors;
    public bool IsOpened = false;

    public void OpenDoor()
    {
        IsOpened = true;
        Doors[0].SetActive(false);
        Doors[1].SetActive(true);
    }

    public void CloseDoor()
    {
        IsOpened = false;
        Doors[0].SetActive(true);
        Doors[1].SetActive(false);
    }
}
