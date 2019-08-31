using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayableAsset_OneAnimationClip : PlayableAsset
{
    AnimationClip m_hClip;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var hPlayable = ScriptPlayable<PlayableBehaviour_OneAnimationClip>.Create(graph);

        var hBav = hPlayable.GetBehaviour();
        hBav.Init(go,m_hClip, graph,hPlayable);

        return hPlayable;
    }

    public void SetClip(AnimationClip hClip)
    {
        m_hClip = hClip;
    }

    
}
