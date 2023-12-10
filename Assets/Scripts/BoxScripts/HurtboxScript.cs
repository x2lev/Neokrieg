using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxScript : BoxScript
{
    [SerializeField] bool counterhit = false;
    private void Awake()
    {
        if (counterhit)
            gizmoColor = Color.cyan;
        else
            gizmoColor = Color.green;
    }
}
