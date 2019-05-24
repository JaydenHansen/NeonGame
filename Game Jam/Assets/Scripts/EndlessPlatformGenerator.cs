using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates platforms in a pseudo random way.
/// </summary>
public class EndlessPlatformGenerator : MonoBehaviour
{
    /// <summary>
    /// The number of straight chunks consecutively generated before a random generation occurs again.
    /// </summary>
    [Tooltip("The number of straight chunks consecutively generated before a random generation occurs again.")]
    public int numOfStraightPlatformsBeforeRegen = 10;
    /// <summary>
    /// The number of tunnel chunks consecutively generated before a random generation occurs again.
    /// </summary>
    [Tooltip("The number of tunnel chunks consecutively generated before a random generation occurs again.")]
    public int numOfTunnelPlatformsBeforeRegen = 5;

    /// <summary>
    /// Collection of different possible straight platforms.
    /// </summary>
    [Tooltip("Collection of different possible straight platforms.")]
    public GameObject[] straightPlatforms;
    /// <summary>
    /// Collection of different possible tunnel platforms.
    /// </summary>
    [Tooltip("Collection of different possible tunnel platforms.")]
    public GameObject[] tunnelPlatforms;

    /// <summary>
    /// The location where the platform will get spawned at.
    /// </summary>
    [Tooltip("The location where the platform will get spawned at.")]
    public Transform spawnPoint;
    /// <summary>
    /// The distance between the player and the spawn point.
    /// </summary>
    [Tooltip("The distance between the player and the spawn point.")]
    public float spawnDistThreshold = 100.0f;
    /// <summary>
    /// The distance between the last chunk and the spawn point.
    /// </summary>
    [Tooltip("The distance between the last chunk and the spawn point.")]
    public float spawnSeparation = 10.0f;
    /// <summary>
    /// The object that will be instantiated on the platform.
    /// </summary>
    [Tooltip("Reference to the health pickup prefab.")]
    public GameObject healthPickup;
    /// <summary>
    /// Displacement vector from the position of the platform the pickup is on.
    /// </summary>
    [Tooltip("Position of the pickup local to the platform.")]
    public Vector3 pickupSpawnPoint;
    /// <summary>
    /// Chance of a pickup spawning.
    /// </summary>
    [Tooltip("Chance of a pickup spawning.")]
    public int pickupSpawnChance = 1;

    /// <summary>
    /// The movement speed of the platform.
    /// </summary>
    [Tooltip("The movement speed of the platform.")]
    public float moveSpeed = 10.0f;
    /// <summary>
    /// The increase in movement speed over time.
    /// </summary>
    [Tooltip("The increase in movement speed over time.")]
    public float difficultyScalar = 0.05f;
    /// <summary>
    /// Maximum platform movement speed.
    /// </summary>
    [Tooltip("Maximum platform movement speed.")]
    public float maxMovementSpeed = 100.0f;
    /// <summary>
    /// The increase in animation speed over time.
    /// </summary>
    [Tooltip("The increase in animation speed over time.")]
    public float animationScalar = 0.005f;
    /// <summary>
    /// Maximum animation speed.
    /// </summary>
    [Tooltip("Maximum animation speed.")]
    public float maxAnimationSpeed = 10.0f;

    /// <summary>
    /// Collection of all spawned chunks.
    /// </summary>
    [HideInInspector]
    public Queue<GameObject> platforms = new Queue<GameObject>();

    /// <summary>
    /// Reference to the player object.
    /// </summary>
    private GameObject m_player;
    /// <summary>
    /// Reference to the player animator.
    /// </summary>
    private Animator m_playerAnim;
    /// <summary>
    /// The number of straight chunks that have been spawned.
    /// </summary>
    private int m_numOfStraightPlatforms = 0;
    /// <summary>
    /// The number of tunnel chunks that have been spawned.
    /// </summary>
    private int m_numOfTunnelPlatforms = 0;
    /// <summary>
    /// The type of platform that is being spawned.
    /// </summary>
    private PlatformManager.PlatformType m_platformType = PlatformManager.PlatformType.Straight;

    /// <summary>
    /// Gets the player object and animator.
    /// </summary>
    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_playerAnim = m_player.GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Creates a platform at the spawn point.
    /// </summary>
    private void GeneratePlatform()
    {
        // checks if it is time to potentially start spawning a different type of platform
        if (m_numOfStraightPlatforms == numOfStraightPlatformsBeforeRegen || m_numOfTunnelPlatforms == numOfTunnelPlatformsBeforeRegen)
        {
            // chooses a random odd or even number to determine the platform type
            int typeIndex = Random.Range(-100000, 100000) % 2;
            if (typeIndex == 0)
            {
                m_platformType = PlatformManager.PlatformType.Straight;
            }
            else
            {
                m_platformType = PlatformManager.PlatformType.Tunnel;
            }
            m_numOfStraightPlatforms = 0;
            m_numOfTunnelPlatforms = 0;            
        }
        // checks if a straight platform should be created
        if (m_platformType == PlatformManager.PlatformType.Straight && m_numOfStraightPlatforms < numOfStraightPlatformsBeforeRegen)
        {
            // chooses a random type of straight platform
            int straightPlatformIndex = Random.Range(0, 100000) % straightPlatforms.Length;
            // creates a platform of the random type at the spawn point
            GameObject platform = Instantiate(straightPlatforms[straightPlatformIndex], spawnPoint.position + new Vector3(0, 0, spawnSeparation), spawnPoint.rotation);
            platform.transform.parent = transform;
            platforms.Enqueue(platform);
            // increments the count of the number of consecutive straight platforms that have been spawned
            m_numOfStraightPlatforms++;
            // sets the spawn point for the next platform to the new platform's spawn point
            spawnPoint = platform.GetComponent<PlatformManager>().spawnPoint;

            // checks if the random number generated suggests that a pickup should be spawned
            bool spawnPickup = (Random.Range(0, 100) % pickupSpawnChance == 0) ? true : false;
            if (spawnPickup)
            {
                // makes the pickup position local to the platform
                Vector3 pickupPosition = platform.transform.position + pickupSpawnPoint;
                // creates a pickup
                GameObject pickup = Instantiate(healthPickup, pickupPosition, Quaternion.identity);
                // binds the pickup to the platform
                pickup.transform.parent = platform.transform;
            }
        }
        // checks if a tunnel platform should be created
        else if (m_platformType == PlatformManager.PlatformType.Tunnel && m_numOfTunnelPlatforms < numOfTunnelPlatformsBeforeRegen)
        {
            // chooses a random type of curved platform
            int tunnelPlatformIndex = Random.Range(0, 100000) % tunnelPlatforms.Length;

            // creates a platform of the random type at the spawn point
            GameObject platform = Instantiate(tunnelPlatforms[tunnelPlatformIndex], spawnPoint.position + new Vector3(0, 0, spawnSeparation), spawnPoint.rotation);
            platform.transform.parent = transform;
            platforms.Enqueue(platform);
            // increments the count of the number of consecutive tunnel platforms that have been spawned
            m_numOfTunnelPlatforms++;
            // sets the spawn point for the next platform to the new platform's spawn point
            spawnPoint = platform.GetComponent<PlatformManager>().spawnPoint;

            // checks if the random number generated suggests that a pickup should be spawned
            bool spawnPickup = (Random.Range(0, 100) % pickupSpawnChance == 0) ? true : false;
            if (spawnPickup)
            {
                // makes the pickup position local to the platform
                Vector3 pickupPosition = platform.transform.position + pickupSpawnPoint;
                // creates a pickup
                GameObject pickup = Instantiate(healthPickup, pickupPosition, Quaternion.identity);
                // binds the pickup to the platform
                pickup.transform.parent = platform.transform;
            }
        }
    }

    /// <summary>
    /// Generates platforms when appropriate.
    /// </summary>
    private void Update()
    {
        if (platforms.Count > 0)
        {
            // checks if the first object has been destroyed and removes it from the collection
            if (platforms.ToArray()[0] == null)
            {
                platforms.Dequeue();
            }
            // checks if the distance between player and the spawn point is less than or equal to the distance threshold
            if (Vector3.Distance(m_player.transform.position, spawnPoint.position) <= spawnDistThreshold)
            {
                GeneratePlatform();
            }
        }
        else
        {
            GeneratePlatform();
        }

        if (moveSpeed < maxMovementSpeed)
        {
            // increases the platform movement speed each frame.
            moveSpeed += difficultyScalar * Time.deltaTime;
        }
        if (m_playerAnim.speed < maxAnimationSpeed)
        {
            // increases the animation speed each frame.
            m_playerAnim.speed += animationScalar * Time.deltaTime;
        }
    }
}