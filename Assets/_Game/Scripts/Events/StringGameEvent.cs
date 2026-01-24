using UnityEngine;

/// <summary>
/// Game event that passes a string value. Useful for messages, names, dialogue, notifications, etc.
/// </summary>
/// <example>
/// <code>
/// // Raising: onNotification.Raise("Level Complete!");
/// // Listening: void OnNotification(string message) { ... }
/// </code>
/// </example>
[CreateAssetMenu(fileName = "New String Event", menuName = "Events/String Event")]
public class StringGameEvent : GenericGameEvent<string> { }