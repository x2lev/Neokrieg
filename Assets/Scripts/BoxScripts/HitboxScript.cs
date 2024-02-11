using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxScript : BoxScript
{
    public Dictionary<string, Hitbox> hitboxes = new();
    private void Awake()
    {
        gizmoColor = Color.red;
    }
    public override void AddCollider(string tag, Vector2 offset, Vector2 size)
    {
        AddCollider(tag, offset, size, Vector2.zero, 0);
    }
    public void AddCollider(string tag, Vector2 offset, Vector2 size, Vector2 angle, int priority)
    {
        base.AddCollider(tag, offset, size);
        hitboxes[tag] = new(angle, priority);
    }
    public override void RemoveCollider(string tag)
    {
        base.RemoveCollider(tag);
        if (hitboxes.ContainsKey(tag))
            hitboxes.Remove(tag);
    }
    public override void ClearColliders()
    {
        base.ClearColliders();
        hitboxes.Clear();
    }
}
