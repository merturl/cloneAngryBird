using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    private Rigidbody2D _structureRigidbody;
    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _structureRigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _structureRigidbody.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ActivatePhysics();
    }
    
    public void ActivatePhysics()
    {
        _structureRigidbody.bodyType = RigidbodyType2D.Dynamic; // 공격받으면 움직이도록 설정
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);
    }
}
