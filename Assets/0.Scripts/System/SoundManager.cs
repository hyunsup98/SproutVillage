using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundType
{
    BGM,
    SoundEffect
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource soundEffectAudioSource;

    public event Action<SoundType, float> onChangedVolume;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmAudioSource == null || clip == null) return;

        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (soundEffectAudioSource == null || clip == null) return;

        soundEffectAudioSource.PlayOneShot(clip);
        transform.position = Camera.main.transform.position;
    }

    public void SetSoundVolume(SoundType type, float volume)
    {
        switch (type)
        {
            case SoundType.BGM:
                bgmAudioSource.volume = volume;
                onChangedVolume?.Invoke(SoundType.BGM, bgmAudioSource.volume);
                break; ;
            case SoundType.SoundEffect:
                soundEffectAudioSource.volume = volume;
                onChangedVolume?.Invoke(SoundType.SoundEffect, soundEffectAudioSource.volume);
                break;
        }
    }

    //씬이 로드될 때 실행할 메서드
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAudioSource(bgmAudioSource);
        StopAudioSource(soundEffectAudioSource);
    }

    private void StopAudioSource(AudioSource source)
    {
        if (source != null && source.isPlaying)
        {
            source.Stop();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
