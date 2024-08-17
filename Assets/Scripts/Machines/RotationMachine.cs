using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    internal class RotationMachine : BaseMachine
    {
        public TileBase[] TurnLeft;
        public TileBase[] TurnRight;

        public override MachineEnum MachineType => MachineEnum.RotationMachine;

        private bool _rotateRight = true;

        public bool RotateRight
        {
            get => _rotateRight;
            set
            {
                _rotateRight = value;
                InvokeTileChanged();
            }
        }

        public override TileBase[] Tile => RotateRight ? TurnRight : TurnLeft;

        public override void Next()
        {
            RotateRight = !RotateRight;
        }
    }
}
