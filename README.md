# GGJ2026 Unity Template Project

A ready-to-use Unity 6.3 template project designed to help you hit the ground running at Global Game Jam 2026. This template includes pre-configured packages, an organized folder structure, and helper scripts covering common game jam patterns discussed in the presentation.

## Requirements

- **Unity 6.3** (6000.3.x)

## Quick Setup

1. **Clone or download** this repository
2. **Rename** the folder name to your game title
3. **Open Unity Hub** and click "Add" to add the project
4. **Open the project** with Unity 6.3
5. Unity will import all packages and assets automatically
6. Open `Assets/_Game/Scenes/Gameplay.unity` to start working

## Included Packages

This template comes pre-configured with the following Unity packages:

| Package | Description |
|---------|-------------|
| **Cinemachine** | Advanced camera system for smooth follow cameras, cutscenes, and dynamic camera behaviors |
| **Post-Processing (URP)** | Visual effects like bloom, color grading, vignette, and more via the Universal Render Pipeline |
| **TextMesh Pro** | High-quality text rendering with advanced styling options |
| **Input System** | Modern input handling supporting keyboard, gamepad, touch, and custom bindings |
| **Timeline** | Sequencing tool for cutscenes, animations, and scripted events |
| **2D Animation** | Skeletal animation system for 2D sprites and rigging |

## Project Structure

```
Assets/
└── _Game/
    ├── Animations/          # Animation clips and controllers
    ├── Audio/
    │   ├── Music/           # Background music tracks
    │   └── SFX/             # Sound effects
    ├── Materials/           # Materials and shaders
    ├── Prefabs/
    │   ├── Characters/      # Player, enemies, NPCs
    │   ├── Effects/         # Particles, VFX
    │   ├── Environment/     # Level props, obstacles
    │   └── UI/              # UI elements and panels
    ├── Scenes/
    │   ├── MainMenu.unity   # Title/start screen
    │   ├── Gameplay.unity   # Main game scene
    │   └── GameOver.unity   # End screen
    ├── ScriptableObjects/
    │   ├── Events/          # Event channel assets
    │   └── Variables/       # Shared variable assets
    └── Scripts/
        ├── Audio/           # AudioManager
        ├── Events/          # Event system scripts
        ├── Pooling/         # Object pooling
        └── Utilities/       # General helpers
```

## Helper Scripts

### Object Pooling (`SimplePoolManager`)

Object pooling improves performance by reusing GameObjects instead of instantiating and destroying them repeatedly. Perfect for bullets, particles, enemies, and other frequently spawned objects.

**Location:** `Assets/_Game/Scripts/Pooling/SimplePoolManager.cs`

**Setup:**
1. Create an empty GameObject and add the `SimplePoolManager` component
2. Assign the prefab you want to pool
3. Configure the default capacity and max pool size

**Usage:**
```csharp
// Reference the pool manager
[SerializeField] private SimplePoolManager bulletPool;

// Spawn from pool
GameObject bullet = bulletPool.SpawnFromPool(spawnPoint.position, spawnPoint.rotation);

// Return to pool when done
bulletPool.ReturnToPool(bullet);
```

### ScriptableObject Event Channels

A decoupled event system using ScriptableObjects. This pattern allows different systems to communicate without direct references, making your code more modular and easier to maintain.

**Location:** `Assets/_Game/Scripts/Events/`

**Available Event Types:**
| Type | Use Case |
|------|----------|
| `GameEvent` | Simple notifications (no data) |
| `IntGameEvent` | Scores, counts, levels |
| `FloatGameEvent` | Health, time, percentages |
| `BoolGameEvent` | Toggle states, flags |
| `StringGameEvent` | Messages, notifications |
| `Vector3GameEvent` | Positions, directions |
| `GameObjectEvent` | Spawned objects, targets |

**Creating an Event:**
1. Right-click in Project window
2. Navigate to `Create > Events > [Event Type]`
3. Name your event (e.g., "OnPlayerDied", "OnScoreChanged")

**Raising an Event:**
```csharp
[SerializeField] private GameEvent onPlayerDied;
[SerializeField] private IntGameEvent onScoreChanged;

void Die()
{
    onPlayerDied.Raise();
}

void AddScore(int points)
{
    currentScore += points;
    onScoreChanged.Raise(currentScore);
}
```

**Listening to an Event:**
```csharp
[SerializeField] private GameEvent onPlayerDied;
[SerializeField] private IntGameEvent onScoreChanged;

void OnEnable()
{
    onPlayerDied.RegisterListener(HandlePlayerDied);
    onScoreChanged.RegisterListener(UpdateScoreUI);
}

void OnDisable()
{
    onPlayerDied.UnregisterListener(HandlePlayerDied);
    onScoreChanged.UnregisterListener(UpdateScoreUI);
}

void HandlePlayerDied()
{
    // React to player death
}

void UpdateScoreUI(int newScore)
{
    scoreText.text = newScore.ToString();
}
```

**Pre-configured Events:**
The template includes sample events in `Assets/_Game/ScriptableObjects/Events/`:
- `Game/` - OnGameStarted, OnGamePaused, OnGameResumed, OnGameOver
- `Player/` - OnPlayerDied, OnPlayerRespawned, OnPlayerHealthChanged
- `Score/` - OnScoreChanged
- `Combat/` - OnEnemyDiedAt

### Audio Manager

A simple audio management system with volume controls and persistence.

**Location:** `Assets/_Game/Scripts/Audio/AudioManager.cs`

**Features:**
- Master, Music, and SFX volume channels
- Automatic volume persistence via PlayerPrefs
- Optional UI slider integration for settings menus

**Setup:**
1. Add the `AudioManager` component to a GameObject
2. Assign your AudioMixer (must have exposed parameters: `MasterVolume`, `MusicVolume`, `SFXVolume`)
3. Optionally connect UI sliders for a settings menu

## Tips for Game Jams

- **Use the event system** to keep your code decoupled - you'll thank yourself when making last-minute changes
- **Pool frequently spawned objects** like bullets and particles from the start
- **Keep your game in `_Game/`** to separate your work from Unity's default folders
- **Use the pre-made scenes** as starting points - MainMenu, Gameplay, and GameOver are ready to customize
- **Enable debug logs** on events and pools during development (disable for builds)

## License

This template is provided free for use in Global Game Jam 2026 and any other game projects.

---

Good luck and have fun jamming!
