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
        public Belt BeltMachine;
        public RotationMachine RotationMachine;
        public SizeChangerMachine SizeChangerMachine;
        public FlipMachine FlipMachine;
        public Wall Wall;
        public Target Target;
        public Spawner Spawner;
        public EmptyMachine EmptyMachine;



        private LevelManager _levelManager;
        private BaseLevel _currentLevel;

        private readonly Dictionary<Vector2Int, BaseMachine> _machines = new();

        //TODO: Show user placement failed
        public event Action<Vector2Int, MachineEnum> MachinePlacementFailed;

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

        private bool CountAddMachine(MachineEnum machineType, int amount, bool force = false)
        {
            if (_currentLevel == null)
                return false;

            switch (machineType)
            {
                case MachineEnum.Belt when _currentLevel.RemainingBelts + amount >= 0 || force || _currentLevel.RemainingBelts < 0:
                    if (_currentLevel.RemainingBelts > 0)
                        _currentLevel.RemainingBelts = Math.Max(0, _currentLevel.RemainingBelts + amount);
                    LaunchNotification($"inventory:update:{machineType}");
                    LaunchNotification($"inventory:update:{machineType}:{_currentLevel.RemainingBelts}");
                    return true;

                case MachineEnum.RotationMachine when _currentLevel.RemainingRotations + amount >= 0 || force || _currentLevel.RemainingRotations < 0:
                    if (_currentLevel.RemainingRotations > 0)
                        _currentLevel.RemainingRotations = Math.Max(0, _currentLevel.RemainingRotations + amount);
                    LaunchNotification($"inventory:update:{machineType}");
                    LaunchNotification($"inventory:update:{machineType}:{_currentLevel.RemainingRotations}");
                    return true;

                case MachineEnum.SizeChangerMachine when _currentLevel.RemainingSize + amount >= 0 || force || _currentLevel.RemainingSize < 0:
                    if (_currentLevel.RemainingSize > 0)
                        _currentLevel.RemainingSize = Math.Max(0, _currentLevel.RemainingSize + amount);
                    LaunchNotification($"inventory:update:{machineType}");
                    LaunchNotification($"inventory:update:{machineType}:{_currentLevel.RemainingSize}");
                    return true;

                case MachineEnum.FlipMachine when _currentLevel.RemainingFlips + amount >= 0 || force || _currentLevel.RemainingFlips < 0:
                    if (_currentLevel.RemainingFlips > 0)
                        _currentLevel.RemainingFlips = Math.Max(0, _currentLevel.RemainingFlips + amount);
                    LaunchNotification($"inventory:update:{machineType}");
                    LaunchNotification($"inventory:update:{machineType}:{_currentLevel.RemainingFlips}");
                    return true;

                case MachineEnum.None:
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
                    SetMachine(new Vector2Int(x, y), MachineEnum.None);
                }
            }

            Renderer.RenderSize = level.Size;


            LaunchNotification($"inventory:update:{MachineEnum.None}");
            LaunchNotification($"inventory:update:{MachineEnum.None}:{-1}");


            LaunchNotification($"inventory:update:{MachineEnum.Belt}");
            LaunchNotification($"inventory:update:{MachineEnum.Belt}:{_currentLevel.RemainingBelts}");

            LaunchNotification($"inventory:update:{MachineEnum.RotationMachine}");
            LaunchNotification($"inventory:update:{MachineEnum.RotationMachine}:{_currentLevel.RemainingRotations}");

            LaunchNotification($"inventory:update:{MachineEnum.SizeChangerMachine}");
            LaunchNotification($"inventory:update:{MachineEnum.SizeChangerMachine}:{_currentLevel.RemainingSize}");

            LaunchNotification($"inventory:update:{MachineEnum.FlipMachine}");
            LaunchNotification($"inventory:update:{MachineEnum.FlipMachine}:{_currentLevel.RemainingFlips}");
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

        private BaseMachine CreateMachine(Vector2Int position, MachineEnum machineType)
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

        private BaseMachine GetPrototype(MachineEnum machineType)
        {
            return machineType switch
            {
                MachineEnum.Belt => BeltMachine,
                MachineEnum.RotationMachine => RotationMachine,
                MachineEnum.SizeChangerMachine => SizeChangerMachine,
                MachineEnum.FlipMachine => FlipMachine,
                MachineEnum.Spawner => Spawner,
                MachineEnum.Target => Target,
                MachineEnum.Wall => Wall,
                MachineEnum.None => EmptyMachine,
                _ => throw new ArgumentOutOfRangeException(nameof(machineType))
            };
        }


        public void SetMachine(Vector2Int position, MachineEnum machineType)
        {
            if (position.x < 0 || position.y < 0 || _currentLevel == null
                || position.x >= _currentLevel.Size.x || position.y >= _currentLevel.Size.y)
            {
                MachinePlacementFailed?.Invoke(position, machineType);
                LaunchNotification($"failed:machine:{machineType}:{position.x}:{position.y}");
                return;
            }


            BaseMachine oldMachine = GetMachine(position);

            if (machineType != MachineEnum.None)
            {
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
            else
            {
                if (oldMachine != null)
                {
                    CountAddMachine(oldMachine.MachineType, 1);
                    DestroyMachine(oldMachine);
                }

                Renderer.SetMachine(CreateMachine(position, machineType));
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
