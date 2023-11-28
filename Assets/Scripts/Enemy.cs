using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Enemy : MonoBehaviour
{
    public LayerMask mask;
    Rigidbody2D rb;
    private Collider2D coll;
    List<string> paths = new List<string>();
    bool isMoving = false;
    public float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        RaycastHit2D upR = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, mask);
        RaycastHit2D downR = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, mask);
        RaycastHit2D leftR = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, mask);
        RaycastHit2D rightR = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, mask);
        Debug.DrawRay(transform.position, Vector2.up * upR.distance, Color.green);
        Debug.DrawRay(transform.position, Vector2.down * downR.distance, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * leftR.distance, Color.blue);
        Debug.DrawRay(transform.position, Vector2.right * rightR.distance, Color.yellow);

        if (!isMoving)
        {
            if (upR.collider != null)
            {
                float distanceUp = Vector2.Distance(transform.position, upR.point);
                if (distanceUp > 2)
                {
                    paths.Add("up");
                }
            }
            if (downR.collider != null)
            {
                float distanceDown = Vector2.Distance(transform.position, downR.point);
                if (distanceDown > 2)
                {
                    paths.Add("down");
                }
            }
            if (leftR.collider != null)
            {
                float distanceLeft = Vector2.Distance(transform.position, leftR.point);
                if (distanceLeft > 2)
                {
                    paths.Add("left");
                }
            }
            if (rightR.collider != null)
            {
                float distanceRight = Vector2.Distance(transform.position, rightR.point);
                if (distanceRight > 2)
                {
                    paths.Add("right");
                }
            }

            if (paths.Count != 0)
            {
                string path=GenerateRandomDirection(paths);
                if (path == "up") 
                {
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    rb.velocity = new Vector2(0, speed * Time.fixedDeltaTime);
                }
                if (path == "down")
                {
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    rb.velocity = new Vector2(0, -speed * Time.fixedDeltaTime);
                }
                if (path == "left")
                {
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, 0);
                }
                if (path == "right")
                {
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    rb.velocity = new Vector2(speed * Time.fixedDeltaTime, 0);
                }
            }
            
        }
        if(isMoving)
        {
            paths.Clear();
        }

        if (rb.velocity.magnitude > 0.1f)
        {
            isMoving= true;
        }
        else
        {
            isMoving= false;
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }


    string GenerateRandomDirection(List<string> paths)
    {
        string random = paths[Random.Range(0, paths.Count)];
        return random;
    }
}
