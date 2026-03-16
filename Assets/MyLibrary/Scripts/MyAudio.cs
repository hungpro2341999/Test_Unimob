using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum TypeSound { None, Clone }
public enum Sound { Audio1, Audio11, Audio12, Audio13, Audio14, None }
public class DataAudio : Data<DataAudio>
{
    public bool ACTIVE_SOUND_GAME = true;
    public bool ACTIVE_MUSIC_GAME = true;
    public bool ACTIVE_VIBRATION = true;
    public void SetActiveSound(bool active)
    {
        ACTIVE_SOUND_GAME = active;
        Save();
    }
    public void SetActiveMusic(bool active)
    {
        ACTIVE_MUSIC_GAME = active;
        Save();
    }
    public void SetActiveVibration(bool active)
    {
        ACTIVE_VIBRATION = active;
        Save();
    }
    public void ChangeVibration()
    {
        ACTIVE_VIBRATION = !ACTIVE_VIBRATION;
        Save();
    }
}
public class MyAudio : MonoBehaviour
{
    protected static MyAudio instance = null;
    public AudioSource soundGame;
    public AudioSource musicBGGame;
    public Dictionary<string, AudioClip> ALL_AUDIO = new Dictionary<string, AudioClip>();
    public List<AudioSource> allAudioClone = new List<AudioSource>();
    public List<string> allDelaySameSound = new List<string>();
    float volumeMusic = 0.5f;
    float volumeSound = 1;
    public static MyAudio Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject("MyAudio").AddComponent<MyAudio>();
                instance = obj;
                DontDestroyOnLoad(obj);
                instance.Init();
            }
            return instance;
        }
    }
    private void Start()
    {

    }
    void PlaySound(Sound sound, TypeSound typeSound)
    {
        PlaySound(sound.ToString(), typeSound);
    }
    public AudioSource PlaySoundClone(string nameSound)
    {
        if (!GameData.GetData<DataAudio>().ACTIVE_SOUND_GAME)
        {
            return null;
        }
        GameObject obj = new GameObject(nameSound);
        obj.transform.parent = transform;
        var audio = obj.AddComponent<AudioSource>();
        audio.loop = true;
        audio.clip = ALL_AUDIO[nameSound];
        audio.Play();
        float totalSeconds = audio.clip.length;
        allAudioClone.Add(audio);
        return audio;
    }

    public void PlaySound(string nameSound, TypeSound typeSound)
    {
        if (GameData.GetData<DataAudio>().ACTIVE_SOUND_GAME)
        {
            switch (typeSound)
            {
                case TypeSound.None:
                    soundGame.clip = ALL_AUDIO[nameSound];
                    soundGame.Play();
                    break;
                case TypeSound.Clone:
                    if (allDelaySameSound.Contains(nameSound))
                    {
                        return;
                    }
                    GameObject obj = new GameObject(nameSound);
                    obj.transform.parent = transform;
                    var audio = obj.AddComponent<AudioSource>();
                    audio.clip = ALL_AUDIO[nameSound];
                    audio.loop = false;
                    audio.Play();
                    float totalSeconds = audio.clip.length;
                    allAudioClone.Add(audio);
                    MyThread.Instance.AddDelayAction(totalSeconds, () =>
                    {
                        allAudioClone.Remove(audio);
                        GameObject.Destroy(audio.gameObject);
                    });
                    allDelaySameSound.Add(nameSound);
                    MyThread.Instance.AddDelayAction(0.1f, () =>
                    {
                        allDelaySameSound.Remove(nameSound);
                    });
                    break;
            }
        }
    }
    public AudioSource AddSoundClone(Sound sound)
    {
        GameObject obj = new GameObject(sound.ToString());
        obj.transform.parent = transform;
        var audio = obj.AddComponent<AudioSource>();
        audio.clip = ALL_AUDIO[sound.ToString()];
        audio.loop = false;
        if (!GameData.GetData<DataAudio>().ACTIVE_SOUND_GAME)
        {
            audio.volume = 0;
        }
        audio.Play();
        float totalSeconds = audio.clip.length;
        allAudioClone.Add(audio);
        MyThread.Instance.AddDelayAction(totalSeconds, () =>
        {
            if (audio == null)
                return;
            allAudioClone.Remove(audio);
            GameObject.Destroy(audio.gameObject);
        });
        return audio;
    }
    public void RemoveSoundClone(AudioSource audio)
    {
        if (audio == null)
            return;
        allAudioClone.Remove(audio);
        GameObject.Destroy(audio.gameObject);
    }
    public void PlayMusic(Sound sound)
    {
        PlayMusic(sound.ToString());
    }
    public void PlayMusic(string nameSound)
    {
        Debug.Log("Play Music " + nameSound);
        musicBGGame.clip = ALL_AUDIO[nameSound];
        musicBGGame.Play();
    }
    void Init()
    {
        var allAudio = Resources.LoadAll<AudioClip>("Audio").ToList();
        foreach (var audio in allAudio)
        {
            ALL_AUDIO.Add(audio.name, audio);
        }
        var sound = new GameObject("Sound").AddComponent<AudioSource>();
        sound.playOnAwake = false;
        sound.transform.parent = transform;
        soundGame = sound;

        var music = new GameObject("Music").AddComponent<AudioSource>();
        music.playOnAwake = false;
        music.loop = true;
        music.transform.parent = transform;

        soundGame.volume = volumeSound;
        musicBGGame.volume = volumeMusic;
        System.Action actionUpdate = () =>
        {
            musicBGGame.Play();
            foreach (var soundCheck_1 in allAudioClone)
            {
                soundCheck_1.volume = volumeSound;
            }
        };
        GameAction.RegisterActionGlobal(TypeActionGlobal.UpdateSound, actionUpdate);
    }
    public void StartAudio()
    {
        MyThread.Instance.AddDelayFrame(1, () =>
        {
            PlayMusic(Sound.Audio1);
        });
    }
    public void ResumeMusic()
    {
        musicBGGame.Play();
    }
    public void ChangeMusic()
    {
        GameData.GetData<DataAudio>().SetActiveMusic(!GameData.GetData<DataAudio>().ACTIVE_MUSIC_GAME);
        GameAction.InvokeActionGlobal(TypeActionGlobal.UpdateSound);
    }
    public void ChangeSound()
    {
        GameData.GetData<DataAudio>().SetActiveSound(!GameData.GetData<DataAudio>().ACTIVE_SOUND_GAME);
        GameAction.InvokeActionGlobal(TypeActionGlobal.UpdateSound);
    }
    public void ChangeVibration()
    {
        GameData.GetData<DataAudio>().SetActiveVibration(!GameData.GetData<DataAudio>().ACTIVE_VIBRATION);
    }
    public void SetAudio(bool isStop)
    {
        if (isStop)
        {
            musicBGGame.volume = 0;
            soundGame.volume = 0;
            foreach (var sound in allAudioClone)
            {
                sound.volume = 0;
            }
        }
        else
        {
            musicBGGame.volume = volumeMusic;
            soundGame.volume = volumeSound;
            foreach (var sound in allAudioClone)
            {
                sound.volume = volumeSound;
            }
        }
    }
    void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0)
        {
            delay = 0;
        }
    }

    float delay = 0;
    float delayVirbration = 0.2f;
    public void Vibration()
    {
        if (delay <= 0)
        {
            if (GameData.GetData<DataAudio>().ACTIVE_VIBRATION)
            {
                // Handheld.Vibrate();    
                //HapticFeedback.MediumFeedback();                    
            }
            delay = delayVirbration;
        }

    }

}
