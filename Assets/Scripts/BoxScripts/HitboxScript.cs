using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxScript : BoxScript
{
    public Dictionary<string, Vector2> angles = new();
    public Dictionary<string, int> priorities = new();
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
        angles[tag] = angle;
        priorities[tag] = priority;
    }
    public void AddCollider(Box box)
    {
        Hitbox hitbox = (Hitbox) box;
        AddCollider(box.tag, box.offset, box.size, hitbox.angle, hitbox.priority);
    }
    public override void SetColliders(List<Box> boxes) {
        
    }
    public override void RemoveCollider(string tag)
    {
        if (boxes.ContainsKey(tag))
        {
            base.RemoveCollider(tag);
            angles.Remove(tag);
            priorities.Remove(tag);
        }
    }
    public override void ClearColliders()
    {
        base.ClearColliders();
        angles.Clear();
        priorities.Clear();
    }
}
