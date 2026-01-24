using UnityEngine;

/// <summary>
/// Game event that passes a boolean value. Useful for toggle states, flags, enable/disable, etc.
/// </summary>
/// <example>
/// <code>
/// // Raising: onPauseToggled.Raise(true);
/// // Listening: void OnPauseToggled(bool isPaused) { ... }
/// </code>
/// </example>
[CreateAssetMenu(fileName = "New Bool Event", menuName = "Events/Bool Event")]
public class BoolGameEvent : GenericGameEvent<bool> { }