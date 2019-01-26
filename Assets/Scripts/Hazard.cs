﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private float damage;
    public Type type = Type.Rock;

    public enum Type
    {
        Rock,
        Pebble,
        Fish,
    };

    void Start()
    { 
        Debug.Log("Hazard/onTriggerEnter");
        switch (type)
        {
            case Type.Rock:
                damage = 20;
                break;
            case Type.Pebble:
                damage = 0;
                break;
            case Type.Fish:
                damage = 100;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hazard/onTriggerEnter");

            if (Player.Instance.shell == null)
            {
                //death out of shell
                Player.Instance.CurrentHealth = 0;
            }
            else
            {
                if (Player.Instance.shell.currentHealth > 0)
                {
                    Player.Instance.shell.currentHealth -= damage * Player.Instance.shell.DMG_MULT;
                }
            }
        }
    }
}
