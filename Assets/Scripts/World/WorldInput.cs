using Assets.Scripts.Machines;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.World
{
    [RequireComponent(typeof(WorldLogic))]
    public class WorldInput : BaseMonoBehaviour
    {
        public MachineType CurrentMachineType = MachineType.None;

        private WorldLogic _logic;


        private void Start()
        {
            Register("switch:machine:*", OnMachineSwitch);
            _logic = GetComponent<WorldLogic>();
        }

        private Vector2Int? WorldToCell(Vector3 position)
        {
            return _logic.WorldToCell(position);
        }

        private void OnMachineSwitch(string notification)
        {
            string name = notification.Split(':').Last();
            if (Enum.TryParse(name, out MachineType machineType))
            {
                CurrentMachineType = machineType;
            }
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.Left))
            {
                Vector2Int? position = WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                if (position.HasValue)
                {
                    BaseMachine existingMachine = _logic.GetMachine(position.Value);

                    if (existingMachine == null || existingMachine.Placeable)
                    {
                        if (existingMachine != null
                            && existingMachine.MachineType == CurrentMachineType
                            && CurrentMachineType != MachineType.None)
                        {
                            LaunchNotification(
                                $"next:machine:{CurrentMachineType}:{position.Value.x}:{position.Value.y}");
                            existingMachine.Next();
                        }
                        else
                        {
                            LaunchNotification(
                                $"place:machine:{CurrentMachineType}:{position.Value.x}:{position.Value.y}");
                            _logic.SetMachine(position.Value, CurrentMachineType);
                        }
                    }
                }
            }
        }
    }
}
