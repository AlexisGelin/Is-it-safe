using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float nbrJump = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState { idle, running, jumping ,doublejumping , falling , walljump}

    [SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && nbrJump == 0 || Input.GetButtonDown("Jump") && nbrJump == 1)
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            nbrJump++;
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
            if (nbrJump == 2)
            {
                state = MovementState.doublejumping;
            }
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        else
        {
            nbrJump = 0;
        }

        if (IsGroundedSide())
        {
            state = MovementState.walljump;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGroundedSide()
    {
        if (sprite.flipX)
        {
            return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.left, .1f, jumpableGround);
        } else
        {
            return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.right, .1f, jumpableGround);
        }
    }
}