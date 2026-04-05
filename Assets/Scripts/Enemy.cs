using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IStatProvider
{
    private enum Direction { Up, Down, Left, Right }
    private enum EnemyState { Calculating, Moving, Pulsating }

    // Movement
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D enemyRb;
    private List<Direction> paths = new List<Direction>(); // valid directions populated each time the enemy stops
    [SerializeField] private float enemySpeed;
    public float BaseSpeed { get; private set; }
    bool isMoving = false; // tracks actual movement independently of pulsating

    // State
    [SerializeField] private EnemyState currentState = EnemyState.Calculating;

    void Awake()
    {
        enemyRb   = GetComponent<Rigidbody2D>();
        BaseSpeed = enemySpeed;
    }

    // IStatProvider — called by SkillsManager to read owner values
    public float GetStat(string stat)
    {
        switch (stat)
        {
            case "speed": return enemySpeed;
            default: return 0f;
        }
    }

    // IStatProvider — called by SkillsManager when a skill starts or stops
    public void OnSkillActivated(SkillDefinition.SkillType skill)
    {
        if (skill == SkillDefinition.SkillType.Explode)
            currentState = EnemyState.Pulsating;
    }

    public void OnSkillDeactivated(SkillDefinition.SkillType skill)
    {
        if (skill == SkillDefinition.SkillType.Explode)
            currentState = isMoving ? EnemyState.Moving : EnemyState.Calculating;
    }

    void FixedUpdate()
    {
        // Movement logic always runs, even while pulsating
        if (!isMoving)
        {
            // Cache position once to avoid repeated native calls
            Vector2 pos = transform.position;

            RaycastHit2D upR    = Physics2D.Raycast(pos, Vector2.up,    Mathf.Infinity, wallLayer);
            RaycastHit2D downR  = Physics2D.Raycast(pos, Vector2.down,  Mathf.Infinity, wallLayer);
            RaycastHit2D leftR  = Physics2D.Raycast(pos, Vector2.left,  Mathf.Infinity, wallLayer);
            RaycastHit2D rightR = Physics2D.Raycast(pos, Vector2.right, Mathf.Infinity, wallLayer);

            Debug.DrawRay(pos, Vector2.up    * upR.distance,    Color.green);
            Debug.DrawRay(pos, Vector2.down  * downR.distance,  Color.red);
            Debug.DrawRay(pos, Vector2.left  * leftR.distance,  Color.blue);
            Debug.DrawRay(pos, Vector2.right * rightR.distance, Color.yellow);

            // Only add directions where there is room to move (wall further than 2 units)
            TryAddPath(upR,    pos, Direction.Up);
            TryAddPath(downR,  pos, Direction.Down);
            TryAddPath(leftR,  pos, Direction.Left);
            TryAddPath(rightR, pos, Direction.Right);

            if (paths.Count != 0)
            {
                // Pick a random valid direction and launch the enemy that way,
                // freezing the unused axis to prevent diagonal drift
                Direction path = paths[Random.Range(0, paths.Count)];
                switch (path)
                {
                    case Direction.Up:
                        enemyRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                        enemyRb.linearVelocity = new Vector2(0, enemySpeed * Time.fixedDeltaTime);
                        break;
                    case Direction.Down:
                        enemyRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                        enemyRb.linearVelocity = new Vector2(0, -enemySpeed * Time.fixedDeltaTime);
                        break;
                    case Direction.Left:
                        enemyRb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                        enemyRb.linearVelocity = new Vector2(-enemySpeed * Time.fixedDeltaTime, 0);
                        break;
                    case Direction.Right:
                        enemyRb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                        enemyRb.linearVelocity = new Vector2(enemySpeed * Time.fixedDeltaTime, 0);
                        break;
                }
            }
        }

        bool wasMoving = isMoving;
        isMoving = enemyRb.linearVelocity.sqrMagnitude > 0.01f; // 0.01 = 0.1 squared, avoids sqrt

        // Clear paths once when the enemy comes to a stop, ready for the next decision
        if (!isMoving && wasMoving)
            paths.Clear();

        // Keep currentState in sync for external observers, but don't override Pulsating
        if (currentState != EnemyState.Pulsating)
            currentState = isMoving ? EnemyState.Moving : EnemyState.Calculating;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            Destroy(gameObject);
    }

    // Adds direction to paths if the wall in that direction is far enough to move into
    // Uses sqrMagnitude (> 4 = distance > 2) to avoid a sqrt call
    void TryAddPath(RaycastHit2D hit, Vector2 pos, Direction direction)
    {
        if (hit.collider != null && (hit.point - pos).sqrMagnitude > 4f)
            paths.Add(direction);
    }
}
