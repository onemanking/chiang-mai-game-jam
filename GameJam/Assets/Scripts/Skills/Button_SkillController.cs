using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button_SkillController : MonoBehaviour,IPointerDownHandler
{
    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

    [SerializeField] int m_nSkillOfficerID;
    //[SerializeField] Slider m_hCooldownSlider;

    [SerializeField] bool m_bOverrideImageBySkillData;
    [SerializeField] bool m_bShowNativeSizeImage;

    [SerializeField] bool m_bDisableClickWhenCooldown = true;

    [SerializeField] private Image m_hImage;
    [SerializeField] private Image m_hBackGround;

#pragma warning restore 0649
    #endregion

    public int SkillOfficerID { get { return m_nSkillOfficerID; } }

    Button m_hButton;
    bool m_bCooldown;

    #endregion

    #region Base - Mono

    private void Awake()
    {
        m_hButton = GetComponent<Button>();

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
    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_bCooldown && m_bDisableClickWhenCooldown)
            return;

        CGlobal_SkillManager.UseSkill(m_nSkillOfficerID);
        m_hImage.color = new Color32(255, 255, 255, 125);
    }

    /// <summary>
    /// 
    /// </summary>
    void CooldownChange(float fValue)
    {
        //Debug.Log("cooldown " + fValue);
        m_hBackGround.fillAmount = fValue;
        if(fValue >= 1f){
            m_hImage.color = new Color32(255,255,255,255);
        }

        if (fValue < 1)
        {
            m_bCooldown = true;
            m_hImage.raycastTarget = false;
            m_hBackGround.raycastTarget = false;
        }
        else
        {
            m_bCooldown = false;
            m_hImage.raycastTarget = true;
            m_hBackGround.raycastTarget = true;
        }
    }

    #endregion

    #region Action

    /// <summary>
    /// 
    /// </summary>
    void SpriteChange(Sprite hSprite)
    {
        if (!m_bOverrideImageBySkillData)
            return;

        m_hImage.sprite = hSprite;

        if (m_bShowNativeSizeImage)
            m_hImage.SetNativeSize();
    }



    #endregion
}
