using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerSetting {
    [NonSerialized]
    public PlayerInput playerInput;
    [SerializeField]
    public string controlScheme;
}

public class PlayerLobby : MonoBehaviour {
    [Header("Settings")]
    [Space(10)]
    [Tooltip("The prefab that is used to spawn the player.")]
    [SerializeField]
    private GameObject playerPrefab;

    [Tooltip("The list of players.")]
    [SerializeField]
    private List<PlayerSetting> players;
    
    private PlayerInputManager playerInputManager;

    private void Awake() {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void Start() {
        if (playerInputManager == null || playerInputManager.joinBehavior != PlayerJoinBehavior.JoinPlayersManually) return;
        
        var maxPlayer = playerInputManager.maxPlayerCount < players.Count ? playerInputManager.maxPlayerCount : players.Count;

        for (var index = 0; index < maxPlayer; index++) {
            var playerSetting = players[index];
            var controlScheme = playerSetting.controlScheme;

            playerSetting.playerInput = controlScheme.Contains("Keyboard") ? 
                PlayerInput.Instantiate(playerPrefab, controlScheme: playerSetting.controlScheme, pairWithDevice: Keyboard.current) 
                : PlayerInput.Instantiate(playerPrefab, controlScheme: playerSetting.controlScheme, pairWithDevice: Gamepad.current);
        }
    }
}
