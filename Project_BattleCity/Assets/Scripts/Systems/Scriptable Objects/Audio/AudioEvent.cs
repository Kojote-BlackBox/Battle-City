using UnityEngine;
using UnityEngine.Audio;

public abstract class AudioEvent : ScriptableObject{
	public AudioMixerGroup mixerGroup;
	public abstract void Play(AudioSource source);
}