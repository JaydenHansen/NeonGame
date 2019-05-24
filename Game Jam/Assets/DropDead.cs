using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDead : MonoBehaviour
{

    public PlayerController player;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Drop")
        {
            player.currentHealth = 0;
        }
    }
}
