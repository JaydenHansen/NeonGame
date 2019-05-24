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
    /// Collection of all spawned chunks.
    /// </summary>
    [HideInInspector]
    public Queue<GameObject> platforms = new Queue<GameObject>();

    /// <summary>
    /// Reference to the player object.
    /// </summary>
    private GameObject m_player;
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
    /// Gets the player object.
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

        // increases the player animation speed and platform movement speed each frame.
        moveSpeed += difficultyScalar * Time.deltaTime;
        m_playerAnim.speed += (difficultyScalar * Time.deltaTime) / 10.0f;
    }
}