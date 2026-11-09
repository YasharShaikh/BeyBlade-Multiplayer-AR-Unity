# Beyblade Multiplayer AR

A Unity prototype for multiplayer spinning-top battles. The configured build starts with a PlayFab login and Photon lobby, then moves through player selection into the battle scene. The repository also includes a separate AR arena placement scene.

## Systems

| Area | Where to look |
| --- | --- |
| Account and connection flow | [LobbyManager.cs](Assets/Scripts/LobbyManager.cs) handles PlayFab login, display names, and the Photon connection. |
| Match flow | [PlayerSelectionManager.cs](Assets/Scripts/PlayerSelectionManager.cs), [SpawnManager.cs](Assets/Scripts/SpawnManager.cs), and [PlayerSync.cs](Assets/Scripts/PlayerSync.cs). |
| Top movement and battle | [MovementController.cs](Assets/Scripts/MovementController.cs), [Spinner.cs](Assets/Scripts/Spinner.cs), and [BattleScript.cs](Assets/Scripts/BattleScript.cs). |
| Arena placement | [ARPlacementManager.cs](Assets/Scripts/ARPlacementManager.cs) uses a screen-center raycast against detected planes. It also has a fixed-position test mode. |

## Run the prototype

1. Open the repository root in Unity Hub with **Unity 6000.0.27f1**.
2. Set up a PlayFab title for client login and a Photon PUN app for networking. Configure their project settings before starting the lobby.
3. Open [Scene_Lobby](Assets/Scenes/Scene_Lobby.unity) and enter the match flow. Use two clients to exercise multiplayer behavior.
4. To test arena placement, open [Scene_ARGameplay](Assets/Scenes/Scene_ARGameplay.unity) separately. Enable the relevant XR settings, disable **Disable AR Placement** on the placement component for plane detection, and test on an ARCore-compatible Android device.

The package manifest specifies **AR Foundation 6.0.5** and **ARCore XR Plugin 6.0.5**. The [build settings](ProjectSettings/EditorBuildSettings.asset) list the lobby, loading, player selection, and standard gameplay scenes.
