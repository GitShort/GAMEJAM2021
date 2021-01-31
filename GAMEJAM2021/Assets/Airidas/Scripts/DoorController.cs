using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] GameObject[] Doors;
    public bool IsOpened = false;
    [SerializeField] GameObject _light;

    [SerializeField] bool _isOutsideDoor = false;

    public void OpenDoor()
    {
        if (!_isOutsideDoor)
        {
            IsOpened = true;
            Doors[0].SetActive(false);
            Doors[1].SetActive(true);
        }
        else if (GameManager.TimeToLeave && _isOutsideDoor)
        {
            IsOpened = true;
            Doors[0].SetActive(false);
            Doors[1].SetActive(true);
        }
        else if (_isOutsideDoor)
        {
            Debug.Log("Its quarantine outside! You cant leave yet.");
        }
        
    }

    public void CloseDoor()
    {
        IsOpened = false;
        Doors[0].SetActive(true);
        Doors[1].SetActive(false);
    }

    public void TurnOnLight()
    {
        _light.SetActive(true);
    }

    public void TurnOffLight()
    {
        _light.SetActive(false);
    }
}
