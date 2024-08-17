using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class RotationMachine : BaseMachine
    {
        public TileBase TurnLeftTile;
        public TileBase TurnRightTile;

        public override MachineType MachineType => MachineType.RotationMachine;

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

        public override TileBase Tile => RotateRight ? TurnRightTile : TurnLeftTile;

        public override void Next()
        {
            RotateRight = !RotateRight;
        }
    }
}
