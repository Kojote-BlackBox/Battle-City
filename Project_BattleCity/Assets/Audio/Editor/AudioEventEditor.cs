using UnityEngine;
using Core.Event;
using UnityEditor;

[CustomEditor(typeof(GameEventAudio), true)]
public class AudioEventEditor : Editor
{

	[SerializeField] private AudioSource _previewer;

	public void OnEnable()
	{
		_previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
	}

	public void OnDisable()
	{
		DestroyImmediate(_previewer.gameObject);
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
		if (GUILayout.Button("Preview"))
		{
			((GameEventAudio)target).PlayOnSource(_previewer);
		}
		EditorGUI.EndDisabledGroup();
	}
}