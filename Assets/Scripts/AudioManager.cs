using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip SwapSFX;
    [SerializeField] private AudioClip MatchSFX;


    public void PlaySFX(SFXID id)
    {
        switch (id)
        {
            case SFXID.Swap:
                sfxSource.PlayOneShot(SwapSFX);
                break;
            case SFXID.Match:
                sfxSource.PlayOneShot(MatchSFX);
                break;
            default:
                break;
        }
    }
}

public enum SFXID
{
    Swap,
    Match,
}
