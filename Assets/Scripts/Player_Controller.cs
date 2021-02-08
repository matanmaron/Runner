using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //private int jumpCount = 0;
    
    private Animator _anim;
    private VFX_Manager vfxManager;

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

    //observers
    public static event Action OnPlayerJump; 
    public static event Action OnPlayerDead;

    private const string JUMP = "Fire1";

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Start() {
        envController = FindObjectOfType<EnvironmentController>();
        lastPlatformHit = FindObjectOfType<FirstPlatform>().gameObject;
        vfxManager = FindObjectOfType<VFX_Manager>();

        
    }
    // Update is called once per frame
    
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkGroundedRadius, groundLayer);
        
        GetJumpPromt();

    }
    
    private void GetJumpPromt()
    {
        if (isGrounded && Input.GetButtonDown(JUMP))
        {
            //jumpCount++;
            _anim.SetTrigger("Jump");
            OnPlayerJump?.Invoke();
            isJumping = true;
            jumpTimeCounter = JumpMaxDuration;
        }

        if (Input.GetButton(JUMP) && jumpTimeCounter>0 && isJumping)
        {
            
        }
    }
    private void FixedUpdate() {
        if (isJumping)
        {
            JumpPhysics();
        }
    }

    private void JumpPhysics()
    {

        _rb.velocity = Vector2.up * GameManager.Instance.playerJumpForce * jumpFoceMultiplier;
            
        if (Input.GetButton(JUMP) && jumpTimeCounter>0 && isJumping)
        {
            _rb.velocity = Vector2.up * GameManager.Instance.playerJumpForce * jumpFoceMultiplier;
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }

        if (Input.GetButtonUp(JUMP))
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
            //vfxManager.PlayerLandEffect(feetPos.position);
            
            ResetJumpMultiplier();
            
            Platform platform = other.gameObject.GetComponent<Platform>();

            if (platform.type == Platform.PlatformType.Green)
            {
                jumpFoceMultiplier = GameManager.Instance.greenPlatformJumpMultiplier;
            }
            else if (platform.type == Platform.PlatformType.Red)
            {
                jumpFoceMultiplier = GameManager.Instance.redPlatformJumpMultiplier;
            }
            else if (platform.type == Platform.PlatformType.Black)
            {
                StartCoroutine(envController.BlackenPlatforms());
            }

            if (other.gameObject != lastPlatformHit)
            {
                GameManager.Instance.addPlatformToScore();
                lastPlatformHit = other.gameObject;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("PlayerDeath"))
        {
            OnPlayerDead?.Invoke();
            GameManager.Instance.GameOver();
        }
    }
    
}
