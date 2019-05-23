using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertColours : MonoBehaviour
{
    public Material material;
    public float invertDuration = 10.0f;

    private bool m_isInverted = false;
    private float m_timer = 0;

    private void Update()
    {
        if (m_isInverted)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= invertDuration)
            {
                material.SetColor("TintColor", Color.black);
                m_isInverted = false;
                m_timer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        material.SetColor("TintColor", Color.white);
        m_isInverted = true;
    }

    private void OnApplicationQuit()
    {
        material.SetColor("TintColor", Color.black);
        m_isInverted = false;
    }
}