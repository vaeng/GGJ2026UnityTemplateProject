using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// A simple object pooling system using Unity's built-in ObjectPool.
/// Pre-instantiates and reuses GameObjects to avoid runtime allocation overhead.
/// </summary>
/// <remarks>
/// <para>
/// Object pooling is a performance optimization technique that reuses objects instead of
/// creating and destroying them repeatedly. This is especially useful for:
/// <list type="bullet">
/// <item><description>Bullets and projectiles</description></item>
/// <item><description>Particle effects</description></item>
/// <item><description>Enemies and spawnable objects</description></item>
/// <item><description>UI elements that appear/disappear frequently</description></item>
/// </list>
/// </para>
/// <para>
/// The pool pre-warms on Awake by creating <c>defaultCapacity</c> objects.
/// When <c>maxPoolSize</c> is exceeded, excess objects are destroyed.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Get an object from the pool
/// GameObject bullet = bulletPoolManager.SpawnFromPool(spawnPoint.position, spawnPoint.rotation);
///
/// // Return it when done (e.g., on collision or after a delay)
/// bulletPoolManager.ReturnToPool(bullet);
/// </code>
/// </example>
public class SimplePoolManager : MonoBehaviour
{
    [Header("Pool Settings")]
    [Tooltip("The prefab to pool (bullets, enemies, particles, etc.)")]
    [SerializeField] private GameObject prefabToPool;

    [Tooltip("How many objects to pre-create on start")]
    [SerializeField] private int defaultCapacity = 20;

    [Tooltip("Maximum pool size before destroying excess objects")]
    [SerializeField] private int maxPoolSize = 100;

    [Header("Organization")]
    [Tooltip("Parent transform to keep pooled objects organized in hierarchy")]
    [SerializeField] private Transform poolParent;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = false;

    private ObjectPool<GameObject> pool;

    private int createdCount = 0;
    private int activeCount = 0;

    void Awake()
    {
        if (poolParent == null)
        {
            poolParent = new GameObject($"{prefabToPool.name} Pool").transform;
            poolParent.SetParent(transform);
        }

        // Initialize the pool with four callbacks
        pool = new ObjectPool<GameObject>(
            createFunc: CreatePooledObject,      // How to create new objects
            actionOnGet: OnGetFromPool,          // What to do when getting from pool
            actionOnRelease: OnReturnToPool,     // What to do when returning to pool
            actionOnDestroy: OnDestroyPoolObject,// What to do when pool is full
            collectionCheck: true,               // Safety checks (disable in builds for performance)
            defaultCapacity: defaultCapacity,    // Pre-create theses many objects
            maxSize: maxPoolSize                 // Max pool size before destroying
        );

        PreWarmPool();
    }

    private GameObject CreatePooledObject()
    {
        GameObject obj = Instantiate(prefabToPool, poolParent);
        obj.SetActive(false);

        createdCount++;

        if (showDebugLogs)
            Debug.Log($"[Pool] Created new {prefabToPool.name}. Total created: {createdCount}");

        return obj;
    }

    private void OnGetFromPool(GameObject obj)
    {
        obj.SetActive(true);
        activeCount++;

        if (showDebugLogs)
            Debug.Log($"[Pool] Got {obj.name} from pool. Active: {activeCount}");
    }

    private void OnReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        activeCount--;

        obj.transform.position = poolParent.position;
        obj.transform.rotation = Quaternion.identity;

        if (showDebugLogs)
            Debug.Log($"[Pool] Returned {obj.name} to pool. Active: {activeCount}");
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        if (showDebugLogs)
            Debug.Log($"[Pool] Destroying overflow object {obj.name}");

        Destroy(obj);
    }

    private void PreWarmPool()
    {
        GameObject[] preWarmed = new GameObject[defaultCapacity];

        for (int i = 0; i < defaultCapacity; i++)
        {
            preWarmed[i] = pool.Get();
        }

        for (int i = 0; i < defaultCapacity; i++)
        {
            pool.Release(preWarmed[i]);
        }

        if (showDebugLogs)
            Debug.Log($"[Pool] Pre-warmed pool with {defaultCapacity} objects");
    }

    /// <summary>
    /// Retrieves an object from the pool and positions it in the world.
    /// </summary>
    /// <param name="position">The world position to place the spawned object.</param>
    /// <param name="rotation">The rotation to apply to the spawned object.</param>
    /// <returns>An active GameObject from the pool, positioned and rotated as specified.</returns>
    /// <remarks>
    /// If the pool is empty, a new object will be instantiated automatically.
    /// </remarks>
    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject obj = pool.Get();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    /// <summary>
    /// Retrieves an object from the pool with default rotation (identity).
    /// </summary>
    /// <param name="position">The world position to place the spawned object.</param>
    /// <returns>An active GameObject from the pool at the specified position.</returns>
    public GameObject SpawnFromPool(Vector3 position)
    {
        return SpawnFromPool(position, Quaternion.identity);
    }

    /// <summary>
    /// Returns an object to the pool for later reuse.
    /// </summary>
    /// <param name="obj">The GameObject to return to the pool. Must be currently active.</param>
    /// <remarks>
    /// The object will be deactivated and its transform reset. If the pool has reached
    /// <c>maxPoolSize</c>, the object will be destroyed instead of pooled.
    /// </remarks>
    public void ReturnToPool(GameObject obj)
    {
        if (!obj.activeInHierarchy)
        {
            Debug.LogWarning($"[Pool] Trying to return inactive object {obj.name}");
            return;
        }

        pool.Release(obj);
    }

    /// <summary>
    /// Clears all objects from the pool, destroying them.
    /// </summary>
    /// <remarks>
    /// Use this when transitioning scenes or when the pool is no longer needed.
    /// After clearing, new objects will be instantiated on demand.
    /// </remarks>
    public void ClearPool()
    {
        pool.Clear();
        createdCount = 0;
        activeCount = 0;

        if (showDebugLogs)
            Debug.Log("[Pool] Pool cleared");
    }

    void OnGUI()
    {
        if (!showDebugLogs) return;

        GUILayout.BeginArea(new Rect(10, 10, 300, 100));
        GUILayout.Label($"Pool: {prefabToPool.name}");
        GUILayout.Label($"Created: {createdCount}");
        GUILayout.Label($"Active: {activeCount}");
        GUILayout.Label($"Available: {createdCount - activeCount}");
        GUILayout.EndArea();
    }
}