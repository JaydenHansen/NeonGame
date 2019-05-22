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
    /// Used to specify the random generation.
    /// </summary>
    [Tooltip("Used to specify the random generation.")]
    public int seed = 0;
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
    /// The number of straight chunks that have been spawned.
    /// </summary>
    private int m_numOfStraightPlatforms = -1;
    /// <summary>
    /// The number of curved chunks that have been spawned.
    /// </summary>
    private int m_numOfCurvedPlatforms = -1;
    /// <summary>
    /// The type of platform that is being spawned.
    /// </summary>
    private PlatformManager.PlatformType m_platformType;

    /// <summary>
    /// Creates a platform at the spawn point.
    /// </summary>
    private void GeneratePlatform()
    {
        // checks if any platforms have been generated yet or it is time to potentially start spawning a different type of platform
        if ((m_numOfCurvedPlatforms == -1 && m_numOfStraightPlatforms == -1) || m_numOfStraightPlatforms == numOfStraightPlatformsBeforeRegen || m_numOfCurvedPlatforms == numOfCurvedPlatformsBeforeRegen)
        {
            // initialises the pseudo random number generator
            System.Random prng = new System.Random(seed);
            // chooses a random odd or even number to determine the platform type
            m_platformType = (prng.Next(-100000, 100000) % 2 == 0) ? PlatformManager.PlatformType.Straight : PlatformManager.PlatformType.Curved;
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
            // initialises the pseudo random number generator
            System.Random prng = new System.Random(seed);
            // chooses a random type of straight platform
            int straightPlatformIndex = prng.Next(-100000, 100000) % straightPlatforms.Length;
            // creates a platform of the random type at the spawn point
            platforms.Enqueue(Instantiate(straightPlatforms[straightPlatformIndex], spawnPoint.position, spawnPoint.rotation));
            // increments the count of the number of consecutive straight platforms that have been spawned
            m_numOfStraightPlatforms++;
        }
        // checks if a straight platform should be created
        else if (m_platformType == PlatformManager.PlatformType.Curved && m_numOfCurvedPlatforms < numOfCurvedPlatformsBeforeRegen)
        {
            // initialises the pseudo random number generator
            System.Random prng = new System.Random(seed);
            // chooses a random type of curved platform
            int curvedPlatformIndex = prng.Next(-100000, 100000) % straightPlatforms.Length;
            // creates a platform of the random type at the spawn point
            platforms.Enqueue(Instantiate(curvedPlatforms[curvedPlatformIndex], spawnPoint.position, spawnPoint.rotation));
            // increments the count of the number of consecutive curved platforms that have been spawned
            m_numOfCurvedPlatforms++;
        }
    }

    /// <summary>
    /// Generates platforms when appropriate.
    /// </summary>
    private void Update()
    {
        // checks if there aren't any platforms or if the distance between the last platform and the spawn point is greater than or equal to the distance threshold
        if (platforms.Count == 0 || Vector3.Distance(platforms.ToArray()[platforms.Count - 1].transform.position, spawnPoint.position) >= spawnDistThreshold)
        {
            // generates a platform
            GeneratePlatform();
        }
        // checks if the first object has been destroyed and removes it from the collection
        if (platforms.ToArray()[0] == null)
        {
            platforms.Dequeue();
        }
    }
}