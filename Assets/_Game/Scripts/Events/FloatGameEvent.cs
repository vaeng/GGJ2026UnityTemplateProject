using UnityEngine;

/// <summary>
/// Game event that passes a float value. Useful for health, time, percentages, distances, etc.
/// </summary>
/// <example>
/// <code>
/// // Raising: onHealthChanged.Raise(0.75f);
/// // Listening: void OnHealthChanged(float healthPercent) { ... }
/// </code>
/// </example>
[CreateAssetMenu(fileName = "New Float Event", menuName = "Events/Float Event")]
public class FloatGameEvent : GenericGameEvent<float> { }