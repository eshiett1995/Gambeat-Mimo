using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn
{
    public string ID;
    public float X;
    public bool HasLife;
    
    public Spawn(string id, float x, bool hasLife)
    {
        ID = id;
        X = x;
        HasLife = hasLife;
    }
}
