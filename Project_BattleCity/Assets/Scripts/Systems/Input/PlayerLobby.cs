using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLobby : MonoBehaviour {
    [Header("Player Settings")]
    [Space(10)]

    [Tooltip("The prefab that is used to spawn the player.")]
    [SerializeField]
    private GameObject playerPrefab;

    private PlayerInput firstPlayer;
    private PlayerInput secondPlayer;

    private void Start() {
        firstPlayer = PlayerInput.Instantiate(playerPrefab, controlScheme: "Keyboard - WASD", pairWithDevice: Keyboard.current);
        secondPlayer = PlayerInput.Instantiate(playerPrefab, controlScheme: "Keyboard - ARROWS", pairWithDevice: Keyboard.current);
    }
}
