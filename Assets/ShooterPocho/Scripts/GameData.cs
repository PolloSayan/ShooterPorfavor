using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{

    [SerializeField]
    private float currentHP;
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private List<Weapon> weapons;
    [SerializeField]
    private int weaponIndex;


    public float CurrentHP
    { 
        get { return currentHP; }
        set { currentHP = value; }
    }

    public float MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }

    public List<Weapon> Weapons
    {
        get { return weapons; }
        set { weapons = value; }
    }
    
    public int WeaponIndex
    {
        get { return weaponIndex; }
        set { weaponIndex = value; }
    }


}
