using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [SerializeField] List<BoxCollider2D> defaultBoxes = new();
    Dictionary<string, BoxCollider2D> boxes;
    void Start()
    {
        for (int i = 0; i < defaultBoxes.Count; i++)
            boxes.Add("default-" + i, defaultBoxes[i]);
    }
    public virtual void AddCollider(string tag, Vector2 offset, Vector2 size)
    {
        boxes.Add(tag, new() { offset = offset, size = size, isTrigger = true });
    }
    public virtual void RemoveCollider(string tag) 
    {
        boxes.Remove(tag);
    }
    public virtual void ClearColliders()
    { 
        boxes.Clear(); 
    }
}
