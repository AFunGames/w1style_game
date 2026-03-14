using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using W1Style.Features.Combat.Components;
using W1Style.Features.Combat.Enemy;
using W1Style.Features.Combat.Hazards;
using W1Style.Features.Combat.Interaction;
using W1Style.Features.Combat.Noise;
using W1Style.Features.Combat.Physics;
using W1Style.Features.Combat.Player;

namespace W1Style.Features.Combat.Editor
{
    /// <summary>
    /// Editor utility that generates the combat sandbox test scene
    /// with all required objects: player, enemies, physics objects,
    /// hazards, and interactables.
    /// </summary>
    public static class SandboxSceneBuilder
    {
        [MenuItem("W1Style/Combat/Build Sandbox Scene", false, 100)]
        public static void BuildScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            scene.name = "Sandbox";

            // Remove default camera — player has its own
            var mainCam = Camera.main;
            if (mainCam != null)
                Object.DestroyImmediate(mainCam.gameObject);

            BuildArena();
            BuildPlayer();
            BuildEnemies();
            BuildPhysicsObjects();
            BuildHazards();
            BuildInteractables();
            BuildLighting();

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, "Assets/_Project/Scenes/Sandbox.unity");
            Debug.Log("[SandboxSceneBuilder] Sandbox scene created at Assets/_Project/Scenes/Sandbox.unity");
        }

        private static void BuildArena()
        {
            // Floor
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Floor";
            floor.transform.position = Vector3.zero;
            floor.transform.localScale = new Vector3(40f, 0.5f, 40f);
            floor.isStatic = true;

            // Walls
            CreateWall("Wall_North", new Vector3(0f, 2f, 20f), new Vector3(40f, 4f, 0.5f));
            CreateWall("Wall_South", new Vector3(0f, 2f, -20f), new Vector3(40f, 4f, 0.5f));
            CreateWall("Wall_East", new Vector3(20f, 2f, 0f), new Vector3(0.5f, 4f, 40f));
            CreateWall("Wall_West", new Vector3(-20f, 2f, 0f), new Vector3(0.5f, 4f, 40f));
        }

        private static void CreateWall(string name, Vector3 position, Vector3 scale)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.isStatic = true;
        }

        private static void BuildPlayer()
        {
            var player = new GameObject("Player");
            player.transform.position = new Vector3(0f, 1.5f, -10f);
            player.layer = LayerMask.NameToLayer("Default");

            // Rigidbody
            var rb = player.AddComponent<Rigidbody>();
            rb.mass = 70f;
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            // Capsule collider
            var capsule = player.AddComponent<CapsuleCollider>();
            capsule.height = 2f;
            capsule.radius = 0.4f;
            capsule.center = new Vector3(0f, 1f, 0f);

            // Camera
            var camObj = new GameObject("PlayerCamera");
            camObj.transform.SetParent(player.transform);
            camObj.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            var cam = camObj.AddComponent<Camera>();
            cam.nearClipPlane = 0.1f;
            cam.fieldOfView = 75f;
            camObj.AddComponent<AudioListener>();

            // Player components
            player.AddComponent<PlayerController>();
            player.AddComponent<MouseLook>();
            player.AddComponent<PlayerCombat>();
            player.AddComponent<PlayerInteraction>();
            player.AddComponent<PlayerThrow>();

            // PlayerInput for Input System
            var playerInput = player.AddComponent<PlayerInput>();
            var inputAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(
                "Assets/_Project/Settings/Input Controls.inputactions");
            if (inputAsset != null)
            {
                playerInput.actions = inputAsset;
                playerInput.defaultActionMap = "Player";
                playerInput.notificationBehavior = PlayerNotifications.SendMessages;
            }
            else
            {
                Debug.LogWarning("[SandboxSceneBuilder] Input Controls.inputactions not found.");
            }
        }

        private static void BuildEnemies()
        {
            // Normal enemies
            CreateEnemy("Enemy_1", new Vector3(5f, 0.25f, 5f), 50f, false);
            CreateEnemy("Enemy_2", new Vector3(-5f, 0.25f, 8f), 50f, false);
            CreateEnemy("Enemy_3", new Vector3(8f, 0.25f, -3f), 50f, false);

            // Armored enemy
            CreateEnemy("Enemy_Armored", new Vector3(-8f, 0.25f, 3f), 120f, true);
        }

        private static void CreateEnemy(string name, Vector3 position, float health, bool armored)
        {
            var enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = name;
            enemy.transform.position = position;

            // Remove default collider — we'll add our own
            Object.DestroyImmediate(enemy.GetComponent<CapsuleCollider>());

            // Body collider
            var bodyCol = enemy.AddComponent<CapsuleCollider>();
            bodyCol.height = 1.8f;
            bodyCol.radius = 0.4f;
            bodyCol.center = new Vector3(0f, 0.9f, 0f);
            var bodyZone = enemy.AddComponent<HitZone>();
            // Body zone is default

            // Head zone (child object)
            var head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "Head";
            head.transform.SetParent(enemy.transform);
            head.transform.localPosition = new Vector3(0f, 2f, 0f);
            head.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            var headZone = head.AddComponent<HitZone>();
            SetHitZoneToHead(headZone);

            // Rigidbody
            var rb = enemy.AddComponent<Rigidbody>();
            rb.mass = armored ? 100f : 60f;
            rb.freezeRotation = true;

            // Damageable
            var damageable = enemy.AddComponent<Damageable>();
            SetDamageableHealth(damageable, health);

            // Noise listener
            enemy.AddComponent<NoiseListener>();

            // Enemy controller
            var controller = enemy.AddComponent<EnemyController>();
            SetEnemyArmored(controller, armored);

            // Color based on type
            var renderer = enemy.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = armored
                    ? new Color(0.5f, 0.5f, 0.6f) // Grey-blue for armored
                    : new Color(0.8f, 0.2f, 0.2f); // Red for normal
            }
        }

        private static void BuildPhysicsObjects()
        {
            // Crates
            CreatePhysicsBox("Crate_1", new Vector3(3f, 0.75f, -5f), new Vector3(1f, 1f, 1f), 10f, new Color(0.6f, 0.4f, 0.2f));
            CreatePhysicsBox("Crate_2", new Vector3(-3f, 0.75f, -3f), new Vector3(1f, 1f, 1f), 10f, new Color(0.6f, 0.4f, 0.2f));

            // Barrels
            CreatePhysicsBarrel("Barrel_1", new Vector3(7f, 0.75f, 2f), 15f);
            CreatePhysicsBarrel("Barrel_2", new Vector3(-7f, 0.75f, -2f), 15f);

            // Bottles (small, light)
            CreatePhysicsBox("Bottle_1", new Vector3(2f, 0.5f, 3f), new Vector3(0.15f, 0.4f, 0.15f), 0.5f, new Color(0.2f, 0.6f, 0.2f));
            CreatePhysicsBox("Bottle_2", new Vector3(-1f, 0.5f, 6f), new Vector3(0.15f, 0.4f, 0.15f), 0.5f, new Color(0.2f, 0.6f, 0.2f));

            // Torch (long thin object)
            CreatePhysicsBox("Torch", new Vector3(0f, 0.5f, -7f), new Vector3(0.1f, 1f, 0.1f), 1f, new Color(0.4f, 0.3f, 0.1f));
        }

        private static void CreatePhysicsBox(string name, Vector3 position, Vector3 scale, float mass, Color color)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = name;
            obj.transform.position = position;
            obj.transform.localScale = scale;

            var rb = obj.AddComponent<Rigidbody>();
            rb.mass = mass;

            obj.AddComponent<ThrowableObject>();
            obj.AddComponent<PickupObject>();

            var renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = color;
        }

        private static void CreatePhysicsBarrel(string name, Vector3 position, float mass)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            obj.name = name;
            obj.transform.position = position;
            obj.transform.localScale = new Vector3(0.8f, 1f, 0.8f);

            var rb = obj.AddComponent<Rigidbody>();
            rb.mass = mass;

            obj.AddComponent<ThrowableObject>();
            obj.AddComponent<PickupObject>();

            var renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = new Color(0.5f, 0.3f, 0.1f);
        }

        private static void BuildHazards()
        {
            // Spikes
            var spikes = GameObject.CreatePrimitive(PrimitiveType.Cube);
            spikes.name = "SpikeTrap";
            spikes.transform.position = new Vector3(12f, 0.4f, 8f);
            spikes.transform.localScale = new Vector3(4f, 0.3f, 4f);
            var spikeCollider = spikes.GetComponent<BoxCollider>();
            spikeCollider.isTrigger = true;
            spikes.AddComponent<SpikeTrap>();
            var spikeRenderer = spikes.GetComponent<Renderer>();
            if (spikeRenderer != null)
                spikeRenderer.material.color = new Color(0.3f, 0.3f, 0.3f);

            // Fire area
            var fire = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fire.name = "FireHazard";
            fire.transform.position = new Vector3(-12f, 0.4f, 8f);
            fire.transform.localScale = new Vector3(4f, 0.3f, 4f);
            var fireCollider = fire.GetComponent<BoxCollider>();
            fireCollider.isTrigger = true;
            fire.AddComponent<FireHazard>();
            var fireRenderer = fire.GetComponent<Renderer>();
            if (fireRenderer != null)
                fireRenderer.material.color = new Color(1f, 0.3f, 0f);

            // Pit (below floor level)
            var pitHole = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pitHole.name = "PitHazard";
            pitHole.transform.position = new Vector3(0f, -3f, 15f);
            pitHole.transform.localScale = new Vector3(5f, 1f, 5f);
            var pitCollider = pitHole.GetComponent<BoxCollider>();
            pitCollider.isTrigger = true;
            pitHole.AddComponent<PitHazard>();
            var pitRenderer = pitHole.GetComponent<Renderer>();
            if (pitRenderer != null)
                pitRenderer.material.color = Color.black;

            // Remove floor section above pit
            var pitFloorCover = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pitFloorCover.name = "PitOpening";
            pitFloorCover.transform.position = new Vector3(0f, 0.25f, 15f);
            pitFloorCover.transform.localScale = new Vector3(5f, 0.01f, 5f);
            Object.DestroyImmediate(pitFloorCover.GetComponent<BoxCollider>());
            var pitOpeningRenderer = pitFloorCover.GetComponent<Renderer>();
            if (pitOpeningRenderer != null)
                pitOpeningRenderer.material.color = new Color(0.1f, 0.1f, 0.1f);
        }

        private static void BuildInteractables()
        {
            // Door
            var doorFrame = new GameObject("DoorFrame");
            doorFrame.transform.position = new Vector3(10f, 0f, 0f);

            var door = GameObject.CreatePrimitive(PrimitiveType.Cube);
            door.name = "Door";
            door.transform.SetParent(doorFrame.transform);
            door.transform.localPosition = new Vector3(0f, 1.5f, 0f);
            door.transform.localScale = new Vector3(2f, 3f, 0.2f);
            door.AddComponent<Door>();
            var doorRenderer = door.GetComponent<Renderer>();
            if (doorRenderer != null)
                doorRenderer.material.color = new Color(0.5f, 0.3f, 0.15f);

            // Lever
            var leverBase = new GameObject("LeverBase");
            leverBase.transform.position = new Vector3(-10f, 1f, 5f);

            var lever = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lever.name = "Lever";
            lever.transform.SetParent(leverBase.transform);
            lever.transform.localPosition = Vector3.zero;
            lever.transform.localScale = new Vector3(0.1f, 0.6f, 0.1f);
            lever.AddComponent<Lever>();
            var leverRenderer = lever.GetComponent<Renderer>();
            if (leverRenderer != null)
                leverRenderer.material.color = new Color(0.4f, 0.4f, 0.4f);
        }

        private static void BuildLighting()
        {
            // Directional light should already exist from NewScene default.
            // Add ambient light boost.
            RenderSettings.ambientLight = new Color(0.4f, 0.4f, 0.45f);
        }

        // Helper methods to set serialized fields via SerializedObject
        private static void SetHitZoneToHead(HitZone zone)
        {
            var so = new SerializedObject(zone);
            var prop = so.FindProperty("_zone");
            if (prop != null)
            {
                prop.enumValueIndex = (int)HitZone.ZoneType.Head;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void SetDamageableHealth(Damageable damageable, float health)
        {
            var so = new SerializedObject(damageable);
            var prop = so.FindProperty("_maxHealth");
            if (prop != null)
            {
                prop.floatValue = health;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void SetEnemyArmored(EnemyController controller, bool armored)
        {
            var so = new SerializedObject(controller);
            var prop = so.FindProperty("_isArmored");
            if (prop != null)
            {
                prop.boolValue = armored;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
        }
    }
}
