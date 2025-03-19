using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Kirby : MonoBehaviour
{
    [Header("Jumping")]
    public float jumpPower;

    [Header("Sprites")]
    public Sprite standing;
    public Sprite squating;
    public ParticleSystem Jumpingpart;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private bool _grounded = true;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Squat();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Unsquat();

            if (_grounded)
                Jump();
        }
    }

    private void Squat()
    {
        _spriteRenderer.sprite = squating;
    }

    private void Unsquat()
    {
        _spriteRenderer.sprite = standing;
    }

    private void Jump()
    {
        _grounded = false;
        _rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        Jumpingpart.Play();
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        _grounded = true;
    }
}
