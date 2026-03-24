using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField]
    public GameObject SFXPrefab;
    private AudioSource musicSource;
    private AudioSource ambientSource;
    private float musicVolume;
    private float sfxVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        musicSource = gameObject.AddComponent<AudioSource>();
        ambientSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip _music)
    { 
        musicSource.clip = _music;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayAmbientSound(AudioClip _ambient)
    { 
        ambientSource.clip = _ambient;
        ambientSource.volume = sfxVolume;
        ambientSource.Play();
    }

    public void StopAmbientSound()
    { 
        ambientSource.Stop();
    }

    public void PlaySFX(AudioClip _sfx, Vector3 _position)
    {
        GameObject SFXClone = Instantiate(SFXPrefab, _position, Quaternion.identity);
        SFXClone.GetComponent<AudioSource>().clip = _sfx;
        SFXClone.GetComponent<AudioSource>().volume = sfxVolume;
        SFXClone.GetComponent<AudioSource>().Play();
        Destroy(SFXClone, _sfx.length);
    }

    public void SetMusicVolume(float _volume)
    {
        musicVolume = _volume;
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float _volume)
    {
        sfxVolume = _volume;
        ambientSource.volume = sfxVolume;
    }
}
