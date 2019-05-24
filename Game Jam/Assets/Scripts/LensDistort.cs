using UnityEngine;

/// <summary>
/// Triggers the player lens distort mind break.
/// </summary>
public class LensDistort : MonoBehaviour
{
    /// <summary>
    /// Checks if a player collided with the trigger and changes the camera lens distortion if so.
    /// </summary>
    /// <param name="other">The object colliding with the trigger volume.</param>
    private void OnTriggerEnter(Collider other)
    {
        // checks if it is a player
        if (other.tag == "Player")
        {
            other.GetComponent<MindBreakManager>().LensDistortion();
        }
    }
}