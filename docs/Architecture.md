# W1Style Game — Architecture Guide

> **Unity Version:** 6000.0.49f1 (Unity 6)

## 1. Folder Structure

```
Assets/
├── Plugins/                               # Third-party libraries (managed as assets)
│   ├── Demigiant/DOTween/                 # Tween/animation library
│   ├── I2/Localization/                   # Multi-language localization system
│   ├── Sirenix/                           # Odin Inspector & Serializer
│   └── Zenject/                           # Dependency Injection framework
├── ThirdParty/                            # Other third-party assets (non-Plugin)
└── _Project/                              # All project-specific code and content
    ├── Source/                            # C# source code (assembly definitions)
    │   ├── Bootstrap/Runtime/             # Startup logic, composition root
    │   │   ├── BootstrapController.cs
    │   │   ├── BootstrapSceneInstaller.cs
    │   │   ├── ProjectInstaller.cs
    │   │   ├── ConfigInstaller.cs
    │   │   └── W1Style.Bootstrap.asmdef
    │   ├── Core/Runtime/                  # Shared interfaces, events, constants
    │   │   ├── Interfaces/
    │   │   │   ├── ILogService.cs
    │   │   │   ├── ISceneService.cs
    │   │   │   ├── ITimeProvider.cs
    │   │   │   ├── ISaveService.cs
    │   │   │   ├── IEventBus.cs
    │   │   │   └── IAppLifecycleService.cs
    │   │   ├── Events/
    │   │   │   └── GameEvents.cs
    │   │   ├── Constants/
    │   │   │   └── SceneNames.cs
    │   │   └── W1Style.Core.asmdef
    │   ├── Infrastructure/Runtime/        # Service implementations
    │   │   ├── Services/
    │   │   │   ├── LogService.cs
    │   │   │   ├── SceneService.cs
    │   │   │   ├── UnityTimeProvider.cs
    │   │   │   ├── EventBus.cs
    │   │   │   ├── AppLifecycleService.cs
    │   │   │   └── PlayerPrefsSaveService.cs
    │   │   └── W1Style.Infrastructure.asmdef
    │   ├── Configs/Runtime/               # ScriptableObject config definitions
    │   │   ├── GameConfig.cs
    │   │   ├── AudioConfig.cs
    │   │   ├── GameplayConfig.cs
    │   │   ├── UIConfig.cs
    │   │   └── W1Style.Configs.asmdef
    │   ├── Features/                      # Feature modules (one folder per feature)
    │   │   └── Inventory/Runtime/
    │   │       ├── Domain/
    │   │       │   ├── InventoryItem.cs
    │   │       │   └── IInventoryService.cs
    │   │       ├── Services/
    │   │       │   └── InventoryService.cs
    │   │       ├── Config/
    │   │       │   └── InventoryConfig.cs
    │   │       ├── Installer/
    │   │       │   └── InventoryInstaller.cs
    │   │       └── W1Style.Features.Inventory.asmdef
    │   ├── UI/Runtime/                    # UI framework and shared components
    │   │   ├── Core/
    │   │   │   ├── UIPanel.cs
    │   │   │   └── IUIService.cs
    │   │   ├── Services/
    │   │   │   └── UIService.cs
    │   │   └── W1Style.UI.asmdef
    │   └── Editor/                        # Editor-only tools and utilities
    │       ├── Tools/
    │       │   ├── BootstrapSceneLoader.cs
    │       │   ├── ConfigValidator.cs
    │       │   └── ProjectMenuItems.cs
    │       └── W1Style.Editor.asmdef
    ├── Tests/                             # Test assemblies
    │   ├── EditMode/
    │   │   ├── Core/
    │   │   │   └── EventBusTests.cs
    │   │   ├── Features/
    │   │   │   └── InventoryItemTests.cs
    │   │   └── W1Style.Tests.EditMode.asmdef
    │   └── PlayMode/
    │       └── W1Style.Tests.PlayMode.asmdef
    ├── Art/                               # Visual assets
    │   ├── Animations/
    │   ├── Materials/
    │   ├── Models/
    │   └── Sprites/
    ├── Audio/                             # Audio assets
    │   ├── Music/
    │   └── SFX/
    ├── Prefabs/                           # Shared prefabs (non-feature-specific)
    ├── Scenes/                            # All scene files
    │   ├── Bootstrap.unity
    │   ├── MainMenu.unity
    │   └── Gameplay.unity
    ├── Settings/                          # Config .asset files
    │   ├── GameConfig.asset
    │   ├── AudioConfig.asset
    │   ├── GameplayConfig.asset
    │   └── UIConfig.asset
    └── Resources/                         # Only for Resources.Load assets
        ├── ProjectContext.prefab
        ├── DOTweenSettings.asset
        └── I2Languages.asset
```

---

## 2. Folder Responsibilities

| Folder | Responsibility |
|--------|---------------|
| `Plugins/` | Third-party libraries distributed as assets: Zenject, DOTween, I2 Localization, Odin Inspector (Sirenix). Not modified by project code. |
| `ThirdParty/` | Additional third-party assets not distributed via Package Manager or Plugins. |
| `_Project/` | **Everything project-specific.** Both source code and content live here, cleanly separated from third-party assets. |
| `_Project/Source/` | All C# source code, organized by architectural module. Each module has its own assembly definition. |
| `_Project/Source/Bootstrap` | Composition root. ProjectInstaller, ConfigInstaller, BootstrapController. This is where the game starts and global DI bindings are defined. |
| `_Project/Source/Core` | Pure interfaces, events, constants, and data types shared across the project. **No implementations, no Unity dependencies** (noEngineReferences: true). |
| `_Project/Source/Infrastructure` | Concrete implementations of Core interfaces. Unity-dependent services (SceneService, TimeProvider, LogService, etc.). |
| `_Project/Source/Configs` | ScriptableObject class definitions for all config types. The actual `.asset` files live in `_Project/Settings/`. |
| `_Project/Source/Features` | One subfolder per gameplay feature. Each feature is self-contained with its own domain, services, config, installer, and asmdef. |
| `_Project/Source/UI` | Shared UI framework: base panel class, UI service, common UI utilities. Feature-specific UI lives inside the feature folder. |
| `_Project/Source/Editor` | Editor-only tools: menu items, validators, custom inspectors. Compiled only in the Unity Editor. |
| `_Project/Tests/` | EditMode and PlayMode test assemblies. Mirror the source folder structure. |
| `_Project/Art/` | Sprites, models, materials, animations, shaders. Organized by type, not by feature. |
| `_Project/Audio/` | Music tracks and sound effects. |
| `_Project/Prefabs/` | Shared prefabs not owned by a specific feature. |
| `_Project/Scenes/` | All Unity scene files with consistent naming. |
| `_Project/Settings/` | Config `.asset` files (GameConfig, AudioConfig, GameplayConfig, UIConfig). |
| `_Project/Resources/` | **Minimal use.** Only for assets that must use `Resources.Load` — Zenject ProjectContext, DOTweenSettings, I2Languages. |

---

## 3. Zenject Architecture & Installer Layout

### ProjectContext (Global / Cross-Scene)
Lives in `Assets/_Project/Resources/ProjectContext.prefab`. Unity automatically loads this via `Resources.Load`.

**Attached Installers:**
- `ProjectInstaller` — Binds all global services (ILogService, IEventBus, ITimeProvider, ISceneService, ISaveService, IAppLifecycleService)
- `ConfigInstaller` — Binds GameConfig and all sub-configs (AudioConfig, GameplayConfig, UIConfig)

**What belongs in ProjectContext:**
- Services that must persist across scene loads
- Configuration that is needed globally
- Singleton services with no scene-specific state

### SceneContext (Per-Scene)
Each scene can have a `SceneContext` GameObject with scene-level installers.

**What belongs in SceneContext:**
- Scene-specific bindings (e.g., InventoryInstaller for a gameplay scene)
- UI panels and controllers specific to that scene
- Feature installers relevant only to that scene's gameplay

### Installer Naming Convention
| Installer | Scope | Purpose |
|-----------|-------|---------|
| `ProjectInstaller` | Global | Core infrastructure services |
| `ConfigInstaller` | Global | Config ScriptableObject bindings |
| `BootstrapSceneInstaller` | Scene | Bootstrap scene bindings |
| `InventoryInstaller` | Scene/Feature | Inventory feature bindings |
| `{Feature}Installer` | Scene/Feature | Any feature's DI bindings |

### Rules to Avoid Installer Mess
1. **One installer per concern.** Don't put all bindings in one mega-installer.
2. **Feature installers stay inside feature folders.** They are added to SceneContext only in scenes that need them.
3. **Global services go in ProjectInstaller.** Scene-level services go in scene installers.
4. **Config bindings are separate** from service bindings (ConfigInstaller vs ProjectInstaller).
5. **Never bind the same interface in both ProjectContext and SceneContext** unless intentionally overriding.

---

## 4. Bootstrap Flow

### Startup Order
```
1. Unity loads Bootstrap scene (Build Settings index 0)
   Location: Assets/_Project/Scenes/Bootstrap.unity
2. ProjectContext prefab auto-loads from _Project/Resources/
   └── ProjectInstaller.InstallBindings()     → global services registered
   └── ConfigInstaller.InstallBindings()      → configs bound
3. Bootstrap scene's SceneContext initializes
   └── BootstrapSceneInstaller.InstallBindings()
4. BootstrapController.Start() executes
   └── Publishes GameStateChangedEvent (None → Bootstrap)
   └── Runs startup sequence (data loading, validation, etc.)
   └── Publishes GameStateChangedEvent (Bootstrap → MainMenu)
   └── Calls ISceneService.LoadSceneAsync("MainMenu")
5. MainMenu scene loads with its own SceneContext
```

### Key Principles
- Bootstrap scene should be **lightweight** — no heavy assets.
- All global services are available before `BootstrapController.Start()`.
- The bootstrap scene is **never returned to** — it's a one-time entry point.
- Debug play: Set Bootstrap as Scene 0 in Build Settings. For quick iteration, use the editor tools to open specific scenes.

---

## 5. Config Architecture

### Design
```
GameConfig (root)
├── AudioConfig
├── GameplayConfig
└── UIConfig
```

- **GameConfig** is the single root asset linking all subsystem configs.
- Each sub-config is a separate ScriptableObject for modularity and version control.
- ConfigInstaller binds both the root and each sub-config individually, so services can inject exactly what they need.
- Odin Inspector `[Required]` attributes validate references in the inspector.

### Config File Locations
- **Class definitions**: `_Project/Source/Configs/Runtime/` (e.g., `GameConfig.cs`, `AudioConfig.cs`)
- **Asset instances**: `_Project/Settings/` (e.g., `GameConfig.asset`, `AudioConfig.asset`)

### Creating Config Assets
1. Right-click in Project: `Create → W1Style/Configs/Game Config`
2. Right-click in Project: `Create → W1Style/Configs/Audio Config`, etc.
3. Assign sub-configs to the GameConfig asset.
4. Assign GameConfig to the ConfigInstaller on the ProjectContext prefab.

### Adding New Configs
1. Create a new `XyzConfig : ScriptableObject` in `_Project/Source/Configs/Runtime/`.
2. Add a reference field in `GameConfig`.
3. Add a binding in `ConfigInstaller`.
4. Create the `.asset` file in `_Project/Settings/`.
5. Inject `XyzConfig` in any service that needs it.

---

## 6. Foundational Services

| Service | Interface | Implementation | Scope | Purpose |
|---------|-----------|---------------|-------|---------|
| Logging | `ILogService` | `LogService` | Global | Abstracts Debug.Log. Enables filtering, structured logging, disabling in release builds. |
| Scene Loading | `ISceneService` | `SceneService` | Global | Wraps SceneManager. Supports sync, async, additive loading. |
| Time | `ITimeProvider` | `UnityTimeProvider` | Global | Abstracts Time API. Enables deterministic testing. |
| Event Bus | `IEventBus` | `EventBus` | Global | Type-keyed pub/sub for decoupled cross-system communication. |
| Save/Load | `ISaveService` | `PlayerPrefsSaveService` | Global | Abstracts persistence. Swap implementation for file/cloud storage. |
| App Lifecycle | `IAppLifecycleService` | `AppLifecycleService` | Global | Centralizes pause/resume/quit handling. Ensures cleanup on app transitions. |

---

## 7. Feature Module Structure (Inventory Example)

```
Source/Features/Inventory/
└── Runtime/
    ├── Domain/                    # Pure data models and interfaces
    │   ├── InventoryItem.cs       # Plain C# data class
    │   └── IInventoryService.cs   # Feature interface
    ├── Services/                  # Concrete implementations
    │   └── InventoryService.cs    # Business logic
    ├── Config/                    # Feature-specific config
    │   └── InventoryConfig.cs     # ScriptableObject with tuning values
    ├── Installer/                 # Zenject installer
    │   └── InventoryInstaller.cs  # Binds IInventoryService → InventoryService
    └── W1Style.Features.Inventory.asmdef
```

### Adding a New Feature
1. Create `Source/Features/{FeatureName}/Runtime/` with Domain, Services, Config, Installer subfolders.
2. Create a `W1Style.Features.{FeatureName}.asmdef` referencing `W1Style.Core` (and `W1Style.Configs` if needed).
3. Define interfaces in Domain, implementations in Services, config in Config.
4. Create `{FeatureName}Installer` and add it to the appropriate SceneContext.

---

## 8. Assembly Definition Dependency Graph

```
W1Style.Core                  (no dependencies, noEngineReferences: true)
    ↑
W1Style.Configs              (→ Core)
    ↑
W1Style.Infrastructure       (→ Core, Configs, Zenject)
    ↑
W1Style.Bootstrap            (→ Core, Configs, Infrastructure, Zenject)

W1Style.UI                   (→ Core, Zenject)
W1Style.Features.Inventory   (→ Core, Configs, Zenject)
W1Style.Features.*           (→ Core, Configs, Zenject)

W1Style.Editor               (→ Core, Configs, Infrastructure — Editor only)

W1Style.Tests.EditMode       (→ Core, Infrastructure, Features — Editor only)
W1Style.Tests.PlayMode       (→ Core, Infrastructure, Bootstrap, Features, UI, Zenject)
```

### Key Rules
- **Core has zero dependencies** and `noEngineReferences: true` — pure C#.
- **Features never reference Infrastructure** directly; they depend on Core interfaces.
- **Features never reference other features** — communicate via IEventBus.
- **Editor assembly is Editor-only** platform.
- **Test assemblies** use `overrideReferences: true` and reference `nunit.framework.dll`.
- **Zenject** is referenced only by assemblies that define installers or use `[Inject]`.
- **Odin Inspector** is referenced via `versionDefines` and conditional `#if ODIN_INSPECTOR` to keep it optional.

---

## 9. Scene Strategy

### Current Scenes
All scenes live in `Assets/_Project/Scenes/`.

| Scene | Purpose | Notes |
|-------|---------|-------|
| `Bootstrap` | Entry point (Build Index 0) | Lightweight, runs startup sequence |
| `MainMenu` | Main menu UI | First scene after bootstrap |
| `Gameplay` | Core gameplay | Primary game scene |

### Naming Convention
- PascalCase, no spaces: `Bootstrap`, `MainMenu`, `Gameplay`
- Test scenes prefixed: `Test_InventoryUI`, `Test_Combat`
- Development scenes: `Dev_Sandbox`, `Dev_LevelDesign`

### Rules
- `Bootstrap` is always Build Index 0.
- Test/Dev scenes are excluded from builds.
- Each scene has its own SceneContext if it needs DI bindings.
- Avoid scene proliferation — use additive scenes for sub-areas.

---

## 10. Naming Conventions

| Element | Convention | Example |
|---------|-----------|---------|
| **Folders** | PascalCase | `Bootstrap`, `Infrastructure`, `Features` |
| **Namespaces** | `W1Style.{Area}.{SubArea}` | `W1Style.Core.Interfaces`, `W1Style.Features.Inventory.Domain` |
| **Interfaces** | `I` prefix | `ILogService`, `IInventoryService` |
| **Services** | Suffix `Service` | `LogService`, `SceneService` |
| **MonoBehaviours** | Descriptive, suffix `Controller`/`View` | `BootstrapController`, `InventoryView` |
| **ScriptableObjects** | Suffix `Config` | `GameConfig`, `AudioConfig`, `InventoryConfig` |
| **Installers** | Suffix `Installer` | `ProjectInstaller`, `InventoryInstaller` |
| **Events** | Suffix `Event` | `GameStateChangedEvent`, `SceneLoadedEvent` |
| **Tests** | Suffix `Tests` | `EventBusTests`, `InventoryItemTests` |
| **Factories** | Suffix `Factory` | `EnemyFactory`, `ItemFactory` |
| **Signals** (Zenject) | Suffix `Signal` | `PlayerDiedSignal`, `LevelCompletedSignal` |
| **Constants** | Static class, PascalCase | `SceneNames.Bootstrap` |
| **Private fields** | `_camelCase` | `_logService`, `_config` |
| **Assembly Defs** | `W1Style.{Area}` | `W1Style.Core`, `W1Style.Features.Inventory` |

---

## 11. Testing Structure

```
Tests/
├── EditMode/                          # Fast tests, no scene loading
│   ├── Core/                          # Core abstraction tests
│   │   └── EventBusTests.cs
│   ├── Features/                      # Feature domain/service tests
│   │   └── InventoryItemTests.cs
│   └── W1Style.Tests.EditMode.asmdef
└── PlayMode/                          # Tests requiring Unity runtime
    └── W1Style.Tests.PlayMode.asmdef
```

### Testing Strategy
- **EditMode tests** for pure C# logic (EventBus, domain models, services with mocked dependencies).
- **PlayMode tests** for MonoBehaviour integration, scene loading, UI interaction.
- **Mock dependencies** by implementing interfaces directly — no mocking framework required for simple cases.
- **Zenject testability**: Services accept interfaces via constructor injection, making them trivially mockable.
- Services that depend on `ILogService` can receive a no-op implementation in tests.

---

## 12. Editor Tooling

| Tool | Menu Path | Purpose |
|------|-----------|---------|
| Bootstrap Scene Loader | `W1Style/Scenes/Open Bootstrap Scene` | Quick-open Bootstrap scene |
| MainMenu Scene Loader | `W1Style/Scenes/Open MainMenu Scene` | Quick-open MainMenu scene |
| Gameplay Scene Loader | `W1Style/Scenes/Open Gameplay Scene` | Quick-open Gameplay scene |
| Config Validator | `W1Style/Validation/Validate Game Config` | Checks all config references are assigned |
| Open Configs Folder | `W1Style/Open/Project Configs Folder` | Navigate to config assets |
| Open Scenes Folder | `W1Style/Open/Scenes Folder` | Navigate to scenes |
| Clear PlayerPrefs | `W1Style/Clear/PlayerPrefs` | Wipe saved data during development |

---

## First-Time Setup Checklist

1. Open the project in Unity 6 (6000.0.49f1 or compatible).
2. Plugins are pre-installed in `Assets/Plugins/`:
   - Zenject (DI), Odin Inspector (Sirenix), DOTween (Demigiant), I2 Localization.
3. UniTask is pre-configured via Package Manager (`Packages/manifest.json`).
4. `ProjectContext` prefab already exists in `Assets/_Project/Resources/`:
   - Verify `ProjectInstaller` and `ConfigInstaller` are attached as MonoInstallers.
5. Config assets already exist in `Assets/_Project/Settings/`:
   - `GameConfig.asset`, `AudioConfig.asset`, `GameplayConfig.asset`, `UIConfig.asset`.
   - Verify sub-configs are assigned in GameConfig.
   - Verify GameConfig is assigned to ConfigInstaller on the ProjectContext prefab.
6. Scenes already exist in `Assets/_Project/Scenes/`:
   - `Bootstrap.unity` — verify it is Build Index 0 with SceneContext + BootstrapController.
   - `MainMenu.unity`, `Gameplay.unity` — add SceneContext as needed.
7. Run `W1Style/Validation/Validate Game Config` to verify setup.
8. Press Play in Bootstrap scene to test the full startup flow.

---

## Third-Party Libraries

### Installed in `Assets/Plugins/`

| Library | Folder | Purpose |
|---------|--------|---------|
| **Zenject** | `Plugins/Zenject/` | Dependency Injection framework. Provides ProjectContext, SceneContext, MonoInstaller, and `[Inject]` attribute. |
| **Odin Inspector** | `Plugins/Sirenix/` | Enhanced Unity Inspector with attributes like `[Required]`, `[Title]`, `[PropertyRange]`. Used in config ScriptableObjects for validation and grouping. |
| **DOTween** | `Plugins/Demigiant/DOTween/` | High-performance tween/animation library. Use for UI animations, transitions, gameplay effects. |
| **I2 Localization** | `Plugins/I2/Localization/` | Multi-language localization system. Language asset is at `_Project/Resources/I2Languages.asset`. |

### Installed via Package Manager (`Packages/manifest.json`)

| Package | Source | Purpose |
|---------|--------|---------|
| **UniTask** | `com.cysharp.unitask` (git) | Allocation-free async/await for Unity. Use instead of coroutines for async operations. |
| **Unity Test Framework** | `com.unity.test-framework` 1.5.1 | NUnit-based testing for EditMode and PlayMode tests. |
| **Unity UI** | `com.unity.ugui` 2.0.0 | Unity's built-in UI system (Canvas, EventSystem). |
