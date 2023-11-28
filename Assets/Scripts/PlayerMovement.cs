using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Direction
{
    north, south, east, west
}
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    Direction movingDir;
    public Collider2D coll;
    public GameObject blockPrefab;
    public int ammo;
    [SerializeField] bool isMoving = false;
    public TextMeshProUGUI TText;


    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        TText.text = ammo.ToString();
        if (!isMoving)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    movingDir = Direction.east;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    movingDir = Direction.west;
                }
            }
            else if (Input.GetAxisRaw("Vertical") != 0)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    movingDir = Direction.north;
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    movingDir = Direction.south;
                }
            }
            if (Input.GetMouseButtonDown(0) && ammo>0)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (coll.OverlapPoint(worldPos))
                {
                    worldPos.x = Mathf.FloorToInt(worldPos.x);
                    worldPos.y = Mathf.FloorToInt(worldPos.y);
                    Instantiate(blockPrefab, worldPos, Quaternion.identity);
                    ammo--;
                }
                
            }
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude >0.2)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        switch (movingDir)
        {
            case Direction.north:
                rb.velocity = new Vector2(0, speed*Time.fixedDeltaTime);
                break;
            case Direction.south:
                rb.velocity = new Vector2(0, -speed * Time.fixedDeltaTime);
                break;
            case Direction.east:
                rb.velocity = new Vector2(speed * Time.fixedDeltaTime, 0);
                break;
            case Direction.west:
                rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, 0);
                break;
        }
    }
}
