using Assets.Scripts.Figures;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class Spawner : BaseMachine
    {
        public BaseFigure SquarePrototype;
        public BaseFigure CirclePrototype;

        public TileBase SpawnerTile;

        public override MachineType MachineType => MachineType.Spawner;

        public override bool Placeable => false;

        public override TileBase Tile => SpawnerTile;

        public FigureType Figure;

        protected override void Start()
        {
            base.Start();
            Register("level:start", OnLevelStart);
        }

        private BaseFigure CreateFigure()
        {
            return Figure switch
            {
                FigureType.Circle => Instantiate(CirclePrototype),
                FigureType.Square => Instantiate(SquarePrototype),
                _ => null
            };
        }

        private void OnLevelStart(string notification)
        {
            BaseFigure figure = CreateFigure();
            figure.SetActiveMachine(this);
            figure.Position = Position;
        }

        public override void Next()
        {
        }

        public override void UpdateFigure(BaseFigure figure)
        {
            MoveFigure(figure, Direction.Left);
        }
    }
}
