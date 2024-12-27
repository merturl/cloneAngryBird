using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Camera _cam;
    public float health = 10.0f;
    public static int enemyCount = 0;
    [SerializeField] public GameManager gameManager;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        _cam = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCount++;
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsOutOfBounds())
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        health-= 1;
        _spriteRenderer.color = Color.red;
        if (health <= 0)
        {
            Debug.Log(other.gameObject.name);
            Die();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _spriteRenderer.color = Color.white;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _spriteRenderer.color = Color.white;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var impactForce = collision.relativeVelocity.magnitude;
        // 충돌 세기 (속도 벡터의 크기)
        if (impactForce > 3.0f)
        {
            health-= impactForce;
            _spriteRenderer.color = Color.red;
            if (health <= 0)
            {
                Die();
            }
        }
    }
    
    private bool IsOutOfBounds()
    {
        var screenPosition = _cam.WorldToViewportPoint(transform.position);
        return screenPosition.x < 0 || screenPosition.x > 1 || screenPosition.y < 0 || screenPosition.y > 1;
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        enemyCount--;
        if (enemyCount <= 0 && gameManager)
        {
            Debug.Log("GameClear");
            gameManager.GameClear();
        }
    }
}
