using Assets.Scripts.Machines;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.World
{
    [RequireComponent(typeof(WorldLogic))]
    public class WorldInput : MonoBehaviour
    {
        public MachineEnum CurrentMachineType = MachineEnum.None;

        private WorldLogic _logic;


        private void Start()
        {
            _logic = GetComponent<WorldLogic>();
        }

        private Vector2Int WorldToCell(Vector3 position)
        {
            return _logic.WorldToCell(position);
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.Left))
            {
                Vector2Int position = WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));


                BaseMachine existingMachine = _logic.GetMachine(position);

                if (existingMachine != null && existingMachine.MachineType == CurrentMachineType)
                    _logic.SetMachine(position, MachineEnum.None);
                else
                    _logic.SetMachine(position, CurrentMachineType);

            }
        }
    }
}
