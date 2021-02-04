using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private GameManager _gm;

    private Animator _anim;

    [SerializeField] private GameObject lastPlatformHit;
    private Rigidbody2D _rb;
    private EnvironmentController envController;

    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkGroundedRadius;
    [SerializeField] private LayerMask groundLayer;

    private float jumpTimeCounter;
    [SerializeField] private float JumpMaxDuration;

    private bool isJumping = false;
    [SerializeField] private float jumpFoceMultiplier = 1f;

    protected virtual void Awake()
    {
        _gm = FindObjectOfType<GameManager>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Start() {
        envController = FindObjectOfType<EnvironmentController>();
        lastPlatformHit = FindObjectOfType<FirstPlatform>().gameObject;

        
    }
    // Update is called once per frame
    
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkGroundedRadius, groundLayer);
        
        Jump();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            jumpFoceMultiplier = _gm.greenPlatformJumpMultiplier;
        }if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            jumpFoceMultiplier = _gm.redPlatformJumpMultiplier;
        }if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetJumpMultiplier();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(envController.BlackenPlatforms());

        }

    }
    
    private void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            _anim.SetTrigger("Jump");
            _rb.velocity = Vector2.up * _gm.playerJumpForce * jumpFoceMultiplier * Time.deltaTime;
            isJumping = true;
            jumpTimeCounter = JumpMaxDuration;
            
        }

        if (Input.GetButton("Jump") && jumpTimeCounter>0 && isJumping)
        {
            _rb.velocity = Vector2.up * _gm.playerJumpForce * jumpFoceMultiplier * Time.deltaTime;
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    
    public void StartRunning()
    {
        
        _anim.SetBool("isRunning", true);
    }

    public void ResetJumpMultiplier()
    {
        jumpFoceMultiplier = 1;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Platform") && isGrounded)
        {
            _anim.SetTrigger("Land");
            
            ResetJumpMultiplier();
            
            Platform platform = other.gameObject.GetComponent<Platform>();

            if (platform.type == Platform.platformType.Green)
            {
                jumpFoceMultiplier = _gm.greenPlatformJumpMultiplier;
            }
            else if (platform.type == Platform.platformType.Red)
            {
                jumpFoceMultiplier = _gm.redPlatformJumpMultiplier;
            }
            else if (platform.type == Platform.platformType.Black)
            {
                StartCoroutine(envController.BlackenPlatforms());
            }

            if (other.gameObject != lastPlatformHit)
            {
                _gm.addPlatformToScore();
                lastPlatformHit = other.gameObject;
            }
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("PlayerDeath"))
        {
            _gm.GameOver();
        }
    }
    
}
