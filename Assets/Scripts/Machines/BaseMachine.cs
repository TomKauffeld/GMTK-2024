using Assets.Scripts.Figures;
using Assets.Scripts.World;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class BaseMachine : BaseMonoBehaviour
    {
        public float Speed = 1f;

        public WorldLogic World { get; private set; }
        public event Action<BaseMachine> TileChanged;

        public abstract MachineType MachineType { get; }
        public abstract TileBase Tile { get; }

        public virtual bool Placeable => true;
        public virtual bool CanHoldFigure => true;

        public Vector2Int Position => new((int)transform.localPosition.x, (int)transform.localPosition.y);

        protected void InvokeTileChanged()
        {
            TileChanged?.Invoke(this);
        }

        public abstract void Next();

        private readonly List<BaseFigure> _availableFigures = new();



        protected virtual void Start()
        {
            World = GetComponentInParent<WorldLogic>();
        }

        public abstract void UpdateFigure(BaseFigure figure);



        private void OnTriggerEnter2D(Collider2D other)
        {
            BaseFigure figure = other.GetComponentInParent<BaseFigure>();
            if (figure != null)
            {
                _availableFigures.Add(figure);
                figure.OnMachineEnter(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            BaseFigure figure = other.GetComponentInParent<BaseFigure>();
            if (figure != null)
            {
                figure.OnMachineExit(this);
                _availableFigures.Remove(figure);
            }
        }



        protected BaseMachine GetNextMachine(Direction direction)
        {
            Vector2Int offset = direction switch
            {
                Direction.Up => Vector2Int.up,
                Direction.Right => Vector2Int.right,
                Direction.Down => Vector2Int.down,
                Direction.Left => Vector2Int.left,
                _ => Vector2Int.zero
            };

            return World.GetMachine(Position + offset);
        }

        protected List<BaseMachine> GetNextMachines()
        {
            List<BaseMachine> machines = new();
            Direction[] directions = { Direction.Down, Direction.Up, Direction.Right, Direction.Left };

            foreach (Direction direction in directions)
            {
                BaseMachine machine = GetNextMachine(direction);

                if (machine != null && machine.CanHoldFigure)
                    machines.Add(machine);
            }

            return machines;
        }

        protected List<Belt> GetExitBelts()
        {
            List<Belt> machines = new();
            Direction[] directions = { Direction.Down, Direction.Up, Direction.Right, Direction.Left };

            foreach (Direction direction in directions)
            {
                BaseMachine machine = GetNextMachine(direction);

                if (machine is Belt belt && belt.Direction == direction && belt.CanHoldFigure)
                    machines.Add(belt);
            }

            return machines;
        }

        private ushort _nextExitIndex = 0;

        protected Belt GetExitBelt()
        {
            List<Belt> belts = GetExitBelts();

            return belts.Any()
                ? belts[(_nextExitIndex++) % belts.Count]
                : null;
        }

        protected Vector2 MoveFigureUp(Vector2 position, BaseFigure figure, BaseMachine nextMachine)
        {
            position.x = Position.x;
            position.y += Speed * Time.deltaTime;

            if (position.y >= nextMachine.Position.y)
                figure.SetActiveMachine(nextMachine);

            return position;
        }

        protected Vector2 MoveFigureDown(Vector2 position, BaseFigure figure, BaseMachine nextMachine)
        {
            position.x = Position.x;
            position.y -= Speed * Time.deltaTime;

            if (position.y <= nextMachine.Position.y)
                figure.SetActiveMachine(nextMachine);


            return position;
        }

        protected Vector2 MoveFigureLeft(Vector2 position, BaseFigure figure, BaseMachine nextMachine)
        {
            position.y = Position.y;
            position.x -= Speed * Time.deltaTime;

            if (position.x <= nextMachine.Position.x)
                figure.SetActiveMachine(nextMachine);

            return position;
        }

        protected Vector2 MoveFigureRight(Vector2 position, BaseFigure figure, BaseMachine nextMachine)
        {
            position.y = Position.y;
            position.x += Speed * Time.deltaTime;

            if (position.x >= nextMachine.Position.x)
                figure.SetActiveMachine(nextMachine);

            return position;
        }

        protected void MoveFigure(BaseFigure figure, Direction direction)
        {
            Vector2 position = figure.Position;
            BaseMachine nextMachine = GetNextMachine(direction);

            if (nextMachine == null || !nextMachine.CanHoldFigure)
                return;

            position = direction switch
            {
                Direction.Down => MoveFigureDown(position, figure, nextMachine),
                Direction.Up => MoveFigureUp(position, figure, nextMachine),
                Direction.Left => MoveFigureLeft(position, figure, nextMachine),
                Direction.Right => MoveFigureRight(position, figure, nextMachine),
                _ => position
            };

            figure.Position = position;
        }

        public virtual void AcceptFigure(BaseFigure figure)
        {
        }
    }
}
