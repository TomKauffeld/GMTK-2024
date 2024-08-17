using System;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    internal class Belt : BaseMachine
    {
        public TileBase BeltUpTile;
        public TileBase BeltDownTile;
        public TileBase BeltLeftTile;
        public TileBase BeltRightTile;

        private Direction _direction;

        public Direction Direction
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
                    Direction.Down => BeltDownTile,
                    Direction.Left => BeltLeftTile,
                    Direction.Right => BeltRightTile,
                    Direction.Up => BeltUpTile,
                    _ => throw new ArgumentOutOfRangeException(nameof(Direction))
                };
            }
        }

        public override void Next()
        {
            Direction = Direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => Direction.Up
            };
        }

        public override MachineType MachineType => MachineType.Belt;
    }
}
