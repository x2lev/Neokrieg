using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox
{
    public Vector2 center;
    public Vector2 offset;
    public Vector2 launchAngle;
    public int priority;
    public Hitbox(Vector2 center, Vector2 offset, Vector2 launchAngle, int priority)
    {
        this.center = center;
        this.offset = offset;
        this.launchAngle = launchAngle;
        this.priority = priority;
    }
}
