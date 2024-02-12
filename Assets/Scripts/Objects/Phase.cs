using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase
{
    public int frames;
    public List<Box> hitboxes = new();
    public List<Box> hurtboxes = new();
    public List<Box> pushboxes = new();
    public Phase(int frames, List<Box> hurtboxes, List<Box> hitboxes, List<Box> pushboxes)
    {
        this.frames = frames;
        this.hurtboxes = hurtboxes;
        this.hitboxes = hitboxes;
        this.pushboxes = pushboxes;
    }
}
