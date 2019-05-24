using UnityEngine;

/// <summary>
/// Restores player health on contact
/// </summary>
public class HealthPickup : MonoBehaviour
{
    /// <summary>
    /// The amount of health restored on contact.
    /// </summary>
    public int healthRestoreAmount = 10;

    /// <summary>
    /// Passes the restore amount into the player restore health function on collision with a player.
    /// </summary>
    /// <param name="other">The object being collided with.</param>
    private void OnTriggerEnter(Collider other)
    {
        // checks if the object is a player
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().RestoreHealth(healthRestoreAmount);
            Destroy(gameObject);
        }
    }
}