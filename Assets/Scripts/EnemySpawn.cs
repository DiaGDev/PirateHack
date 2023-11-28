using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    Vector2Int randomPoint;
    Vector3Int spawnPoint;
    public Vector3Int bottomLeft;
    public Vector3Int bottomRight;
    public Vector3Int topLeft;
    public Vector3Int topRight;

    void Start()
    {

        InvokeRepeating("SpawnEnemy", 15f, 15f);
    }

    void SpawnEnemy()
    {
        if (GameObject.FindObjectOfType<Enemy>() == null)
        {
            randomPoint = new Vector2Int(Random.Range(bottomLeft.x, bottomRight.x), Random.Range(bottomLeft.y, topLeft.y));
            randomPoint = CalculatePoint(randomPoint);
            spawnPoint = new Vector3Int(randomPoint.x, randomPoint.y, 0);
            Debug.Log(spawnPoint);
            bool objectFound = CheckForObjectAtPosition(spawnPoint);
            if (!objectFound)
            {
                Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            }
            else
            {
                SpawnEnemy();
            }
        }
    }

    Vector2Int CalculatePoint(Vector2Int point)
    {
        int X = point.x;
        int Y = point.y;

        if (Mathf.Abs(point.x) <= Mathf.Abs(point.y))
        {
            if (point.x < 0)
            {
                X = -8;
            }
            else
            {
                X = 21;
            }
        }
        else
        {
            if (point.y < 0)
            {
                Y = -13;
            }
            else
            {
                Y = 18;
            }
        }

        return new Vector2Int(X, Y);
    }

    bool CheckForObjectAtPosition(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Blocker") || collider.CompareTag("Player"))
            {
                return true; 
            }
        }

        return false; 
    }
}
