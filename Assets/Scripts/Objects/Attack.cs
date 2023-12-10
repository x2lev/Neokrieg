using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    int startup;
    int recovery;
    Dictionary<int, List<Hitbox>> attack = new();
    public Attack(int startup, int recovery, Dictionary<int, List<Hitbox>> attack)
    {
        this.startup = startup;
        this.recovery = recovery;
        this.attack = attack;
    }
}
