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
    /// The rate at which the colours lerp.
    /// </summary>
    [Tooltip("Speed at which the colours invert.")]
    public float invertSpeed = 0.5f;
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
    /// <summary>
    /// The threshold ± from the original distortion. Used to help stop the distortion.
    /// </summary>
    [Tooltip("The threshold ± from the original distortion.")]
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
            // interpolates the colour towards the inverted colour
            invertMat.SetColor("TintColor", Color.Lerp(Color.black, Color.white, Mathf.Clamp01(m_invertTimer * invertSpeed)));

            // checks if the timer ran out
            if (m_invertTimer >= invertDuration)
            {
                m_isInverted = false;
                m_invertTimer = 0.0f;
            }
        }
        // checks if the colour should not be inverted
        else
        {
            // checks if the colour is not back to normal
            if (invertMat.GetColor("TintColor") != Color.black)
            {
                // interpolates towards the normal colour
                m_invertTimer += Time.deltaTime;
                invertMat.SetColor("TintColor", Color.Lerp(Color.white, Color.black, Mathf.Clamp01(m_invertTimer * invertSpeed)));
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

        // checks if the lens is distorted
        if (m_isDistorted)
        {
            // increments the timer
            m_distortTimer += Time.deltaTime;
            // gets the lens distortion setting from the post processing
            mainCam.GetComponent<PostProcessVolume>().profile.TryGetSettings(out UnityEngine.Rendering.PostProcessing.LensDistortion lensDistortion);
            // alternates the intensity between ± distortion amount over time
            lensDistortion.intensity.value = Mathf.Sin(m_distortTimer) * distortionAmount;
            // does the same as above for the first person camera
            firstPersonCam.GetComponent<PostProcessVolume>().profile.TryGetSettings(out lensDistortion);
            lensDistortion.intensity.value = Mathf.Sin(m_distortTimer) * distortionAmount;
            // checks if the timer ran out
            if (m_distortTimer >= distortDuration)
            {
                m_isDistorted = false;
            }
        }
        // checks if the lens should not be distorted
        else
        {
            // checks if the timer has not been reset yet which indicates that the lens distortion is not back to normal
            if (m_distortTimer != 0.0f)
            {
                // gets the lens distortion setting from the post processing
                mainCam.GetComponent<PostProcessVolume>().profile.TryGetSettings(out UnityEngine.Rendering.PostProcessing.LensDistortion lensDistortion);
                // checks if the intensity is not back to normal
                if (lensDistortion.intensity.value > 0.0f + distortStopThreshold || lensDistortion.intensity.value < 0.0f - distortStopThreshold)
                {
                    // alternates the distortion intensity between the distortion amount
                    m_distortTimer += Time.deltaTime;
                    lensDistortion.intensity.value = Mathf.Sin(m_distortTimer) * distortionAmount;
                    firstPersonCam.GetComponent<PostProcessVolume>().profile.TryGetSettings(out lensDistortion);
                    lensDistortion.intensity.value = Mathf.Sin(m_distortTimer) * distortionAmount;
                }
                // resets the timer once the distortion is back to normal
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
        if (!m_isInverted)
        {
            m_isInverted = true;
            m_invertTimer = 0.0f;
        }
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