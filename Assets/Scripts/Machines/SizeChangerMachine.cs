using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    internal class SizeChangerMachine : BaseMachine
    {
        public TileBase[] SizeUp1;
        public TileBase[] SizeUp2;

        public TileBase[] SizeDown1;
        public TileBase[] SizeDown2;

        public override MachineEnum MachineType => MachineEnum.SizeChangerMachine;

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

        public override TileBase[] Tile
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

        public override void Next()
        {
            Multiplier = Multiplier switch
            {
                1 => 2,
                >= 2 => -2,
                <= -2 => -1,
                -1 => 1,
                _ => 1,
            };
        }
    }
}
