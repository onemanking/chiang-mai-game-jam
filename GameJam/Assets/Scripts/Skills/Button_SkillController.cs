using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_SkillController : MonoBehaviour
{
    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

    [SerializeField] int m_nSkillOfficerID;
    [SerializeField] Slider m_hCooldownSlider;

    [SerializeField] bool m_bShowNativeSideImage;

#pragma warning restore 0649
    #endregion

    public int SkillOfficerID { get { return m_nSkillOfficerID; } }

    Button m_hButton;
    Image m_hImage;

    #endregion

    #region Base - Mono

    private void Awake()
    {
        m_hButton = GetComponent<Button>();
        m_hImage = GetComponent<Image>();

        CGlobal_SkillManager.RegisterButtonSkill(this);
    }

    private void OnEnable()
    {
        CGlobal_SkillManager.AddActionSpriteChange(m_nSkillOfficerID, SpriteChange);

        CGlobal_SkillManager.AddActionCooldownChange(m_nSkillOfficerID, CooldownChange);
    }

    private void OnDisable()
    {
        CGlobal_SkillManager.RemoveActionSpriteChange(m_nSkillOfficerID, SpriteChange);

        CGlobal_SkillManager.RemoveActionCooldownChange(m_nSkillOfficerID, CooldownChange);
    }

    #endregion

    #region Main

    /// <summary>
    /// 
    /// </summary>
    public void ClickSkill()
    {
        CGlobal_SkillManager.UseSkill(m_nSkillOfficerID);
    }

    /// <summary>
    /// 
    /// </summary>
    void CooldownChange(float fValue)
    {
        if (m_hCooldownSlider)
        {
            m_hCooldownSlider.maxValue = 1;
            m_hCooldownSlider.value = 1 - fValue;
        }
        else
        {
            m_hImage.fillAmount = fValue;
        }

        if (fValue < 1)
            m_hImage.raycastTarget = false;
        else
            m_hImage.raycastTarget = true;
    }

    #endregion

    #region Action

    /// <summary>
    /// 
    /// </summary>
    void SpriteChange(Sprite hSprite)
    {
        m_hImage.sprite = hSprite;

        if (m_bShowNativeSideImage)
            m_hImage.SetNativeSize();
    }

    #endregion
}
