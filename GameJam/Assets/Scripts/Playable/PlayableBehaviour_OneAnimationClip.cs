using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

// A behaviour that is attached to a playable
public class PlayableBehaviour_OneAnimationClip : PlayableBehaviour
{
    public void Init(GameObject hGO,AnimationClip hClip,PlayableGraph hGraph, Playable hPlayable)
    {
        if (hGO == null || hClip == null)
            return;

        var hAnim = hGO.GetComponent<Animator>();

        if (hAnim == null)
            return;

        var hOutput = AnimationPlayableOutput.Create(hGraph, "Animation", hAnim);

        hOutput.SetSourcePlayable(hPlayable);
        hOutput.SetSourceOutputPort(0);

        hPlayable.SetInputCount(1);

       var m_hMixer = AnimationMixerPlayable.Create(hGraph, 2);

        hGraph.Connect(m_hMixer, 0, hPlayable, 0);

        hPlayable.SetInputWeight(0, 1);

        var hAnimClip = AnimationClipPlayable.Create(hGraph, hClip);

        hGraph.Connect(hAnimClip, 0, m_hMixer, 0);

        m_hMixer.SetInputWeight(0, 1.0f);

        // Connect to animator.
        var hControlAnimator = AnimatorControllerPlayable.Create(hGraph, hAnim.runtimeAnimatorController);
        hGraph.Connect(hControlAnimator, 0, m_hMixer, m_hMixer.GetInputCount() - 1);

        // play same as animator.
        var hCurrentStateInfo = hAnim.GetCurrentAnimatorStateInfo(0);

        var hController = (AnimatorControllerPlayable)m_hMixer.GetInput(m_hMixer.GetInputCount() - 1);
        hController.Play(hCurrentStateInfo.shortNameHash, 0, hCurrentStateInfo.normalizedTime);
    }

}
