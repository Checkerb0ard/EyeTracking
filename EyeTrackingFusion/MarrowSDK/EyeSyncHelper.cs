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
    public class EyeSyncHelper : MonoBehaviour
    {
#if MELONLOADER
        private string _guid = null!;
        private AvatarReferences? _avatarReferences;
        private SpawnableReferences? _spawnableReferences;
        private static readonly Dictionary<Transform, string> SpawnableGuidMap = new();
        
        private RigManager? _rigManagerCached;
        private AnimationRig? _animationRig;

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
            _guid = new Guid().ToString();
#endif
        }

        public void Start()
        {
#if MELONLOADER
            _avatarReferences?.CrateSpawner.onSpawnEvent.add_DynamicCalls((Action<CrateSpawner, GameObject>)OnSpawnEvent);
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
#endif
        }
        
        public void OnEnable()
        {
#if MELONLOADER
            if (_avatarReferences == null)
                return;

            _rigManagerCached = GetComponentInParent<RigManager>();
            
            if (BoneLib.Player.RigManager != _rigManagerCached)
            {
                MelonLogger.Msg($"Eye sync stopped from running on rig {_rigManagerCached.gameObject.name} as it's not ours.");
                return;
            }

            _animationRig = _rigManagerCached.animationRig;
            
            _avatarReferences.Guid.SetValue(_guid);
            _avatarReferences.Guid.ReceiveValue(_guid);
            ContentDownloader.PreloadSyncer(() => _avatarReferences.RequestCrate.Invoke());
#endif
        }

        public void OnDisable()
        {
#if MELONLOADER
            _rigManagerCached = null;

            if (_spawnableReferences == null) return;
            
            if (PooleeExtender.Cache.TryGet(_spawnableReferences.Poolee, out var networkEntity))
                PooleeUtilities.RequestDespawn(networkEntity.ID, false);
            else
                MelonLogger.Warning($"Failed to despawn eye sync (Rig: {_rigManagerCached?.gameObject.name})");
#endif
        }
        
        // Called from logic underneath the spawnable
        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public void Spawnable_OnEnable(Transform spawnableRoot, RPCString guidRPCString, OwnershipEvents ownershipEvents)
        {
#if MELONLOADER
            ApplyGuid();

            void ApplyGuid()
            {
                if (!SpawnableGuidMap.TryGetValue(spawnableRoot, out var value)) return;
                guidRPCString.SetValue(value);
                guidRPCString.ReceiveValue(value);
            }
            
            ownershipEvents.TakeOwnership();
#endif
        }

        public void SetAvatarReferences(RPCString guidRPCString, CrateSpawner crateSpawner, RPCEvent requestCrate)
        {
#if MELONLOADER
            _avatarReferences = new AvatarReferences(guid: guidRPCString, crateSpawner: crateSpawner, requestCrate: requestCrate);
#endif
        }
    }
}