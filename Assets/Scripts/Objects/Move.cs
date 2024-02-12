using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Move
{
    HitboxScript hitb;
    HurtboxScript hurtb;
    PushboxScript pushb;
    int frame = 0;
    int phaseIndex = 0;
    List<Phase> phases = new();
    public Move(PlayerScript player, List<Phase> phases)
    {
        hitb = player.GetComponent<HitboxScript>();
        hurtb = player.GetComponent<HurtboxScript>();
        pushb = player.GetComponent<PushboxScript>();
        this.phases = phases;
    }
    public void Start()
    {
        frame = 0;
        phaseIndex = 0;
    }
    public bool Advance()
    {
        frame += 1;
        if (frame > phases[phaseIndex].frames)
        {
            phaseIndex += 1;
            if (phaseIndex == phases.Count)
                return false;
            else
            {
                hitb.SetColliders(phases[phaseIndex].hitboxes);
                hurtb.SetColliders(phases[phaseIndex].hurtboxes);
                pushb.SetColliders(phases[phaseIndex].pushboxes);
            }
        }
        
        return true;
    }
}
