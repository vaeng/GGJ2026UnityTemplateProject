using System;
using UnityEngine;

/// <summary>
/// A generic ScriptableObject-based event that can pass data of type <typeparamref name="T"/> to listeners.
/// Base class for typed game events like <see cref="IntGameEvent"/>, <see cref="FloatGameEvent"/>, etc.
/// </summary>
/// <typeparam name="T">The type of data passed when the event is raised.</typeparam>
/// <remarks>
/// <para>
/// This extends the basic <see cref="GameEvent"/> pattern to support passing data with events.
/// Create concrete implementations by inheriting from this class with a specific type.
/// </para>
/// <para>
/// Common typed events are already provided in TypedGameEvents.cs:
/// <list type="bullet">
/// <item><description><see cref="IntGameEvent"/> - for integer values (scores, counts, etc.)</description></item>
/// <item><description><see cref="FloatGameEvent"/> - for floating point values (health, time, etc.)</description></item>
/// <item><description><see cref="StringGameEvent"/> - for text messages</description></item>
/// <item><description><see cref="BoolGameEvent"/> - for toggle states</description></item>
/// <item><description><see cref="Vector3GameEvent"/> - for positions or directions</description></item>
/// <item><description><see cref="GameObjectEvent"/> - for passing GameObject references</description></item>
/// </list>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Creating a custom typed event:
/// [CreateAssetMenu(fileName = "New Enemy Event", menuName = "Events/Enemy Event")]
/// public class EnemyGameEvent : GenericGameEvent&lt;Enemy&gt; { }
///
/// // Usage in scripts:
/// [SerializeField] private IntGameEvent onScoreChanged;
///
/// void AddScore(int points)
/// {
///     currentScore += points;
///     onScoreChanged.Raise(currentScore);
/// }
///
/// // Listening to the event:
/// void OnEnable() => onScoreChanged.RegisterListener(UpdateScoreUI);
/// void OnDisable() => onScoreChanged.UnregisterListener(UpdateScoreUI);
///
/// void UpdateScoreUI(int newScore)
/// {
///     scoreText.text = newScore.ToString();
/// }
/// </code>
/// </example>
public abstract class GenericGameEvent<T> : ScriptableObject
{
    private Action<T> listeners;

    /// <summary>
    /// Raises the event with the specified value, notifying all registered listeners.
    /// </summary>
    /// <param name="value">The value to pass to all listeners.</param>
    /// <remarks>
    /// In the Unity Editor, this will log a debug message with the value if <c>enableDebugLogs</c> is enabled.
    /// </remarks>
    public void Raise(T value)
    {
#if UNITY_EDITOR
        if (enableDebugLogs)
            Debug.Log($"[Event] {name} raised with value: {value}", this);
#endif

        listeners?.Invoke(value);
    }

    /// <summary>
    /// Registers a listener to be notified when this event is raised.
    /// </summary>
    /// <param name="listener">The callback method to invoke with the event value when raised.</param>
    /// <remarks>
    /// Always unregister listeners in OnDisable to prevent memory leaks and null reference exceptions.
    /// </remarks>
    public void RegisterListener(Action<T> listener)
    {
        listeners += listener;
    }

    /// <summary>
    /// Unregisters a listener so it will no longer be notified when this event is raised.
    /// </summary>
    /// <param name="listener">The callback method to remove.</param>
    public void UnregisterListener(Action<T> listener)
    {
        listeners -= listener;
    }

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] protected bool enableDebugLogs = false;

    [TextArea(3, 10)]
    [SerializeField] protected string description = "Describe what this event represents and what value it passes.";

    private void OnEnable()
    {
        listeners = null;
    }
#endif
}