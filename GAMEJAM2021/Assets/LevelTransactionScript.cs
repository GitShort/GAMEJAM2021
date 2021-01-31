using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTransactionScript : MonoBehaviour
{
    public string Day;
    public TextMeshProUGUI TextInput;

    // Update is called once per frame
    void Update()
    {
        Day = "DAY ";
        Day += GameManager.CurrentDay.ToString();
        TextInput.text = Day;
    }
}
