using System;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    internal class Belt : BaseMachine
    {
        public TileBase BeltUp;
        public TileBase BeltDown;
        public TileBase BeltLeft;
        public TileBase BeltRight;

        private DirectionEnum _direction;

        public DirectionEnum Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                InvokeTileChanged();
            }
        }

        public override TileBase Tile
        {
            get
            {
                return Direction switch
                {
                    DirectionEnum.Down => BeltDown,
                    DirectionEnum.Left => BeltLeft,
                    DirectionEnum.Right => BeltRight,
                    DirectionEnum.Up => BeltUp,
                    _ => throw new ArgumentOutOfRangeException(nameof(Direction))
                };
            }
        }

        public override MachineEnum MachineType
        {
            get
            {
                return Direction switch
                {
                    DirectionEnum.Down => MachineEnum.BeltDown,
                    DirectionEnum.Left => MachineEnum.BeltLeft,
                    DirectionEnum.Right => MachineEnum.BeltRight,
                    DirectionEnum.Up => MachineEnum.BeltUp,
                    _ => throw new ArgumentOutOfRangeException(nameof(Direction))
                };
            }
            set
            {
                Direction = value switch
                {
                    MachineEnum.BeltDown => DirectionEnum.Down,
                    MachineEnum.BeltLeft => DirectionEnum.Left,
                    MachineEnum.BeltRight => DirectionEnum.Right,
                    MachineEnum.BeltUp => DirectionEnum.Up,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
}
