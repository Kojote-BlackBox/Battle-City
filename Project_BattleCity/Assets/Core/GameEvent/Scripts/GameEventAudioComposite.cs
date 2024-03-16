using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Event
{
	[CreateAssetMenu(fileName = "NewGameEventAudio", menuName = "Events/GameEventAudio/Composite")]
	public class CompositeAudioEvent : GameEventAudio
	{
		[Serializable]
		public struct CompositeEntry
		{
			public GameEventAudio Event;
			public float Weight;
		}

		public CompositeEntry[] Entries;

		public override void Play(Vector3 position)
		{
			float totalWeight = 0;
			for (int i = 0; i < Entries.Length; ++i)
				totalWeight += Entries[i].Weight;

			float pick = Random.Range(0, totalWeight);
			for (int i = 0; i < Entries.Length; ++i)
			{
				if (pick > Entries[i].Weight)
				{
					pick -= Entries[i].Weight;
					continue;
				}

				Entries[i].Event.Play(position);
				return;
			}
		}

		public override void PlayOnSource(AudioSource source)
		{
			throw new System.NotImplementedException();
		}
	}
}