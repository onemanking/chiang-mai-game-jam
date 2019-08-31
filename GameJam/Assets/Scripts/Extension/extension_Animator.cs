using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class extension_Animator
{
    /// <summary>
    /// 
    /// </summary>
    public static void Play(this Animator hAnim,AnimationClip hClip)
    {
        CGlobal_AnimationManager.PlayThisAnimationClip(hAnim, hClip);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void Play(this Animator hAnim, AnimationClip hClip,float fPlayDuration)
    {
        CGlobal_AnimationManager.PlayThisAnimationClip(hAnim, hClip,fPlayDuration);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void StopPlayable(this Animator hAnim)
    {
        CGlobal_AnimationManager.StopThisPlayable(hAnim);
    }
}
