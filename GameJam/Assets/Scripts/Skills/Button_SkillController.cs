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
    }

    private void OnDisable()
    {
        CGlobal_SkillManager.RemoveActionSpriteChange(m_nSkillOfficerID, SpriteChange);
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
    public void CooldownChange(float fCooldownTimeCount,float fCooldownTime)
    {
        if (fCooldownTime <= 0)
            return;

        m_hCooldownSlider.maxValue = fCooldownTime;
        m_hCooldownSlider.value = fCooldownTimeCount;

        if (fCooldownTimeCount > 0)
            m_hButton.interactable = false;
        else
            m_hButton.interactable = true;
    }

    #endregion

    #region Action

    /// <summary>
    /// 
    /// </summary>
    void SpriteChange(Sprite hSprite)
    {
        m_hImage.sprite = hSprite;
    }

    #endregion
}
