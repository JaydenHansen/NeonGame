using UnityEngine;

/// <summary>
/// Triggers the player first person mind break.
/// </summary>
public class FirstPerson : MonoBehaviour
{
    /// <summary>
    /// Checks if a player collided with the trigger and changes the camera to first person if so.
    /// </summary>
    /// <param name="other">The object colliding with the trigger volume.</param>
    private void OnTriggerEnter(Collider other)
    {
        // checks if it is a player
        if (other.tag == "Player")
        {
            other.GetComponent<MindBreakManager>().FirstPerson();
        }
    }
}