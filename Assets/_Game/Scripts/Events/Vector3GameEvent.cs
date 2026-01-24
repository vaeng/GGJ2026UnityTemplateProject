using UnityEngine;

/// <summary>
/// Game event that passes a Vector3 value. Useful for positions, directions, velocities, etc.
/// </summary>
/// <example>
/// <code>
/// // Raising: onPlayerMoved.Raise(transform.position);
/// // Listening: void OnPlayerMoved(Vector3 position) { ... }
/// </code>
/// </example>
[CreateAssetMenu(fileName = "New Vector3 Event", menuName = "Events/Vector3 Event")]
public class Vector3GameEvent : GenericGameEvent<Vector3> { }