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
    /// The number of curved chunks consecutively generated before a random generation occurs again.
    /// </summary>
    [Tooltip("The number of curved chunks consecutively generated before a random generation occurs again.")]
    public int numOfCurvedPlatformsBeforeRegen = 5;
    /// <summary>
    /// Collection of different possible straight platforms.
    /// </summary>
    [Tooltip("Collection of different possible straight platforms.")]
    public GameObject[] straightPlatforms;
    /// <summary>
    /// Collection of different possible curved platforms.
    /// </summary>
    [Tooltip("Collection of different possible curved platforms.")]
    public GameObject[] curvedPlatforms;
    /// <summary>
    /// The location where the platform will get spawned at.
    /// </summary>
    [Tooltip("The location where the platform will get spawned at.")]
    public Transform spawnPoint;
    /// <summary>
    /// The distance between the last chunk and the spawn point.
    /// </summary>
    [Tooltip("The distance between the last chunk and the spawn point.")]
    public float spawnDistThreshold = 100.0f;

    /// <summary>
    /// Collection of all spawned chunks.
    /// </summary>
    [HideInInspector]
    public Queue<GameObject> platforms = new Queue<GameObject>();
    /// <summary>
    /// Reference to the player object.
    /// </summary>
    [HideInInspector]
    public GameObject player;

    /// <summary>
    /// The number of straight chunks that have been spawned.
    /// </summary>
    private int m_numOfStraightPlatforms = 0;
    /// <summary>
    /// The number of curved chunks that have been spawned.
    /// </summary>
    private int m_numOfCurvedPlatforms = 0;
    /// <summary>
    /// The type of platform that is being spawned.
    /// </summary>
    private PlatformManager.PlatformType m_platformType = PlatformManager.PlatformType.Straight;

    /// <summary>
    /// Gets the player object.
    /// </summary>
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Creates a platform at the spawn point.
    /// </summary>
    private void GeneratePlatform()
    {
        // checks if any platforms have been generated yet or it is time to potentially start spawning a different type of platform
        if (m_numOfStraightPlatforms == numOfStraightPlatformsBeforeRegen || m_numOfCurvedPlatforms == numOfCurvedPlatformsBeforeRegen)
        {
            // chooses a random odd or even number to determine the platform type
            m_platformType = (Random.Range(-100000, 100000) % (straightPlatforms.Length + curvedPlatforms.Length) != 0) ? PlatformManager.PlatformType.Straight : PlatformManager.PlatformType.Curved;
            Debug.Log(m_platformType);
            // checks if the platform type is straight
            if (m_platformType == PlatformManager.PlatformType.Straight)
            {
                m_numOfStraightPlatforms = 0;
            }
            else
            {
                m_numOfCurvedPlatforms = 0;
            }
        }
        // checks if a straight platform should be created
        if (m_platformType == PlatformManager.PlatformType.Straight && m_numOfStraightPlatforms < numOfStraightPlatformsBeforeRegen)
        {
            // chooses a random type of straight platform
            int straightPlatformIndex = Random.Range(-100000, 100000) % straightPlatforms.Length;
            GameObject platform = Instantiate(straightPlatforms[straightPlatformIndex], spawnPoint.position, spawnPoint.rotation);
            platform.transform.parent = transform;
            // creates a platform of the random type at the spawn point
            platforms.Enqueue(platform);
            // increments the count of the number of consecutive straight platforms that have been spawned
            m_numOfStraightPlatforms++;
            spawnPoint = platform.GetComponent<PlatformManager>().spawnPoint;
        }
        // checks if a straight platform should be created
        else if (m_platformType == PlatformManager.PlatformType.Curved && m_numOfCurvedPlatforms < numOfCurvedPlatformsBeforeRegen)
        {
            // chooses a random type of curved platform
            int curvedPlatformIndex = Random.Range(-100000, 100000) % curvedPlatforms.Length;
            GameObject platform = Instantiate(curvedPlatforms[curvedPlatformIndex], spawnPoint.position, spawnPoint.rotation);
            platform.transform.parent = transform;
            // creates a platform of the random type at the spawn point
            platforms.Enqueue(platform);
            // increments the count of the number of consecutive curved platforms that have been spawned
            m_numOfCurvedPlatforms++;
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
            if (Vector3.Distance(player.transform.position, spawnPoint.position) <= spawnDistThreshold)
            {
                GeneratePlatform();
            }
        }
        else
        {
            GeneratePlatform();
        }
    }

    /// <summary>
    /// Rotates all chunks.
    /// </summary>
    /// <param name="rotSpeed">Scalar for the rotation speed.</param>
    public void Rotate(float rotSpeed)
    {
        // rotates around a local up axis
        transform.Rotate(transform.up, -rotSpeed * Time.deltaTime);
    }
}