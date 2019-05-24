using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Image[] healthImages;
    public TextMeshProUGUI timer;
    public PlayerController m_playerController;
    [HideInInspector]
    public float m_secondsCount;

    void Update()
    {
        PlayerHealthControl();
        UpdateTimerUI();
    }

    void PlayerHealthControl()
    {
        if (m_playerController != null)
        {
            if (m_playerController.currentHealth <= 100.0f)
            {
                healthImages[0].gameObject.SetActive(true);
                healthImages[1].gameObject.SetActive(true);

                healthImages[2].gameObject.SetActive(false);
                healthImages[3].gameObject.SetActive(false);

                healthImages[4].gameObject.SetActive(false);
                healthImages[5].gameObject.SetActive(false);

                healthImages[6].gameObject.SetActive(false);
                healthImages[7].gameObject.SetActive(false);
            }
            if (m_playerController.currentHealth <= 50.0f)
            {
                healthImages[0].gameObject.SetActive(false);
                healthImages[1].gameObject.SetActive(false);
                healthImages[4].gameObject.SetActive(false);
                healthImages[5].gameObject.SetActive(false);
                healthImages[6].gameObject.SetActive(false);
                healthImages[7].gameObject.SetActive(false);

                healthImages[2].gameObject.SetActive(true);
                healthImages[3].gameObject.SetActive(true);
            }
            if (m_playerController.currentHealth <= 20.0f)
            {
                healthImages[2].gameObject.SetActive(false);
                healthImages[3].gameObject.SetActive(false);
                healthImages[0].gameObject.SetActive(false);
                healthImages[1].gameObject.SetActive(false);
                healthImages[6].gameObject.SetActive(false);
                healthImages[7].gameObject.SetActive(false);

                healthImages[4].gameObject.SetActive(true);
                healthImages[5].gameObject.SetActive(true);
            }
            if (m_playerController.currentHealth <= 0.0f)
            {
                healthImages[2].gameObject.SetActive(false);
                healthImages[3].gameObject.SetActive(false);
                healthImages[0].gameObject.SetActive(false);
                healthImages[1].gameObject.SetActive(false);
                healthImages[4].gameObject.SetActive(false);
                healthImages[5].gameObject.SetActive(false);


                healthImages[6].gameObject.SetActive(true);
                healthImages[7].gameObject.SetActive(true);
            }

        }
    }

    //call this on update
    public void UpdateTimerUI()
    {
        //set timer UI
        m_secondsCount += Time.deltaTime;
        timer.text = Mathf.FloorToInt(m_secondsCount).ToString();
    }
}
