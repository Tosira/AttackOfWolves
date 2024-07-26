using System.Collections.Generic;
using UnityEngine;

public class Statistics
{
    protected short level;
    public short Level { get{ return level; } }
    protected short availableLevel; // No se usa
    public short AvailableLevel { get{ return availableLevel; } }
    protected float damage;
    public float Damage { get{ return damage; } }
    protected float bulletSpeed;
    public float BulletSpeed { get{ return bulletSpeed; } }
    protected float radio;
    public float Radio { get{ return radio; } }
    protected float frequency;
    public float Frequency { get{ return frequency; } }
    protected int precio;
    public int Precio { get{ return precio; } }
}

public class WaterTower2 : Statistics
{
    public WaterTower2()
    {
        level = 2;
        availableLevel = 2;
        damage = 0.35f; //
        bulletSpeed = 1.3f; //
        radio = 3.0f;
        frequency = 1.2f;
        precio = 40; //
    }
}

public class WaterTower3 : Statistics
{
    public WaterTower3()
    {
        level = 3;
        availableLevel = 2;
        damage = 0.40f; //
        bulletSpeed = 1.3f;
        radio = 3.0f;
        frequency = 1.5f; //
        precio = 55; //
    }
}

public class StoneTower2 : Statistics
{
    public StoneTower2()
    {
        level = 2;
        availableLevel = 1;
        damage = 1f;
        bulletSpeed = 1f;
        radio = 6f;
        frequency = 1.2f; //
        precio = 50; //
    }
}

public class StoneTower3 : Statistics
{
    public StoneTower3()
    {
        level = 3;
        availableLevel = 1;
        damage = 1.3f; //
        bulletSpeed = 1f;
        radio = 6f;
        frequency = 1.5f; //
        precio = 65; //
    }
}

public class MudTower2 : Statistics
{
    public MudTower2()
    {
        level = 2;
        availableLevel = 1;
        damage = 1.6f; //
        bulletSpeed = 1.5f;
        radio = 4f;
        frequency = 1.7f; //
        precio = 60; //
    }
}

public class MudTower3 : Statistics
{
    public MudTower3()
    {
        level = 3;
        availableLevel = 1;
        damage = 1.8f; //
        bulletSpeed = 1.6f; //
        radio = 4.2f; //
        frequency = 1.7f;
        precio = 75; //
    }
}

public class FastTower2 : Statistics
{
    public FastTower2()
    {
        level = 2;
        availableLevel = 3;
        damage = 0.55f; //
        bulletSpeed = 1.3f; //
        radio = 5f;
        frequency = 0.3f;
        precio = 65; //
    }
}

public class FastTower3 : Statistics
{
    public FastTower3()
    {
        level = 3;
        availableLevel = 3;
        damage = 0.65f; //
        bulletSpeed = 1.3f;
        radio = 5f;
        frequency = 0.23f; //
        precio = 80; //
    }
}