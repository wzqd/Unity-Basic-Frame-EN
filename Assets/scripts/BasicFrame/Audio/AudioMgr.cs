using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


#region Audio Manager
//Audio manger is used for managing background music and other sound effects
//The principle is to put all AudioSource onto one created gameObject, and dynamically add and delete them
//Thus, this is not used for large 3D projects where sounds are depended on distance

//The main methods includes playing background music and sound effects and adjusting them
#endregion

public class AudioMgr : Singleton<AudioMgr>
{
    private AudioSource BGM = null;
    private float BGMVolume = 1f;

    private GameObject soundCarrier = null;
    private float audioVolume = 1f;
    private List<AudioSource> audioList = new List<AudioSource>();

    /// <summary>
    /// Constructor Add update listener
    /// </summary>
    public AudioMgr()
    {
        MonoMgr.Instance.AddUpdateListener(Update);
    }

    /// <summary>
    /// Detect every frame if there are any completed audio and clear them
    /// </summary>
    private void Update()
    {
        for (int i = audioList.Count-1; i >= 0; --i)
        {
            if (!audioList[i].isPlaying)
            {
                Object.Destroy(audioList[i]);
                audioList.RemoveAt(i);
            }
        }
    }
    
    /// <summary>
    /// Play BGM
    /// </summary>
    /// <param name="name">name of BGM</param>
    public void PlayBGM(string name)
    {
        if (BGM == null)
        {
            GameObject obj = new GameObject("BGMCarrier");
            GameObject.DontDestroyOnLoad(obj);
            BGM = obj.AddComponent<AudioSource>();
        }
        
        ResMgr.Instance.LoadAsync<AudioClip>("Music/BGM/" + name, (audioClip) =>
        {
            BGM.clip = audioClip;
            BGM.loop = true;
            BGM.volume = BGMVolume;
            BGM.Play();
        });
    }

    /// <summary>
    /// Change the volume of BGM
    /// </summary>
    /// <param name="volume">Volume</param>
    public void ChangeBGMVolume(float volume)
    {
        BGMVolume = volume;
        if (BGM == null) return;
        BGM.volume = BGMVolume;
    }
    
    /// <summary>
    /// Pause BGM
    /// </summary>
    public void PauseBGM()
    {
        if (BGM == null) return;
        BGM.Pause();
        
    }
    
    /// <summary>
    /// Stop BGM
    /// </summary>
    public void StopBGM()
    {
        if (BGM == null) return;
        BGM.Stop();
    }

    /// <summary>
    /// Play sound effects
    /// </summary>
    /// <param name="name">name of sound effect</param>
    /// <param name="isLoop">if it loops</param>
    /// <param name="afterPlay">call back function called after playing</param>
    /// <return>return AudioSource for stopping</return>
    public AudioSource PlayAudio(string name, bool isLoop, UnityAction<AudioSource> afterPlay = null)
    {
        if (soundCarrier == null)
        {
            soundCarrier = new GameObject ( "SoundCarrier");
        }

        AudioSource source = null;
        ResMgr.Instance.LoadAsync<AudioClip>("Music/Audio/" + name, (audioClip) =>
        {
            source = soundCarrier.AddComponent<AudioSource>();
            source.clip = audioClip;
            source.loop = isLoop;
            source.volume = audioVolume;
            source.Play();
            audioList.Add(source);

            if(afterPlay != null)
                afterPlay(source);
            
        });
        return source;
    }

    /// <summary>
    /// Change volume of all sound effects
    /// </summary>
    /// <param name="volume">volume</param>
    public void ChangeAudioVolume(float volume)
    {
        audioVolume = volume;
        foreach (AudioSource source in audioList)
        {
            source.volume = volume;
        }
    }

    /// <summary>
    /// Stop particular one sound effect
    /// </summary>
    /// <param name="source">AudioSource</param>
    public void StopAudio(AudioSource source)
    {
        if (audioList.Contains(source))
        {
            audioList.Remove(source);
            source.Stop();
            Object.Destroy(source);
        }
        
    }

    /// <summary>
    /// Clear all audio in the list Used when changing scenes
    /// </summary>
    public void ClearAudio()
    {
        audioList.Clear();
    }

}
