using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    PlayerScript player;
    HurtboxScript hurtb;
    HitboxScript hitb;
    PushboxScript pushb;
    int frame = 0;
    int phaseIndex = 0;
    public List<Phase> phases = new();
    public Move(PlayerScript player, List<Phase> phases)
    {
        this.player = player;
        hurtb = player.GetComponentInChildren<HurtboxScript>();
        hitb = player.GetComponentInChildren<HitboxScript>();
        pushb = player.GetComponentInChildren<PushboxScript>();
        this.phases = phases;
    }
    public void Start()
    {
        frame = -1;
        phaseIndex = -1;
        player.activeMove = this;
        Advance();
    }
    public bool Advance()
    {
        Debug.Log(frame);
        if (frame < 0 || frame > phases[phaseIndex].frames)
        {
            phaseIndex += 1;
            if (phaseIndex == phases.Count)
            {
                player.activeMove = null;
                return false;
            }
            else
            {
                frame = 0;
                hurtb.SetColliders(phases[phaseIndex].hurtboxes);
                hitb.SetColliders(phases[phaseIndex].hitboxes);
                pushb.SetColliders(phases[phaseIndex].pushboxes);
            }
        }

        frame += 1;

        return true;
    }
}
