﻿using UnityEngine;

/// <summary>
/// Triggers the player invert colours mind break.
/// </summary>
public class InvertColours : MonoBehaviour
{
    /// <summary>
    /// Checks if a player collided with the trigger and changes the colour if so.
    /// </summary>
    /// <param name="other">The object colliding with the trigger volume.</param>
    private void OnTriggerEnter(Collider other)
    {
        // checks if it is a player
        if (other.tag == "Player")
        {
            other.GetComponent<MindBreakManager>().InvertColours();
        }
    }
}