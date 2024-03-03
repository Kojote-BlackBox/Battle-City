using UnityEngine;
using UnityEngine.Audio;

namespace Core.Event
{
	public abstract class GameEventAudio : ScriptableObject
	{
		public string identifier;

		public AudioMixerGroup mixerGroup;

		public abstract void Play(Vector3 position);

		public abstract void PlayOnSource(AudioSource source);
	}
}
