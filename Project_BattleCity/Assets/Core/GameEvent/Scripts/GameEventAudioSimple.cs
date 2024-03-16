using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Event
{
	[CreateAssetMenu(fileName = "NewGameEventAudio", menuName = "Events/GameEventAudio/Simple")]
	public class SimpleAudioEvent : GameEventAudio
	{
		public AudioClip[] clips;

		public RangedFloat volume;

		[MinMaxRange(0, 2)]
		public RangedFloat pitch;

		public bool enableLoop;

		public override void Play(Vector3 position)
		{
			if (clips.Length == 0) return;

			var gameObjectSound = new GameObject();
			gameObjectSound.transform.position = position;
			gameObjectSound.name = identifier;

			var audioSource = gameObjectSound.AddComponent<AudioSource>();

			audioSource.outputAudioMixerGroup = mixerGroup;
			audioSource.clip = clips[Random.Range(0, clips.Length)];
			audioSource.volume = Random.Range(volume.minValue, volume.maxValue);
			audioSource.pitch = Random.Range(pitch.minValue, pitch.maxValue);
			audioSource.loop = enableLoop;

			audioSource.Play();

			Destroy(gameObjectSound, audioSource.clip.length);
		}

		public override void PlayOnSource(AudioSource source)
		{
			if (clips.Length == 0) return;

			source.outputAudioMixerGroup = mixerGroup;
			source.clip = clips[Random.Range(0, clips.Length)];
			source.volume = Random.Range(volume.minValue, volume.maxValue);
			source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
			source.loop = enableLoop;

			source.Play();
		}
	}
}