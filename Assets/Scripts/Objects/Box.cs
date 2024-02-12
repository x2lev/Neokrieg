using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box
{
    public string tag = "";
    public Vector2 offset;
    public Vector2 size;
    public Box(string tag, Vector2 offset, Vector2 size)
    {
        this.tag = tag;
        this.offset = offset;
        this.size = size;
    }
}
