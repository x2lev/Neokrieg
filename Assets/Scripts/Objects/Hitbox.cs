using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox
{
    public Vector2 launchAngle;
    public int priority;
    public Hitbox(Vector2 launchAngle, int priority)
    {
        this.launchAngle = launchAngle;
        this.priority = priority;
    }
}
