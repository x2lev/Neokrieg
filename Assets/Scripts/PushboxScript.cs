using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PushboxScript : BoxScript
{
    [SerializeField] GameObject _collider;
    Dictionary<string, BoxCollider2D> colliderBoxes;
    public override void AddCollider(string tag, Vector2 offset, Vector2 size)
    {
        base.AddCollider(tag, offset, size);
        colliderBoxes.Add(tag, new() { offset = offset, size = size, isTrigger = false });
    }
    public override void RemoveCollider(string tag)
    {
       base.RemoveCollider(tag);
    }
    public override void ClearColliders()
    {
        base.ClearColliders();
    }
}
