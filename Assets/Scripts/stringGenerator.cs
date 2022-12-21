using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stringGenerator : MonoBehaviour
{
    public string StringGenerate(string start)
    {
        string generated = start;
        for (int i = 0; i < 12; i++)
        {
            generated += Random.Range(0, 10).ToString();
        }
        return generated;
    }
}
