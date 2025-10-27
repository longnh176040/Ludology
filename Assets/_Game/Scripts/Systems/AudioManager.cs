using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[Serializable]
public struct AudioData
{
    public AudioClip audioClip;
    [Range(0, 1)] public float volume;
}
public class AudioManager : Singleton<AudioManager>
{
    private AudioSource musicSource;
    private AudioSource soundSource;
    private Dictionary<string, int> musics;
    private Dictionary<string, int> sounds;
    public bool MuteMusic {
        get => musicSource.mute;
        set => musicSource.mute = value;
    }
    public bool MuteSound {
        get => soundSource.mute;
        set => soundSource.mute = value;
    }
    public bool IsMusicPlaying
    {
        get => musicSource.isPlaying;
    }

    [SerializeField] private float crossFadeTransition;
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup soundMixer;
    [SerializeField] private List<AudioData> musicTracks;
    [SerializeField] private List<AudioData> soundTracks;

    public void Awake()
    {
        //base.Awake();
        musics = new Dictionary<string, int>();
        sounds = new Dictionary<string, int>();
        AudioMapping(musicTracks, musics);
        AudioMapping(soundTracks, sounds);
        soundSource = CreateAudioSource(soundMixer);
        musicSource = CreateAudioSource(musicMixer);
        musicSource.loop = true;
    }

    private void Start()
    {
        Initialize();
    }

    private AudioSource CreateAudioSource(AudioMixerGroup audioMixerGroup)
    {
        GameObject gameObject = new GameObject(typeof(AudioSource).Name);
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.transform.SetParent(transform);
        audioSource.playOnAwake = false;
        return audioSource;
    }

    private IEnumerator CrossFade(AudioSource audioSource, float volume, Action task)
    {
        float beginVolume = audioSource.volume;
        for (float elapsed = 0; elapsed <= crossFadeTransition; elapsed += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(beginVolume, volume, elapsed / crossFadeTransition);
            yield return null;
        }
        audioSource.volume = volume;
        task?.Invoke();
    }
    private void AudioMapping(List<AudioData> audioDatas, Dictionary<string, int> map)
    {
        for (int index = 0; index < audioDatas.Count; index++)
        {
            if (!map.ContainsKey(audioDatas[index].audioClip.name))
            {
                map.Add(audioDatas[index].audioClip.name, index);
            }
        }
    }
    public void Initialize()
    {
        SetMusicVolume(DataManager.Instance.settingData.music);
        SetSFXVolume(DataManager.Instance.settingData.sound);
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.audioMixer.SetFloat("MusicVol", volume);
        DataManager.Instance.settingData.music = volume;
    }

    public void SetSFXVolume(float volume)
    {
        soundMixer.audioMixer.SetFloat("SFXVol", volume);
        DataManager.Instance.settingData.sound = volume;
    }

    public void PlayMusic(string musicName, bool loop = true)
    {
        if (musics.TryGetValue(musicName, out int index))
        {
            AudioData audioData = musicTracks[index];
            if (audioData.audioClip == null) return;
            if (!musicSource.isPlaying)
            {
                musicSource.clip = audioData.audioClip;
                musicSource.loop = loop;
                musicSource.volume = 0;
                musicSource.Play();
                StartCoroutine(CrossFade(musicSource, audioData.volume, null));
            }
            else StartCoroutine(CrossFade(musicSource, 0, () =>
            {
                musicSource.Stop();
                PlayMusic(musicName, loop);
            }));
        }
    }
    public void PlaySound(string soundName)
    {
        if (string.IsNullOrWhiteSpace(soundName)) return;
        if (sounds.TryGetValue(soundName, out int index))
        {
            AudioData audioData = soundTracks[index];
            if (audioData.audioClip == null) return;
            soundSource.PlayOneShot(audioData.audioClip, audioData.volume);
        }
    }
    public AudioManager StopMusic()
    {
        if (musicSource.isPlaying) StartCoroutine(CrossFade(musicSource, 0, musicSource.Stop));
        return this;
    }
}