using UnityEngine;
using System;
using System.Collections.Generic;



[Serializable]
public class MogusGameData
{
    [SerializeField]
    private float mogusCurrentHP;
    [SerializeField]
    private float mogusMaxHP;


    public float MogusCurrentHP
    {
        get { return mogusCurrentHP; }
        set { mogusCurrentHP = value; }
    }

    public float MogusMaxHP
    {
        get { return mogusMaxHP; }
        set { mogusMaxHP = value; }
    }

}
