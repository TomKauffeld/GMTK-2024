using Assets.Scripts.Levels;
using Assets.Scripts.Machines;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.World
{
    [RequireComponent(typeof(LevelManager))]
    internal class WorldLogic : BaseMonoBehaviour
    {
        public WorldRenderer Renderer;

        public Transform MachinesTransform;
        public Belt BeltMachinePrototype;
        public RotationMachine RotationMachinePrototype;
        public SizeChangerMachine SizeChangerMachinePrototype;
        public FlipMachine FlipMachinePrototype;
        public Wall WallPrototype;
        public Target TargetPrototype;
        public Spawner SpawnerPrototype;
        public Floor FloorPrototype;



        private LevelManager _levelManager;
        private BaseLevel _currentLevel;

        private readonly Dictionary<Vector2Int, BaseMachine> _machines = new();

        //TODO: Show user placement failed
        public event Action<Vector2Int, MachineType> MachinePlacementFailed;

        private void Start()
        {
            _levelManager = GetComponent<LevelManager>();
            _levelManager.OnLevelLoad += OnLevelLoad;
            Renderer.OnReady += RendererOnReady;
            if (Renderer.IsReady)
                RendererOnReady();
        }


        private void RendererOnReady()
        {
            Renderer.OnReady -= RendererOnReady;
            _levelManager.LoadLevel(1);
        }

        private void ClearAllMachines()
        {
            foreach (BaseMachine machine in MachinesTransform.GetComponentsInChildren<BaseMachine>())
            {
                Renderer.SetEmpty(machine.Position);
                CountAddMachine(machine.MachineType, 1);
                DestroyMachine(machine);
            }
        }

        private bool CountAddMachine(MachineType machineType, int amount, bool force = false)
        {
            if (_currentLevel == null)
                return false;

            switch (machineType)
            {
                case MachineType.Belt when _currentLevel.RemainingBelts + amount >= 0 || force || _currentLevel.RemainingBelts < 0:
                    if (_currentLevel.RemainingBelts >= 0)
                        _currentLevel.RemainingBelts = Math.Max(0, _currentLevel.RemainingBelts + amount);
                    LaunchNotification($"inventory:update:{machineType}");
                    LaunchNotification($"inventory:update:{machineType}:{_currentLevel.RemainingBelts}");
                    return true;

                case MachineType.RotationMachine when _currentLevel.RemainingRotations + amount >= 0 || force || _currentLevel.RemainingRotations < 0:
                    if (_currentLevel.RemainingRotations >= 0)
                        _currentLevel.RemainingRotations = Math.Max(0, _currentLevel.RemainingRotations + amount);
                    LaunchNotification($"inventory:update:{machineType}");
                    LaunchNotification($"inventory:update:{machineType}:{_currentLevel.RemainingRotations}");
                    return true;

                case MachineType.SizeChangerMachine when _currentLevel.RemainingSize + amount >= 0 || force || _currentLevel.RemainingSize < 0:
                    if (_currentLevel.RemainingSize >= 0)
                        _currentLevel.RemainingSize = Math.Max(0, _currentLevel.RemainingSize + amount);
                    LaunchNotification($"inventory:update:{machineType}");
                    LaunchNotification($"inventory:update:{machineType}:{_currentLevel.RemainingSize}");
                    return true;

                case MachineType.FlipMachine when _currentLevel.RemainingFlips + amount >= 0 || force || _currentLevel.RemainingFlips < 0:
                    if (_currentLevel.RemainingFlips >= 0)
                        _currentLevel.RemainingFlips = Math.Max(0, _currentLevel.RemainingFlips + amount);
                    LaunchNotification($"inventory:update:{machineType}");
                    LaunchNotification($"inventory:update:{machineType}:{_currentLevel.RemainingFlips}");
                    return true;

                case MachineType.None:
                    return true;

                default:
                    return false;
            }
        }

        private void OnLevelLoad(BaseLevel level)
        {
            ClearAllMachines();
            _currentLevel = level;
            for (int x = 0; x < level.Size.x; ++x)
            {
                for (int y = 0; y < level.Size.y; ++y)
                {
                    SetMachine(new Vector2Int(x, y), MachineType.None);
                }
            }

            Renderer.RenderSize = level.Size;


            LaunchNotification($"inventory:update:{MachineType.None}");
            LaunchNotification($"inventory:update:{MachineType.None}:{-1}");


            LaunchNotification($"inventory:update:{MachineType.Belt}");
            LaunchNotification($"inventory:update:{MachineType.Belt}:{_currentLevel.RemainingBelts}");

            LaunchNotification($"inventory:update:{MachineType.RotationMachine}");
            LaunchNotification($"inventory:update:{MachineType.RotationMachine}:{_currentLevel.RemainingRotations}");

            LaunchNotification($"inventory:update:{MachineType.SizeChangerMachine}");
            LaunchNotification($"inventory:update:{MachineType.SizeChangerMachine}:{_currentLevel.RemainingSize}");

            LaunchNotification($"inventory:update:{MachineType.FlipMachine}");
            LaunchNotification($"inventory:update:{MachineType.FlipMachine}:{_currentLevel.RemainingFlips}");


            LaunchNotification($"switch:machine:{MachineType.None}");
        }

        private void Update()
        {

        }


        private void OnMachineTileChanged([NotNull] BaseMachine machine)
        {
            Renderer.SetMachine(machine);
        }

        public BaseMachine GetMachine(Vector2Int position)
        {
            return _machines.GetValueOrDefault(position);
        }

        private BaseMachine CreateMachine(Vector2Int position, MachineType machineType)
        {
            BaseMachine prototype = GetPrototype(machineType);
            BaseMachine machine = Instantiate(prototype, MachinesTransform);

            machine.transform.localPosition = new Vector3(position.x, position.y);
            machine.TileChanged += OnMachineTileChanged;

            _machines[position] = machine;

            LaunchNotification($"machine:create:{machineType}");

            return machine;
        }

        private void DestroyMachine([NotNull] BaseMachine machine)
        {
            LaunchNotification($"machine:destroy:{machine.MachineType}");

            _machines.Remove(machine.Position);
            machine.TileChanged -= OnMachineTileChanged;

            Destroy(machine.gameObject);
        }

        private BaseMachine GetPrototype(MachineType machineType)
        {
            return machineType switch
            {
                MachineType.Belt => BeltMachinePrototype,
                MachineType.RotationMachine => RotationMachinePrototype,
                MachineType.SizeChangerMachine => SizeChangerMachinePrototype,
                MachineType.FlipMachine => FlipMachinePrototype,
                MachineType.Spawner => SpawnerPrototype,
                MachineType.Target => TargetPrototype,
                MachineType.Wall => WallPrototype,
                MachineType.None => FloorPrototype,
                _ => throw new ArgumentOutOfRangeException(nameof(machineType))
            };
        }


        public void SetMachine(Vector2Int position, MachineType machineType)
        {
            if (position.x < 0 || position.y < 0 || _currentLevel == null
                || position.x >= _currentLevel.Size.x || position.y >= _currentLevel.Size.y)
            {
                MachinePlacementFailed?.Invoke(position, machineType);
                LaunchNotification($"failed:machine:{machineType}:{position.x}:{position.y}");
                return;
            }


            BaseMachine oldMachine = GetMachine(position);

            if (oldMachine != null)
                CountAddMachine(oldMachine.MachineType, 1);

            if (CountAddMachine(machineType, -1))
            {
                if (oldMachine != null)
                    DestroyMachine(oldMachine);

                Renderer.SetMachine(CreateMachine(position, machineType));
            }
            else
            {
                if (oldMachine != null)
                    CountAddMachine(oldMachine.MachineType, -1, true);
                LaunchNotification($"failed:machine:{machineType}:{position.x}:{position.y}");
                MachinePlacementFailed?.Invoke(position, machineType);
            }
        }

        public Vector2Int? WorldToCell(Vector3 position)
        {
            Vector2Int pos = Renderer.WorldToCell(position);

            if (pos.x < 0 || pos.y < 0 || _currentLevel == null || pos.x >= _currentLevel.Size.x || pos.y >= _currentLevel.Size.y)
                return null;
            return pos;
        }
    }
}
