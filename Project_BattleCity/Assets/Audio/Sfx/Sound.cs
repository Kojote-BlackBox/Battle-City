using UnityEngine;

public enum SoundType {
    Music,
    SFX,
    Voice
}

[System.Serializable]
public class Sound {

    public string name;
    public AudioClip clip;
    public SoundType type;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
