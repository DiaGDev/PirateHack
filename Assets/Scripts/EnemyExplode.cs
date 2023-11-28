using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyExplode : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private bool isPulsating = false;
    private float timer = 0f;
    private float pulsateDuration = 3f; 
    private float pulsateInterval = 9f;
    float enemySpeed = 0.0f;
    Enemy enemyScript;
    public ParticleSystem explosionParticles;
    private Vector3 enemyPosition;
    [SerializeField] private LayerMask blockerLayer;
    Collider2D[] blockerList;

    private void Start()
    {
        enemyScript = GetComponent<Enemy>();
        enemySpeed = enemyScript.speed;
    }

    void Update()
    {
        
        timer += Time.deltaTime;
        enemyScript.speed = enemySpeed;
        enemyPosition = transform.position;

        if (!isPulsating && timer >= pulsateInterval)
        {
            isPulsating = true;
            timer = 0f;
            StartCoroutine(Pulsate());
        }
    }
    IEnumerator Pulsate()
    {
        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;
        

        while (elapsedTime < pulsateDuration)
        {
            enemyScript.speed = enemySpeed - 500;
            if (elapsedTime == pulsateInterval)
            {
                enemyScript.speed = enemySpeed;
            }
            float scaleFactor = Mathf.PingPong(elapsedTime * 3, 0.2f) + 0.8f;
            transform.localScale = originalScale * scaleFactor;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        //Explode
        if (explosionParticles != null)
        {
            Instantiate(explosionParticles, enemyPosition, Quaternion.identity);
            explosionParticles.Play();
            blockerList = Physics2D.OverlapCircleAll(enemyPosition, 3, blockerLayer);
            foreach (var blocker in blockerList)
            {
                Destroy(blocker.gameObject);
            }
        }
         // Start the particle system
        isPulsating = false;
    }


}
