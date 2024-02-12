using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [HideInInspector] public Dictionary<string, BoxCollider2D> boxes = new();
    [HideInInspector] public Color gizmoColor = Color.black;
    [HideInInspector] public GameObject player;
    [HideInInspector] public PlayerScript playerScript;
    private void Start()
    {
        player = transform.parent.gameObject;
        playerScript = player.GetComponent<PlayerScript>();
        BoxCollider2D[] b = GetComponents<BoxCollider2D>();
        for (int i = 0; i < b.Length; i++)
            boxes.Add("default-" + i, b[i]);
    }
    public virtual void AddCollider(string tag, Vector2 offset, Vector2 size)
    {
        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        box.offset = offset;
        box.size = size;
        box.isTrigger = true;
        boxes.Add(tag, box);
    }
    public virtual void SetColliders(List<Box> boxes)
    {
        ClearColliders();
        foreach (Box b in boxes)
            AddCollider(b.tag, b.offset, b.size);
    }
    public virtual void RemoveCollider(string tag)
    {
        if (boxes.ContainsKey(tag))
        {
            Destroy(boxes[tag]);
            boxes.Remove(tag);
        }
    }
    public virtual void ClearColliders()
    {
        if (boxes.Count > 0)
        {
            foreach (BoxCollider2D box in boxes.Values.ToList())
                Destroy(box);
            boxes.Clear();
        }
    }
    public Vector3 GetCenter()
    {
        Vector3 center = Vector3.zero;
        foreach (BoxCollider2D box in boxes.Values)
            center += box.bounds.center;
        return center / boxes.Count;
    }
    private void OnDrawGizmos()
    {
        foreach (BoxCollider2D box in boxes.Values)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
            Gizmos.color = gizmoColor.WithAlpha(.1f);
            Gizmos.DrawCube(box.bounds.center, box.bounds.size);
        }
    }
}
