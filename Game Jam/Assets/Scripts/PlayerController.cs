using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController m_controller;
    private Vector3 m_moveDirection;

    public float jumpSpeed;
    public float moveSpeed;
    public float gravity;
    public int damageDone;

    [Range(0, 100)]
    private int m_maxHealth;
    [Range(0, 100)]
    public int currentHealth;

    void Awake()
    {
        m_controller = GetComponent<CharacterController>();
        m_maxHealth = currentHealth;
        m_moveDirection = Vector3.zero;
    }

    void Update()
    {
        PlayerControl();
    }

    void PlayerControl() {
        if (this.gameObject != null) {
            if (m_controller.isGrounded)
            {
                m_moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
                m_moveDirection *= moveSpeed;

                if (Input.GetKey(KeyCode.Space))
                {
                    m_moveDirection.y = jumpSpeed;
                }
            }
                m_moveDirection.y -= gravity * Time.deltaTime;
                m_controller.Move(m_moveDirection * Time.deltaTime);
        }
    }

    public void RestoreHealth(int restoreAmount)
    {
        if (currentHealth < m_maxHealth)
        {
            currentHealth += restoreAmount;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            m_controller.Move(Vector3.zero);
            currentHealth -= damageDone;
        }

        if (other.gameObject.tag == "Drop") {
            currentHealth = 0;
        }
    }

}
