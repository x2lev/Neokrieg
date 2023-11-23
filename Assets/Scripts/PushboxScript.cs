using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushboxScript : BoxScript
{
    private void Awake()
    {
        gizmoColor = Color.yellow;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject box= collision.collider.gameObject;
        GameObject parent= box.transform.parent.gameObject;
        Collider2D thisCollider= collision.otherCollider;
        Collider2D otherCollider= collision.collider;
        Debug.Log("Hitting the " + box.name + " of " + parent.name);
        if (box.name == "Pushbox")
        {
            Bounds b1 = thisCollider.bounds;
            Bounds b2 = otherCollider.bounds;
            float overlapRight = (b1.center.x + b1.extents.x - b2.center.x + b2.extents.x) / 2;
            float overlapLeft = (b2.center.x + b2.extents.x - b1.center.x + b1.extents.x) / 2;
            if (overlapRight < overlapLeft)
                player.transform.position -= new Vector3(overlapRight, 0, 0);
            else
                player.transform.position += new Vector3(overlapLeft, 0, 0);
        }
        else if (box.name == "Edge")
        {
            if (parent.name == "Floor")
            {
                player.GetComponent<PlayerScript>().grounded = true;
                player.GetComponent<PlayerScript>().velocity.y = 0;
                player.transform.position += new Vector3(0, otherCollider.bounds.center.y - thisCollider.bounds.min.y, 0);
            }
            else if (parent.name == "Ceiling")
            {
                player.GetComponent<PlayerScript>().velocity.y = 0;
                player.transform.position -= new Vector3(0, thisCollider.bounds.max.y - otherCollider.bounds.center.y, 0);
            }
            else
            {
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
                player.GetComponent<PlayerScript>().grounded = false;
            }
        }
    }
}
