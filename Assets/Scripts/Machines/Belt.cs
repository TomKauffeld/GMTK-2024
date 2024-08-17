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

        public override void Next()
        {
            Direction = Direction switch
            {
                DirectionEnum.Up => DirectionEnum.Right,
                DirectionEnum.Right => DirectionEnum.Down,
                DirectionEnum.Down => DirectionEnum.Left,
                DirectionEnum.Left => DirectionEnum.Up,
                _ => DirectionEnum.Up
            };
        }

        public override MachineEnum MachineType => MachineEnum.Belt;
    }
}
