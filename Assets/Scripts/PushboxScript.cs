using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PushboxScript : BoxScript
{
    public bool touchingWall;
    private void Awake()
    {
        gizmoColor = Color.yellow;
    }
    public override void AddCollider(string tag, Vector2 offset, Vector2 size)
    {
        base.AddCollider(tag, offset, size);
        boxes[tag].isTrigger = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject box= collision.collider.gameObject;
        GameObject parent= box.transform.parent.gameObject;
        Collider2D thisCollider= collision.otherCollider;
        Collider2D otherCollider= collision.collider;
        // Debug.Log("Hitting the " + box.name + " of " + parent.name);
        if (box.name == "Pushbox")
        {
            Bounds b1 = thisCollider.bounds;
            Bounds b2 = otherCollider.bounds;
            float overlapRight = (b1.center.x + b1.extents.x - b2.center.x + b2.extents.x) / 2;
            float overlapLeft = (b2.center.x + b2.extents.x - b1.center.x + b1.extents.x) / 2;
            Debug.Log("L=" + overlapLeft + " R=" + overlapRight);
            if (overlapRight < overlapLeft)
                player.transform.position -= new Vector3(overlapRight, 0, 0);
            else
                player.transform.position += new Vector3(overlapLeft, 0, 0);
        }
        else if (box.name == "Edge")
        {
            if (parent.name == "Floor")
            {
                playerScript.grounded = true;
                playerScript.velocity.y = 0;
                player.transform.position += new Vector3(0, otherCollider.bounds.center.y - thisCollider.bounds.min.y, 0);
            }
            else if (parent.name == "Ceiling")
            {
                playerScript.velocity.y = 0;
                player.transform.position -= new Vector3(0, thisCollider.bounds.max.y - otherCollider.bounds.center.y, 0);
            }
            else
            {
                touchingWall = true;
                Bounds b1 = thisCollider.bounds;
                Bounds b2 = otherCollider.bounds;
                float overlapRight = (b1.center.x + b1.extents.x - b2.center.x + b2.extents.x) / 2;
                float overlapLeft = (b2.center.x + b2.extents.x - b1.center.x + b1.extents.x) / 2;
                if (overlapRight < overlapLeft)
                    player.transform.position -= new Vector3(overlapRight, 0, 0);
                else
                    player.transform.position += new Vector3(overlapLeft, 0, 0);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject box = collision.collider.gameObject;
        GameObject parent = box.transform.parent.gameObject;
        Collider2D thisCollider = collision.otherCollider;
        Collider2D otherCollider = collision.collider;
        if (box.name == "Edge")
        {
            if (parent.name == "Floor")
            {
                playerScript.grounded = false;
                playerScript.Jumpbox(this);
                SceneView.RepaintAll();
            }
            else if (parent.name == "Ceiling")
            { }
            else
            {
                touchingWall = false;
            }
        } 
    }
}
