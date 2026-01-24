using UnityEngine;

/// <summary>
/// Game event that passes a GameObject reference. Useful for spawned objects, targets, interactables, etc.
/// </summary>
/// <remarks>
/// Be cautious with GameObject events - ensure the referenced object is still valid when listeners receive it.
/// The GameObject may have been destroyed between raising and handling the event.
/// </remarks>
/// <example>
/// <code>
/// // Raising: onEnemySpawned.Raise(enemyGameObject);
/// // Listening: void OnEnemySpawned(GameObject enemy) { if (enemy != null) ... }
/// </code>
/// </example>
[CreateAssetMenu(fileName = "New GameObject Event", menuName = "Events/GameObject Event")]
public class GameObjectEvent : GenericGameEvent<GameObject> { }