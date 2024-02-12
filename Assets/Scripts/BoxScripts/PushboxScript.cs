using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class PushboxScript : BoxScript
{
    private void Awake()
    {
        gizmoColor = Color.yellow;
    }
    private void FixedUpdate()
    {
        if (player.transform.position.y + playerScript.velocity.y * Time.deltaTime < -31 / 6f && !playerScript.grounded)
            playerScript.IdleBox(this);
    }
    public override void AddCollider(string tag, Vector2 offset, Vector2 size)
    {
        base.AddCollider(tag, offset, size);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionStay2D(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject box = collision.collider.gameObject;
        GameObject parent = box.transform.parent.gameObject;
        Collider2D thisCollider = collision.otherCollider;
        Collider2D otherCollider = collision.collider;
        // Debug.Log("Hitting the " + box.name + " of " + parent.name);
        if (box.name == "Pushbox")
        {
            if (parent.GetComponent<PlayerScript>().backToWall == 0)
            {
                if (playerScript.direction == 1 && playerScript.backToWall != -1)
                    player.transform.position -= new Vector3((thisCollider.bounds.max.x - otherCollider.bounds.min.x) / 2, 0, 0);
                else if (playerScript.direction == -1 && playerScript.backToWall != 1)
                    player.transform.position += new Vector3((otherCollider.bounds.max.x - thisCollider.bounds.min.x) / 2, 0, 0);
            }
            else
            {
                if (playerScript.direction == 1)
                    player.transform.position -= new Vector3(thisCollider.bounds.max.x - otherCollider.bounds.min.x, 0, 0);
                else if (playerScript.direction == -1)
                    player.transform.position += new Vector3(otherCollider.bounds.max.x - thisCollider.bounds.min.x, 0, 0);
            }
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
            else if (parent.name == "Right Wall")
            {
                if (playerScript.direction == -1)
                    playerScript.backToWall = 1;
                player.transform.position -= new Vector3(thisCollider.bounds.max.x - otherCollider.bounds.min.x, 0, 0);
            }
            else if (parent.name == "Left Wall")
            {
                if (playerScript.direction == 1)
                    playerScript.backToWall = -1;
                player.transform.position += new Vector3(otherCollider.bounds.max.x - thisCollider.bounds.min.x, 0, 0);
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
                // SceneView.RepaintAll();
            }
            else if (parent.name == "Ceiling")
            { }
            else
            {
                playerScript.backToWall = 0;
            }
        }
    }
}
