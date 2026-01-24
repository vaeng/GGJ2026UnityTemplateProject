using System;
using UnityEngine;

/// <summary>
/// A ScriptableObject-based event system for decoupled communication between game systems.
/// Allows objects to raise and listen to events without direct references to each other.
/// </summary>
/// <remarks>
/// <para>
/// This implements the Observer pattern using ScriptableObjects, enabling:
/// <list type="bullet">
/// <item><description>Decoupled architecture - publishers and subscribers don't need references to each other</description></item>
/// <item><description>Designer-friendly workflow - events can be created and assigned in the Unity Editor</description></item>
/// <item><description>Easy debugging - events can be inspected and tested in the Editor</description></item>
/// </list>
/// </para>
/// <para>
/// For events that need to pass data, use <see cref="GenericGameEvent{T}"/> or one of the
/// typed variants like <see cref="IntGameEvent"/>, <see cref="FloatGameEvent"/>, etc.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // In a script that raises the event:
/// [SerializeField] private GameEvent onPlayerDied;
///
/// void Die()
/// {
///     onPlayerDied.Raise();
/// }
///
/// // In a script that listens to the event:
/// [SerializeField] private GameEvent onPlayerDied;
///
/// void OnEnable() => onPlayerDied.RegisterListener(HandlePlayerDied);
/// void OnDisable() => onPlayerDied.UnregisterListener(HandlePlayerDied);
///
/// void HandlePlayerDied()
/// {
///     // React to player death
/// }
/// </code>
/// </example>
[CreateAssetMenu(fileName = "New Game Event", menuName = "Events/Game Event")]
public class GameEvent : ScriptableObject
{
    private Action listeners;

    /// <summary>
    /// Raises the event, notifying all registered listeners.
    /// </summary>
    /// <remarks>
    /// In the Unity Editor, this will log a debug message if <c>enableDebugLogs</c> is enabled.
    /// </remarks>
    public void Raise()
    {
#if UNITY_EDITOR
        if (enableDebugLogs)
            Debug.Log($"[Event] {name} raised", this);
#endif

        listeners?.Invoke();
    }

    /// <summary>
    /// Registers a listener to be notified when this event is raised.
    /// </summary>
    /// <param name="listener">The callback method to invoke when the event is raised.</param>
    /// <remarks>
    /// Always unregister listeners in OnDisable to prevent memory leaks and null reference exceptions.
    /// </remarks>
    public void RegisterListener(Action listener)
    {
        listeners += listener;
    }

    /// <summary>
    /// Unregisters a listener so it will no longer be notified when this event is raised.
    /// </summary>
    /// <param name="listener">The callback method to remove.</param>
    public void UnregisterListener(Action listener)
    {
        listeners -= listener;
    }

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool enableDebugLogs = false;

    [TextArea(3, 10)]
    [SerializeField] private string description = "Describe what this event represents and when it should be raised.";

    private void OnEnable()
    {
        listeners = null;
    }
#endif
}