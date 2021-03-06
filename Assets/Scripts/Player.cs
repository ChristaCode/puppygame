﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class Player : Singleton<Player> {

    // comment for re-adding player 

    public float MaxSpeed = 10f;
    public float PlayerSize = 1f;
    public float CurrentSpeed;
    public float SHELL_GRAB_RANGE = 1f;
    public Animator crabAnimations;
    public bool shellEquipped = true;

    public SpriteRenderer shellRenderer;
    public Sprite[] shellSprites;

    public ShellParent shell {get{return _shell;} set{OnWearShell(value);}}
    public ShellParent _shell;

    float MoveHorizontal;
    float MoveVertical;
    bool facingRight;
    Vector3 targetPos;
    float idleTimer;

    public float MaxHealth = 100;
    public float CurrentHealth = 100;

    UnityEngine.UI.Button restartButton;

    Rigidbody _rb;
    Image healthBar;

    void Start()
    {
        shell = shell; //lol
        main = this;
        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();

        //crabAnimations = GetComponent<Animator>();

        _rb = GetComponent<Rigidbody>();

        restartButton = GameObject.Find("Restart").GetComponent<UnityEngine.UI.Button>();
        restartButton.gameObject.SetActive(false);
    }

    void Update()
    {
        // if there's no shell, update player health bar
        if (shell == null && healthBar != null)
        {
            healthBar.fillAmount = (float)(CurrentHealth * .01);
        }

        if (CurrentHealth < .1)
        {
            Death();
        }

        if (Shark.main != null && Shark.main.hasTriggered)
        {
            Death();
        }

        if (MoveHorizontal > 0f)
            facingRight = true;
        else if (MoveHorizontal < 0f)
            facingRight = false;

            if (facingRight != transform.localScale.x > 0f)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
    }

    void Death()
    {
        restartButton.gameObject.SetActive(true);
        Destroy(gameObject);
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
            float animationSpeed = Vector2.ClampMagnitude(new Vector2(MoveHorizontal, MoveVertical), 1f).magnitude;
            crabAnimations.SetFloat("Speed", animationSpeed);
        }
        if (MoveVertical == 0 && MoveHorizontal == 0)
        {
            idleTimer += Time.deltaTime;
        }
        else {
            idleTimer = 0f;    
        }

        //if (crabAnimations != null && idleTimer >= 5f)
            //crabAnimations.SetTrigger("Sit");

        if (actionPressed) {
            OnTryWearShell();
        }

    }    

    private void OnTryWearShell() {
        if (_shell != null) {
            shell = null;
            Debug.Log("Had a shell, dropped");
            return;
        }

        List<ShellParent> shells = FindObjectsOfType<ShellParent>().ToList();
        shells = shells.OrderBy(
            x => Vector3.Distance(this.transform.position,x.transform.position)
        ).ToList();


        foreach (ShellParent currentShell in shells) {
            float distance = (transform.position - currentShell.transform.position).magnitude;
            Debug.Log("Shell at " + distance);
            if (distance <= SHELL_GRAB_RANGE) {
                shell = currentShell;
            }
            return;
        }
    }

    public Sprite GetShellSprite(ShellParent shellObj) {

        int shellIndex = (shellObj == null) ? 0 : ((int)shellObj.type + 1);
        return shellSprites[shellIndex];
    }

    private void OnWearShell(ShellParent newShell) {
        if (newShell != null) {
            newShell.Attach(transform);
        }
        if (_shell != null) {
            _shell.Drop();
        }
        
        _shell = newShell;
        UpdateShellGraphic();
    }

    private void UpdateShellGraphic() {
        shellRenderer.sprite = GetShellSprite(_shell);
    }

    private void DropShell() {
        shell = null;
    }
}
