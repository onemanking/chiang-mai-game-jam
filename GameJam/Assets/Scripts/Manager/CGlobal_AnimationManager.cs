using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public sealed class CGlobal_AnimationManager : MonoBehaviour
{
    #region Struct

    struct GraphData
    {
        public PlayableGraph m_hGraph;
        public float m_fPlayDuration;
    }

    #endregion

    #region Variable

    #region Variable - Property

    static CGlobal_AnimationManager Instance
    {
        get
        {
            if (m_hInstance == null)
                SpawnThisManager();

            return m_hInstance;
        }
    }

    #endregion

    static CGlobal_AnimationManager m_hInstance;

    PlayableAsset_OneAnimationClip m_hAsset;

    Dictionary<Animator, GraphData> m_dicGraphData = new Dictionary<Animator, GraphData>();
    Dictionary<Animator, GraphData> m_dicTempGraph = new Dictionary<Animator, GraphData>();
    List<Animator> m_lstClearGraph = new List<Animator>();

    #endregion

    #region Base - Mono

    private void Awake()
    {
        if(m_hInstance == null)
        {
            m_hInstance = this;
        }
        else if(m_hInstance != this)
        {
            Destroy(this);
            return;
        }

        m_hAsset = ScriptableObject.CreateInstance<PlayableAsset_OneAnimationClip>();
    }

    private void Update()
    {
        if (m_dicGraphData.Count <= 0)
            return;

        m_dicTempGraph.Clear();
        m_lstClearGraph.Clear();

        foreach(var hGraphData in m_dicGraphData)
        {
            var hData = hGraphData.Value;

            if (!hData.m_hGraph.IsValid())
            {
                m_lstClearGraph.Add(hGraphData.Key);
                continue;
            }

            hData.m_fPlayDuration -= Time.deltaTime;

            
            if (hData.m_fPlayDuration <= 0)
            {
                m_lstClearGraph.Add(hGraphData.Key);
                continue;
            }
            else
            {
                m_dicTempGraph.Add(hGraphData.Key, hData);

            }
        }

        for(int i = 0; i < m_lstClearGraph.Count; i++)
        {
            if (m_dicGraphData[m_lstClearGraph[i]].m_hGraph.IsValid()) 
                m_dicGraphData[m_lstClearGraph[i]].m_hGraph.Destroy();

            m_dicGraphData.Remove(m_lstClearGraph[i]);
        }

        foreach(var hGraphData in m_dicTempGraph)
        {
            m_dicGraphData[hGraphData.Key] = hGraphData.Value;
        }
    }

    #endregion

    #region Main

    /// <summary>
    /// 
    /// </summary>
    public static void PlayThisAnimationClip(Animator hAnim,AnimationClip hClip)
    {
        Instance?.MainPlayThisAnimationClip(hAnim, hClip);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void PlayThisAnimationClip(Animator hAnim, AnimationClip hClip,float fPlayDuration)
    {
        Instance?.MainPlayThisAnimationClip(hAnim, hClip,fPlayDuration);
    }


    /// <summary>
    /// 
    /// </summary>
    void MainPlayThisAnimationClip(Animator hAnim, AnimationClip hClip,float fPlayDuration = -1)
    {
        if (hAnim == null || hClip == null)
            return;

        if (m_dicGraphData.ContainsKey(hAnim))
        {
            if (m_dicGraphData[hAnim].m_hGraph.IsValid())
            {
                m_dicGraphData[hAnim].m_hGraph.Destroy();
            }

            m_dicGraphData.Remove(hAnim);
        }

        var hGraph = PlayableGraph.Create("Play Animation");
        hGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        m_hAsset.SetClip(hClip);
        var hPlayable = m_hAsset.CreatePlayable(hGraph, hAnim.gameObject);

        hGraph.Play();

        if (fPlayDuration <= 0)
            fPlayDuration = hClip.length;

        m_dicGraphData.Add(hAnim, new GraphData
        {
            m_hGraph = hGraph,
            m_fPlayDuration = fPlayDuration
        });
    }

    /// <summary>
    /// 
    /// </summary>
    public static void StopThisPlayable(Animator hAnim)
    {
        Instance?.MainStopThisPlayable(hAnim);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainStopThisPlayable(Animator hAnim)
    {
        if (m_dicGraphData.ContainsKey(hAnim))
        {
            if(m_dicGraphData[hAnim].m_hGraph.IsValid())
                m_dicGraphData[hAnim].m_hGraph.Destroy();

            m_dicGraphData.Remove(hAnim);
        }
    }


    #endregion

    #region Helper

    /// <summary>
    /// 
    /// </summary>
    static void SpawnThisManager()
    {
        var hGO = new GameObject();
        hGO.AddComponent<CGlobal_AnimationManager>();
        hGO.name = "Animation Manager";
    }

    #endregion
}
