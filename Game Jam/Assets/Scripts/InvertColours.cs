using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertColours : MonoBehaviour
{
    public Material material;
    private bool m_isInverted = false;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!m_isInverted)
            {
                material.SetColor("TintColor", Color.black);
                m_isInverted = true;
            }
            else
            {
                material.SetColor("TintColor", Color.white);
                m_isInverted = false;
            }
        }
    }
}