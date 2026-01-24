using UnityEngine;

/// <summary>
/// Game event that passes an integer value. Useful for scores, counts, levels, indices, etc.
/// </summary>
/// <example>
/// <code>
/// // Raising: onScoreChanged.Raise(100);
/// // Listening: void OnScoreChanged(int score) { ... }
/// </code>
/// </example>
[CreateAssetMenu(fileName = "New Int Event", menuName = "Events/Int Event")]
public class IntGameEvent : GenericGameEvent<int> { }