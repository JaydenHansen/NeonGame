using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{

    private CharacterController m_controller;

    private void Awake()
    {
        m_controller = GetComponent<CharacterController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }
}
