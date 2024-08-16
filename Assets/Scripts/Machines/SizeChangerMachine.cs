using System;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    internal class SizeChangerMachine : BaseMachine
    {
        public TileBase SizeUp1;
        public TileBase SizeUp2;

        public TileBase SizeDown1;
        public TileBase SizeDown2;

        public override MachineEnum MachineType
        {
            get => MachineEnum.SizeChangerMachine;
            set
            {
                if (value != MachineEnum.SizeChangerMachine)
                    throw new ArgumentOutOfRangeException();
            }
        }

        private int _multiplier;

        public int Multiplier
        {
            get => _multiplier;
            set
            {
                _multiplier = value;
                InvokeTileChanged();
            }
        }

        public override TileBase Tile
        {
            get
            {
                return Multiplier switch
                {
                    1 => SizeUp1,
                    >= 2 => SizeUp2,
                    -1 => SizeDown1,
                    <= -2 => SizeDown2,
                    _ => SizeUp1
                };
            }
        }
    }
}
