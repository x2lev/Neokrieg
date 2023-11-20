using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] float stageWidth = 30;
    [SerializeField] List<GameObject> prefabs;
    GameObject player1;
    GameObject player2;
    PlayerScript script1;
    PlayerScript script2;
    float bound;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        bound = stageWidth/2 - 10f/9*8; // (10f/9*16) is the width of the camera
        player1 = Instantiate(prefabs[Random.Range(0, prefabs.Capacity)]);
        player2 = Instantiate(prefabs[Random.Range(0, prefabs.Capacity)]);
        script1 = player1.GetComponent<PlayerScript>();
        script2 = player2.GetComponent<PlayerScript>();
        player1.transform.position = new Vector3(-5, -5.15f, -1);
        player2.transform.position = new Vector3(5, -5.15f, 0);
        script1.player1 = true;
        script2.flip();
    }
    void FixedUpdate()
    {
        Vector3 pos1 = player1.transform.position;
        Vector3 pos2 = player2.transform.position;
        float x = Mathf.Clamp((pos1.x+pos2.x)/2, -bound, bound);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);

        if (Mathf.Sign(pos2.x - pos1.x) != script1.direction)
        {
            script1.flip();
            script2.flip();
        }
    }
    private void OnDrawGizmos()
    {
        foreach(BoxCollider2D box in GetComponents<BoxCollider2D>())
        {
            switch (box.gameObject.layer)
            {
                case 0: // Default
                    Gizmos.color = Color.black;
                    break;
                case 6: // Hurtbox
                    Gizmos.color = Color.green;
                    break;
                case 7: // Hurtbox (Counterhit state)
                    Gizmos.color = Color.cyan;
                    break;
                case 8: // Hitbox
                    Gizmos.color = Color.red;
                    break;
                case 9: // Pushbox
                    Gizmos.color = Color.yellow;
                    break;
            }
        }
    }
}
