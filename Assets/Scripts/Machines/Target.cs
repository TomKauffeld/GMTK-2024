using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class Target : BaseMachine
    {
        public TileBase TargetTile;

        public override MachineType MachineType => MachineType.Target;


        public override bool Placeable => false;

        public override TileBase Tile => TargetTile;

        public override void Next()
        {
        }
    }
}
