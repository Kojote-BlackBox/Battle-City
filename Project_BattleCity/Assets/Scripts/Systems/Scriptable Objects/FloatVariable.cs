using UnityEngine;

[CreateAssetMenu(fileName = "NewFloatVariable", menuName = "Variables/Float")]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver {

    public float InitialValue;
    [System.NonSerialized]
    public float RuntimeValue;

    public void OnAfterDeserialize() {
        RuntimeValue = InitialValue;
    }

    public void OnBeforeSerialize() { }
}
