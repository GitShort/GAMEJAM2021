using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitchManager : MonoBehaviour
{
    [SerializeField] GameObject[] sofas;

    public void ActionDone()
    {
        sofas[0].SetActive(false);
        sofas[1].SetActive(true);
    }
}
