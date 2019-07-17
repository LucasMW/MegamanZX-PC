
using UnityEngine;



[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    private AudioSource source;
    [Range(0,1)]
    public float volume = 0.7f;
    [Range(0, 1.5f)]
    public float pitch = 1f;
    public bool loop = false;
   public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }
    public void Play()
    {
        source.loop = loop;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        /*Debug.Log(source.clip.name + " " + source.clip.length + " ");*/
    }
    public void Reset()
    {
        source.time = 0;
    }
    public bool isPlaying()
    {
        if (source.isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    // Start is called before the first frame update
    [SerializeField]
    Sound[] sounds;
    void Awake()
    {
        if(_instance != null)
        {
            Debug.Log("Error: More than 1 SoundManager in scene");
        }else
        _instance = this;
    }
    void Start()
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);

            sounds[i].SetSource(_go.AddComponent<AudioSource>());
            _go.transform.parent = this.transform;
        }
        PlaySound("BGM Forest");
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
        //Not found
        Debug.LogWarning("Audio Manager: Sound not found in list. " + _name);
    }
    public void ResetSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Reset();
            }
        }
    }
    public bool SoundEnded(string _name)//Returns true when sound ended
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                if (sounds[i].isPlaying())
                    return false;
                else return true;
            }
        }
        return true;
    }
}
