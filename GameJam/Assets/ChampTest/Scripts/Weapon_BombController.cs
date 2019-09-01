using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_BombController : MonoBehaviour
{
    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

    [SerializeField] GameObject m_hBombParticle;

#pragma warning restore 0649
    #endregion

    Rigidbody m_hRigid;

    Transform m_hOwner;
    float m_fDamageMultiplier;
    Vector3 m_vThrowForce;
    
    #endregion

    #region Base - Mono

    private void OnCollisionEnter(Collision collision)
    {
        DoDamageToAllPrisoner();

        Instantiate(m_hBombParticle,transform.position,Quaternion.Euler(-90,0,0));



        Destroy(gameObject);
    }

    #endregion

    #region Main

    /// <summary>
    /// 
    /// </summary>
    public void Init(Transform hOwner,float fDamageMultiplier,Vector3 vThrowForce)
    {
        m_hOwner = hOwner;
        m_fDamageMultiplier = fDamageMultiplier;
        m_vThrowForce = vThrowForce;

        m_hRigid = GetComponent<Rigidbody>();
        m_hRigid.isKinematic = true;

        StartCoroutine(WaitStart());
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(0.6f);
        m_hRigid.isKinematic = false;
        m_hRigid.AddForce(m_vThrowForce);

    }

    /// <summary>
    /// 
    /// </summary>
    void DoDamageToAllPrisoner()
    {
        if (m_hOwner == null)
            return;

        var hOfficerBase = m_hOwner.GetComponent<OfficerBase>();

        if (hOfficerBase == null)
            return;

        var lstGO = CGlobal_CharacterManager.GetCharacterList(TagType.Prisoner);

        if (lstGO == null || lstGO.Count <= 0)
            return;

        float fDamage = hOfficerBase.GetDamage() * m_fDamageMultiplier;

        for (int i = 0; i < lstGO.Count; i++)
        {
            // For test only.
            lstGO[i].SendMessage("TakeDamage", fDamage, SendMessageOptions.DontRequireReceiver);
        }
    }

    #endregion
}
