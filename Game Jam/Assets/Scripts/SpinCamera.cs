using UnityEngine;

/// <summary>
/// Triggers the player spin camera mind break.
/// </summary>
public class SpinCamera : MonoBehaviour
{
    /// <summary>
    /// Checks if a player collided with the trigger and spins the camera if so.
    /// </summary>
    /// <param name="other">The object colliding with the trigger volume.</param>
    private void OnTriggerEnter(Collider other)
    {
        // checks if it is a player
        if (other.tag == "Player")
        {
            other.GetComponent<MindBreakManager>().SpinCamera();
        }
    }
}