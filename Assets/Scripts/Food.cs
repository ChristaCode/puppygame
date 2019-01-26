﻿using UnityEngine;

public class Food : MonoBehaviour
{
    Animator anim;                              // Reference to the animator component.

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.main.PlayerSize += 1;

            if (Player.main.shell == null) // if no shell, heal player
            {
                Player.main.CurrentHealth += 10;

                if (Player.main.CurrentHealth >= Player.main.MaxHealth - 10)
                    Player.main.CurrentHealth = Player.main.MaxHealth;
            }
            else // if has shell on, heal shell
            {
                Player.main.shell.currentHealth += 10;

                if (Player.main.shell.currentHealth >= Player.main.shell.MAX_HEALTH - 10)
                    Player.main.shell.currentHealth = Player.main.shell.MAX_HEALTH;
            }
                
            Destroy(gameObject);
        }
    }
}

