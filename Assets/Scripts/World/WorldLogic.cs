using Assets.Scripts.Levels;
using Assets.Scripts.Machines;
using JetBrains.Annotations;
using System;
using System.Linq;
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



        private LevelManager _levelManager;
        private BaseLevel _currentLevel;

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
                Renderer.ClearMachine(machine.Position);
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
                case MachineEnum.BeltDown when _currentLevel.RemainingBelts + amount >= 0 || force:
                case MachineEnum.BeltLeft when _currentLevel.RemainingBelts + amount >= 0 || force:
                case MachineEnum.BeltRight when _currentLevel.RemainingBelts + amount >= 0 || force:
                case MachineEnum.BeltUp when _currentLevel.RemainingBelts + amount >= 0 || force:
                    _currentLevel.RemainingBelts = Math.Max(0, _currentLevel.RemainingBelts + amount);
                    return true;

                case MachineEnum.RotationMachine when _currentLevel.RemainingRotations + amount >= 0 || force:
                    _currentLevel.RemainingRotations = Math.Max(0, _currentLevel.RemainingRotations + amount);
                    return true;

                case MachineEnum.SizeChangerMachine when _currentLevel.RemainingSize + amount >= 0 || force:
                    _currentLevel.RemainingSize = Math.Max(0, _currentLevel.RemainingSize + amount);
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
            return
                MachinesTransform
                    .GetComponentsInChildren<BaseMachine>()
                    .FirstOrDefault(machine => machine.Position.Equals(position));
        }

        private BaseMachine CreateMachine(Vector2Int position, MachineEnum machineType)
        {
            BaseMachine prototype = GetPrototype(machineType);
            BaseMachine machine = Instantiate(prototype, MachinesTransform);

            machine.transform.localPosition = new Vector3(position.x, position.y);
            machine.TileChanged += OnMachineTileChanged;
            machine.MachineType = machineType;

            return machine;
        }

        private void DestroyMachine([NotNull] BaseMachine machine)
        {
            machine.TileChanged -= OnMachineTileChanged;
            Destroy(machine.gameObject);
        }

        private BaseMachine GetPrototype(MachineEnum machineType)
        {
            return machineType switch
            {
                MachineEnum.BeltDown => BeltMachine,
                MachineEnum.BeltLeft => BeltMachine,
                MachineEnum.BeltRight => BeltMachine,
                MachineEnum.BeltUp => BeltMachine,
                MachineEnum.RotationMachine => RotationMachine,
                MachineEnum.SizeChangerMachine => SizeChangerMachine,
                _ => throw new ArgumentOutOfRangeException(nameof(machineType))
            };
        }


        public void SetMachine(Vector2Int position, MachineEnum machineType)
        {
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

                Renderer.SetEmpty(position);
            }
        }

        public Vector2Int WorldToCell(Vector3 position)
        {
            return Renderer.WorldToCell(position);
        }
    }
}
