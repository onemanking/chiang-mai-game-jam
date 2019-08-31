using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillCutsceneController : MonoBehaviour
{
    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

    [SerializeField] float m_fShowDuration;

#pragma warning restore 0649
    #endregion

    Image m_hImage;

    float m_fShowDurationCount;

    #endregion

    #region Base - Mono

    private void Awake()
    {
        if(m_hImage == null)
            m_hImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (m_fShowDurationCount <= 0)
            return;

        m_fShowDurationCount -= Time.deltaTime;
        if (m_fShowDurationCount <= 0)
            Hide();
    }

    #endregion

    public void Show(Sprite hSprite)
    {
        if (m_hImage == null)
            return;

        m_hImage.sprite = hSprite;

        Color hColor = m_hImage.color;
        hColor.a = 1;
        m_hImage.color = hColor;

        m_fShowDurationCount = m_fShowDuration;
    }

    public void Hide()
    {
        if (m_hImage == null)
            return;

        Color hColor = m_hImage.color;
        hColor.a = 0;
        m_hImage.color = hColor;
    }
}
