using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //private int jumpCount = 0;
    
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
        
        GetJumpPromt();

    }
    
    private void GetJumpPromt()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            //jumpCount++;
            _anim.SetTrigger("Jump");
            isJumping = true;
            jumpTimeCounter = JumpMaxDuration;
        }

        if (Input.GetButton("Jump") && jumpTimeCounter>0 && isJumping)
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
        //if (isGrounded && Input.GetButtonDown("Jump"))
        //{
           
            _rb.velocity = Vector2.up * _gm.playerJumpForce * jumpFoceMultiplier;
            
            
        //}

        if (Input.GetButton("Jump") && jumpTimeCounter>0 && isJumping)
        {
            _rb.velocity = Vector2.up * _gm.playerJumpForce * jumpFoceMultiplier;
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
