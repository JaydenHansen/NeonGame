using UnityEngine;

/// <summary>
/// Manages the platform movement based on the type of platform.
/// </summary>
public class PlatformManager : MonoBehaviour
{
    /// <summary>
    /// Used to distinguish between different types of platforms.
    /// </summary>
    public enum PlatformType
    {
        Straight,
        Tunnel,
        Curved
    }

    /// <summary>
    /// The movement speed of the platform.
    /// </summary>
    [Tooltip("The movement speed of the platform.")]
    public float moveSpeed = 10.0f;
    /// <summary>
    /// Rotation speed of the chunk.
    /// </summary>
    [Tooltip("Rotation speed of the chunk.")]
    public float rotSpeed = 10.0f;
    /// <summary>
    /// The type of platform.
    /// </summary>
    [Tooltip("The type of platform.")]
    public PlatformType platformType = PlatformType.Straight;
    /// <summary>
    /// The duration before the object is destroyed after it goes out of view.
    /// </summary>
    [Tooltip("The duration before the object is destroyed after it goes out of view.")]
    public float lifeTime;

    /// <summary>
    /// The location where the next platform will spawn.
    /// </summary>
    [Tooltip("The location where the next platform will spawn.")]
    public Transform spawnPoint;

    /// <summary>
    /// Stored move direction to reduce memory footprint.
    /// </summary>
    private Vector3 m_moveDirection = new Vector3(0, 0, -1);

    /// <summary>
    /// Moves the platform each frame.
    /// </summary>
    private void Update()
    {
        // moves the platform down towards the player
        transform.Translate(m_moveDirection * moveSpeed * Time.deltaTime, Space.World);
        // destroys the platform once it is out of view
        if (spawnPoint.position.z < 0.0f)
        {
            Destroy(gameObject, lifeTime);
        }
        if (platformType == PlatformType.Curved)
        {
            if (transform.position.z < 0.0f)
            {
                if (spawnPoint.position.z > 0.0f)
                {

                }
            }
        }
        // once the player gets to the curved platform it will start rotating
        if (platformType == PlatformType.Curved && transform.position.z < 0.0f && spawnPoint.position.z > 0.0f)
        {
            GetComponentInParent<EndlessPlatformGenerator>().Rotate(rotSpeed);
        }
    }
}