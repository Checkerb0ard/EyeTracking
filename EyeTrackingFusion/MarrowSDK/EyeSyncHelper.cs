using System.Diagnostics.CodeAnalysis;
using LabFusion.Marrow.Integration;

#if !MELONLOADER
using System.Collections.Generic;
#endif

#if MELONLOADER
using EyeTracking.Fusion;
using Il2CppSLZ.Marrow.Pool;
using Il2CppSLZ.Marrow;
using MelonLoader;
using Guid = Il2CppSystem.Guid;
using LabFusion.Entities;
using LabFusion.Network;
using LabFusion.Utilities;
#endif

using
    #if MELONLOADER    
        Il2CppSLZ.
    #else
        SLZ.
    #endif
        Marrow.Warehouse;

using UnityEngine;

namespace EyeTracking.MarrowSDK
{
#if MELONLOADER
    [RegisterTypeInIl2Cpp]
#endif
    public class EyeSyncHelper : MonoBehaviour
    {
#if MELONLOADER
        public EyeSyncHelper(IntPtr intPtr) : base(intPtr) { }

        private bool _belongsToLocalRig;
        private string _guid = null!;
        private AvatarReferences? _avatarReferences;
        private SpawnableReferences? _spawnableReferences;
        private static readonly Dictionary<Transform, string> SpawnableGuidMap = new();
        
        private RigManager? _rigManagerCached;
        private AnimationRig? _animationRig;

        private bool IsDestroyed => this == null;
        
        private class AvatarReferences(RPCString guid, CrateSpawner crateSpawner, RPCEvent requestCrate)
        {
            public readonly RPCString Guid = guid;
            public readonly CrateSpawner CrateSpawner = crateSpawner;
            public readonly RPCEvent RequestCrate = requestCrate;
        }
        
        private class SpawnableReferences(Transform root, Poolee poolee)
        {
            public readonly Transform Root = root;
            public readonly Poolee Poolee = poolee;
        }
#endif
        
        public void Awake()
        {
#if MELONLOADER
            _guid = Guid.NewGuid().ToString();
#endif
        }

        public void Update()
        {
#if MELONLOADER
            if (_spawnableReferences == null)
                return;
            
            if (_animationRig)
                _spawnableReferences.Root.transform.position =
                    new Vector3(_animationRig!.eyeGaze.x, _animationRig.eyeGaze.y, 0);
#endif
        }

        private void OnSpawnEvent(CrateSpawner cs, GameObject go)
        {
#if MELONLOADER
            if (_spawnableReferences != null)
            {
                // Cleanup to prevent accumulating GUIDs
                SpawnableGuidMap.Remove(_spawnableReferences.Root);
            }

            _spawnableReferences = new SpawnableReferences(root: go.transform, poolee: Poolee.Cache.Get(go));
            SpawnableGuidMap.Add(go.transform, _guid);
            MelonLogger.Msg("OnSpawnEvent for RB fired - GUID mapped");

            if (Poolee.Cache.TryGet(go, out var poolee))
            {
                poolee.OnDespawnDelegate += (Action<GameObject>)(_ =>
                {
                    MelonLogger.Msg("Eye tracking syncer was despawned - spawning another");

                    if (!IsDestroyed)
                        SpawnRigidbody();
                });
            }
#endif
        }
        
        public void OnEnable()
        {
#if MELONLOADER
            if (!NetworkInfo.HasServer)
                return;
            
            if (_avatarReferences == null)
                return;

            _rigManagerCached = GetComponentInParent<RigManager>();
            _animationRig = _rigManagerCached.animationRig;
            
            if (BoneLib.Player.RigManager != _rigManagerCached)
            {
                MelonLogger.Msg($"Eye sync stopped from running on rig {_rigManagerCached.gameObject.name} as it's not ours.");
                _belongsToLocalRig = false;
                return;
            }

            _belongsToLocalRig = true;
            UpdateAvatarGuid();
            SpawnRigidbody();
#endif
        }

        private void UpdateAvatarGuid()
        {
            _avatarReferences!.Guid.SetValue(_guid);
            _avatarReferences.Guid.ReceiveValue(_guid);
        }

        private void SpawnRigidbody()
        {
#if MELONLOADER
            UpdateAvatarGuid();
            ContentDownloader.PreloadSyncer(() => _avatarReferences!.RequestCrate.Invoke());
#endif
        }

        public void OnDisable()
        {
#if MELONLOADER
            _rigManagerCached = null;

            // Despawn the rigidbody to prevent junk from building up
            if (_spawnableReferences == null)
                return;
            
            if (PooleeExtender.Cache.TryGet(_spawnableReferences.Poolee, out var networkEntity))
                PooleeUtilities.RequestDespawn(networkEntity.ID, false);
            else
                MelonLogger.Warning($"Failed to despawn eye sync (Rig: {_rigManagerCached?.gameObject.name})");
#endif
        }
        
        // Called from logic underneath the spawnable
        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public void Spawnable_OnEnable(Transform spawnableRoot, GameObject guidRPCStringGo, GameObject ownershipEventsGo)
        {
#if MELONLOADER
            MelonLogger.Msg("Spawnable_OnEnable called from UltEvents - apply guid");
            
            var guidRPCString = guidRPCStringGo.GetComponent<RPCString>();
            var ownershipEvents = ownershipEventsGo.GetComponent<OwnershipEvents>();
            ApplyGuid();

            void ApplyGuid()
            {
                var key = SpawnableGuidMap.Keys.FirstOrDefault(x => x == spawnableRoot);
                
                if (key == null)
                {
                    MelonLogger.Warning("No guid found for spawnable");
                    return;
                }

                var value = SpawnableGuidMap[key];
                guidRPCString.SetValue(value);
                guidRPCString.ReceiveValue(value);
                MelonLogger.Msg("Guid applied");
            }
            
            ownershipEvents.TakeOwnership();
#endif
        }

        public void SetAvatarReferences(GameObject guidRPCString, CrateSpawner crateSpawner, GameObject requestCrate)
        {
#if MELONLOADER
            MelonLogger.Msg("Setting avi references");
            
            _avatarReferences = new AvatarReferences(guid: guidRPCString.GetComponent<RPCString>(),
                crateSpawner: crateSpawner, requestCrate: requestCrate.GetComponent<RPCEvent>());

            _avatarReferences.CrateSpawner.onSpawnEvent.add_DynamicCalls((Action<CrateSpawner, GameObject>)OnSpawnEvent);
#endif
        }

        public void OnDestroy()
        {
#if MELONLOADER
            // Only clean the map if we belong to a Rig Manager, to avoid edge cases when our avatar is cloned
            // unconventionally.
            // (e.g: mirrors, portals, cinematic tools)
            if (_belongsToLocalRig)
                SpawnableGuidMap.Clear();            
#endif
        }
    }
}