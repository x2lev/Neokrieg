using System.Collections;
using System.Collections.Generic;
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
    public void AddCollider(string tag, Vector2 offset, Vector2 size)
    {
        boxes.Add(tag, new() { offset = offset, size = size, isTrigger = true });
    }
    public void RemoveCollider(string tag) 
    {
        boxes.Remove(tag);
    }
    public void ClearColliders()
    { 
        boxes.Clear();
    }
    public void DrawBox(BoxCollider2D box)
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
        Gizmos.color = gizmoColor.WithAlpha(.1f);
        Gizmos.DrawCube(box.bounds.center, box.bounds.size);
    }
    private void OnDrawGizmos()
    {
        foreach (BoxCollider2D box in boxes.Values)
            DrawBox(box);
    }
}
