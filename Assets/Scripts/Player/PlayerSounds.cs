using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public enum SfxState { Trow, Dmg, Teleport, Death, Teleport2, Glass,Start, End, Fail, DmgPlayer }
    public SfxState currentState;

    // Referências às músicas para cada estado
    public AudioClip TrowSfx;
    public List<AudioClip> DmgSfx;
    public AudioClip TeleportSfx;
    public AudioClip DeathSfx;
    public AudioClip TeleportTwoSfx;
    public AudioClip StartSfx;
    public AudioClip AssSfx;
    public AudioClip dmgPlayer;
    public List<AudioClip> failSfx;
    public List<AudioClip> GlassSfx;
    private AudioSource audioSource;

    private static PlayerSounds instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void RandomDmgSound()
    {
        if (DmgSfx != null && DmgSfx.Count > 0)
        {
            int randomIndex = Random.Range(0, DmgSfx.Count);
            audioSource.clip = DmgSfx[randomIndex];
        }
    }

    public void RandomFailSound()
    {
        if (failSfx != null && failSfx.Count > 0)
        {
            int randomIndex = Random.Range(0, failSfx.Count);
            audioSource.clip = failSfx[randomIndex];
        }
    }


    public void PlaySfx(SfxState state)
    {
        if (currentState == state && audioSource.isPlaying)
            return; // Não troca a música se já estiver tocando a do estado atual

        currentState = state;
        switch (state)
        {
            case SfxState.Trow:
                audioSource.clip = TrowSfx;
                break;
            case SfxState.Dmg:
                RandomDmgSound();
                break;
            case SfxState.Teleport:
                RandomDmgSound();
                break;
            case SfxState.Death:
                audioSource.clip = DeathSfx;
                break;
            case SfxState.Teleport2:
                audioSource.clip = TeleportTwoSfx;
                break;
            case SfxState.Glass:
                if (GlassSfx != null && GlassSfx.Count > 0)
                {
                    int randomIndex = Random.Range(0, GlassSfx.Count);
                    audioSource.clip = GlassSfx[randomIndex];
                }
                break;
            case SfxState.Start:
                audioSource.clip = StartSfx;
                break;
            case SfxState.End:
                audioSource.clip = AssSfx;
                break;
            case SfxState.DmgPlayer:
                audioSource.clip = dmgPlayer;
                break;
            case SfxState.Fail:
                RandomFailSound();
                break;
}

        audioSource.Play();
    }
}
