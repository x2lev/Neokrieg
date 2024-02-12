using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : Box
{
    public Vector2 angle;
    public int priority;
    public bool test;
    public Hitbox(string tag, Vector2 offset, Vector2 size): base(tag, offset, size)
    {
        angle = Vector2.zero;
        priority = 0;
    }
    public Hitbox(string tag, Vector2 offset, Vector2 size, Vector2 angle, int priority): base(tag, offset, size)
    {
        this.angle = angle;
        this.priority = priority;
    }
}
