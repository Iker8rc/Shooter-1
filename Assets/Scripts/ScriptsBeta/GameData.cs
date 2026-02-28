using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class GameData 
{
    [SerializeField]
    private float currentLife;
    [SerializeField]
    private float maxLife;
    [SerializeField]
    private List<Weapon> weapon;
    private int weaponIndex;

    public float CurrentLife
    {
        get { return currentLife; }
        set { currentLife = value; }
    }
    public float MaxLife
    {
        get { return maxLife; }
        set { maxLife = value; }
    }
    public List<Weapon> Weapon
    {
        get { return weapon; }
        set { weapon = value; }
    }
    public int WeaponIndex
    {
        get { return weaponIndex;  }
        set { weaponIndex = value; }   
    }
}
