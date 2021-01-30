using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaManager : MonoBehaviour
{
    [SerializeField] GameObject[] sofas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleanSofa()
    {
        sofas[0].SetActive(false);
        sofas[1].SetActive(true);
    }
}
