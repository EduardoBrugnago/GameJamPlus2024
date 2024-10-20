using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public enum SfxState { Trow, Dmg, Teleport, Death, Teleport2, Glass }
    public SfxState currentState;

    // Referências às músicas para cada estado
    public AudioClip TrowSfx;
    public AudioClip DmgSfx;
    public AudioClip TeleportSfx;
    public AudioClip DeathSfx;
    public AudioClip TeleportTwoSfx;
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
                audioSource.clip = DmgSfx;
                break;
            case SfxState.Teleport:
                audioSource.clip = TeleportSfx;
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
        }

        audioSource.Play();
    }
}
