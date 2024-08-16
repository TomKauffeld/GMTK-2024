using System;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    internal class RotationMachine : BaseMachine
    {
        public TileBase TurnLeft;
        public TileBase TurnRight;

        public override MachineEnum MachineType
        {
            get => MachineEnum.RotationMachine;
            set
            {
                if (value != MachineEnum.RotationMachine)
                    throw new ArgumentOutOfRangeException();
            }
        }

        private int _degrees;

        public int Degrees
        {
            get => _degrees;
            set
            {
                _degrees = value;
                InvokeTileChanged();
            }
        }

        public int NormalizedDegrees
        {
            get
            {
                int degrees = Degrees switch
                {
                    > 0 => Degrees % 360,
                    < 0 => -(-Degrees % 360),
                    _ => 0
                };
                switch (degrees)
                {
                    case > 180:
                        degrees -= 360;
                        break;
                    case < -180:
                        degrees += 360;
                        break;
                }

                return degrees;
            }
        }

        public override TileBase Tile => NormalizedDegrees > 0 ? TurnRight : TurnLeft;
    }
}
