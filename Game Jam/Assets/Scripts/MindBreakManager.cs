using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Manages all mind breaking effects from the tunnel chunks.
/// </summary>
public class MindBreakManager : MonoBehaviour
{
    /// <summary>
    /// The amount of time colours are inverted for.
    /// </summary>
    [Tooltip("The amount of time colours are inverted for.")]
    public float invertDuration = 5.0f;
    /// <summary>
    /// The amount of time is in first person for.
    /// </summary>
    [Tooltip("The amount of time is in first person for.")]
    public float firstPersonDuration = 5.0f;
    /// <summary>
    /// The amount of time the camera is distorted for.
    /// </summary>
    [Tooltip("The amount of time the camera is distorted for.")]
    public float distortDuration = 5.0f;
    /// <summary>
    /// The amount the lens is distorted by.
    /// </summary>
    [Tooltip("The amount the lens is distorted by.")]
    public float distortionAmount = 50.0f;
    public float distortStopThreshold = 0.5f;
    /// <summary>
    /// The amount of time camera spins for.
    /// </summary>
    [Tooltip("The amount of time camera spins for.")]
    public float spinDuration = 5.0f;
    /// <summary>
    /// The speed at which the camera spins.
    /// </summary>
    [Tooltip("The speed at which the camera spins.")]
    public float spinSpeed = 2.0f;
    /// <summary>
    /// The angle ± from the original camera rotation.
    /// </summary>
    [Tooltip("The angle ± from the original camera rotation.")]
    public float spinStopThreshold = 2.0f;
    /// <summary>
    /// The material that will be inverted.
    /// </summary>
    public Material invertMat;
    /// <summary>
    /// Reference to the first person camera.
    /// </summary>
    [Tooltip("Reference to the first person camera.")]
    public Camera firstPersonCam;
    /// <summary>
    /// Reference to the main camera.
    /// </summary>
    [Tooltip("Reference to the main camera.")]
    public Camera mainCam;

    /// <summary>
    /// Used to time how long the colours have been inverted.
    /// </summary>
    private float m_invertTimer = 0.0f;
    /// <summary>
    /// Used to time how long the camera has been first person.
    /// </summary>
    private float m_firstPersonTimer = 0.0f;
    /// <summary>
    /// Used to time how long the camera has been distorted.
    /// </summary>
    private float m_distortTimer = 0.0f;
    /// <summary>
    /// Used to time how long the camera has been spinning.
    /// </summary>
    private float m_spinTimer = 0.0f;
    /// <summary>
    /// Determines if the colours are inverted.
    /// </summary>
    private bool m_isInverted = false;
    /// <summary>
    /// Determines if the camera is first person.
    /// </summary>
    private bool m_isFirstPerson = false;
    /// <summary>
    /// Determines if the camera is distorted.
    /// </summary>
    private bool m_isDistorted = false;
    /// <summary>
    /// Determines if the camera is spinning.
    /// </summary>
    private bool m_isSpinning = false;

    /// <summary>
    /// Times the mind breaks.
    /// </summary>
    private void Update()
    {
        // checks if the colours are inverted
        if (m_isInverted)
        {
            // increments the timer
            m_invertTimer += Time.deltaTime;
            // checks if the timer ran out
            if (m_invertTimer >= invertDuration)
            {
                // sets the colour back to normal
                invertMat.SetColor("TintColor", Color.black);
                m_isInverted = false;
                m_invertTimer = 0.0f;
            }
        }

        // checks if the camera is first person
        if (m_isFirstPerson)
        {
            // increments the timer
            m_firstPersonTimer += Time.deltaTime;
            // checks if the timer ran out
            if (m_firstPersonTimer >= firstPersonDuration)
            {
                // swaps the activity of the cameras
                mainCam.gameObject.SetActive(true);
                firstPersonCam.gameObject.SetActive(false);
                m_isFirstPerson = false;
                m_firstPersonTimer = 0.0f;
            }
        }

        if (m_isDistorted)
        {
            m_distortTimer += Time.deltaTime;
            mainCam.GetComponent<PostProcessVolume>().profile.TryGetSettings(out UnityEngine.Rendering.PostProcessing.LensDistortion lensDistortion);
            lensDistortion.intensity.value = Mathf.Sin(m_distortTimer) * distortionAmount;
            firstPersonCam.GetComponent<PostProcessVolume>().profile.TryGetSettings(out lensDistortion);
            lensDistortion.intensity.value = Mathf.Sin(m_distortTimer) * distortionAmount;
            if (m_distortTimer >= distortDuration)
            {
                m_isDistorted = false;
            }
        }
        else
        {
            if (m_distortTimer != 0.0f)
            {
                mainCam.GetComponent<PostProcessVolume>().profile.TryGetSettings(out UnityEngine.Rendering.PostProcessing.LensDistortion lensDistortion);
                if (lensDistortion.intensity.value > 0.0f + distortStopThreshold || lensDistortion.intensity.value < 0.0f - distortStopThreshold)
                {
                    m_distortTimer += Time.deltaTime;
                    lensDistortion.intensity.value = Mathf.Sin(m_distortTimer) * distortionAmount;
                    firstPersonCam.GetComponent<PostProcessVolume>().profile.TryGetSettings(out lensDistortion);
                    lensDistortion.intensity.value = Mathf.Sin(m_distortTimer) * distortionAmount;
                }
                else
                {
                    m_distortTimer = 0.0f;
                }
            }
        }

        // checks if the camera is spinning
        if (m_isSpinning)
        {
            // rotates the camera around its local z axis
            mainCam.transform.Rotate(new Vector3(0.0f, 0.0f, Time.deltaTime * spinSpeed));
            firstPersonCam.transform.Rotate(new Vector3(0.0f, 0.0f, Time.deltaTime * spinSpeed));
            // increments the timer
            m_spinTimer += Time.deltaTime;
            // checks if the timer ran out
            if (m_spinTimer >= spinDuration)
            {
                // stops the camera from spinning
                m_isSpinning = false;
                m_spinTimer = 0.0f;
            }
        }
        // checks if the camera should not be spinning
        else
        {
            // checks if the camera is not close to the original rotation
            if (mainCam.transform.rotation.eulerAngles.z > 0.0f + spinStopThreshold || mainCam.transform.rotation.eulerAngles.z < 0.0f - spinStopThreshold)
            {
                // rotates the cameras
                mainCam.transform.Rotate(new Vector3(0.0f, 0.0f, Time.deltaTime * spinSpeed));
                firstPersonCam.transform.Rotate(new Vector3(0.0f, 0.0f, Time.deltaTime * spinSpeed));
            }
        }
    }

    /// <summary>
    /// Inverts the colours.
    /// </summary>
    public void InvertColours()
    {
        invertMat.SetColor("TintColor", Color.white);
        m_isInverted = true;
        m_invertTimer = 0.0f;
    }

    /// <summary>
    /// Swaps the activity of the cameras.
    /// </summary>
    public void FirstPerson()
    {
        mainCam.gameObject.SetActive(false);
        firstPersonCam.gameObject.SetActive(true);
        m_isFirstPerson = true;
        m_firstPersonTimer = 0.0f;
    }

    /// <summary>
    /// Allows the lens to distort.
    /// </summary>
    public void LensDistortion()
    {
        if (!m_isDistorted)
        {
            m_isDistorted = true;
            m_distortTimer = 0.0f;
        }
    }

    /// <summary>
    /// Allows the camera to spin.
    /// </summary>
    public void SpinCamera()
    {
        m_isSpinning = true;
        m_spinTimer = 0.0f;
    }

    /// <summary>
    /// Ensures that the changed colour is not saved when the application is closed.
    /// </summary>
    private void OnApplicationQuit()
    {
        invertMat.SetColor("TintColor", Color.black);
    }
}