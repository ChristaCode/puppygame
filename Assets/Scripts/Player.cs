﻿using UnityEngine;

public class Player : Singleton<Player> {
    
    public float MaxSpeed = 10f;
    public float PlayerSize = 1f;
    public float CurrentSpeed;
    public float SHELL_GRAB_RANGE = 1f;

    public ShellParent shell {get{return _shell;} set{OnWearShell(value);}}
    private ShellParent _shell;

    float MoveHorizontal;
    float MoveVertical;
    bool facingRight;
    Vector3 targetPos;
    Animator crabAnimations;
    float idleTimer;



    Rigidbody _rb;

    SpriteRenderer _mySprite;

    void Start ()
    {
        crabAnimations = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

    }


    void Update()
    {
        if (MoveHorizontal > 0f)
            facingRight = true;
        else if (MoveHorizontal < 0f)
            facingRight = false;

            if (facingRight != transform.localScale.x > 0f)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
    }

    void FixedUpdate()
    {

        //if (SceneMgr.Instance.GameOver || SceneMgr.Instance.DisableMovement)
        //{
        //    GetComponent<Rigidbody2D>().Sleep();
        //    MoveHorizontal = 0f;
        //    MoveVertical = 0f;
        //    return;
        //}

        //for Keyboard Input/////////////////////////////////////////////////////////////////////////////////////////////
        MoveHorizontal = Input.GetAxis("Horizontal");
        MoveVertical = Input.GetAxis("Vertical");
        bool actionPressed = Input.GetKeyDown(KeyCode.Space);

        _rb.velocity = new Vector3(MoveHorizontal * MaxSpeed, _rb.velocity.y, MoveVertical * MaxSpeed);
        CurrentSpeed = MoveVertical * MaxSpeed;

        if (crabAnimations != null) {
            float animationSpeed = Vector2.ClampMagnitude(new Vector2(MoveHorizontal, MoveVertical).normalized, 1f).magnitude;
            crabAnimations.SetFloat("Speed", animationSpeed);
        }
        if (MoveVertical == 0 && MoveHorizontal == 0)
        {
            idleTimer += Time.deltaTime;
        }
        else {
            idleTimer = 0f;    
        }

        if (crabAnimations != null && idleTimer >= 5f)
            crabAnimations.SetTrigger("Sit");

        if (actionPressed) {
            OnTryWearShell();
        }

    }    

    private void OnTryWearShell() {
        if (shell != null) {
            shell = null;
            return;
        }

        ShellParent[] shells = FindObjectsOfType<ShellParent>();
        foreach (ShellParent currentShell in shells) {
            float distance = (transform.position - currentShell.transform.position).magnitude;
            if (distance <= SHELL_GRAB_RANGE) {
                shell = currentShell;
            }
        }
    }

    private void OnWearShell(ShellParent newShell) {
        if (newShell != null) {
            newShell.Attach(transform);
        } else {
            _shell.Drop();
        }
        _shell = newShell;
    }

    private void DropShell() {
        shell = null;
    }
}
