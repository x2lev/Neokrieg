using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase
{
    public int frames;
    public List<Box> hitboxes = new();
    public List<Box> hurtboxes = new();
    public List<Box> pushboxes = new();
    public Phase(int frames, List<Box> hitboxes, List<Box> hurtboxes, List<Box> pushboxes)
    {
        this.frames = frames;
        this.hitboxes = hitboxes;
        this.hurtboxes = hurtboxes;
        this.pushboxes = pushboxes;
    }
}
