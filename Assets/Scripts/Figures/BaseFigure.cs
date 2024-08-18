using Assets.Scripts.Machines;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Figures
{
    public class BaseFigure : BaseMonoBehaviour
    {

        public float Target;

        private readonly List<BaseMachine> _machines = new();
        public BaseMachine CurrentMachine { get; private set; }

        public GameObject Graphics;

        public Quaternion Rotation
        {
            get => Graphics.transform.rotation;
            set => Graphics.transform.rotation = value;
        }

        public Vector2 Position
        {
            get
            {
                float x = transform.position.x;
                float y = transform.position.y;
                return new Vector2(x - 0.5f, y - 0.5f);
            }
            set
            {
                float x = value.x + 0.5f;
                float y = value.y + 0.5f;
                transform.position = new Vector3(x, y, transform.position.z);
            }
        }



        private void Update()
        {
            lock (MergingList)
            {
                while (ToMerge.TryDequeue(out Tuple<BaseFigure, BaseFigure> merge))
                {
                    merge.Item1.CombineToOtherFigure(merge.Item2);
                }
                MergingList.Clear();
            }



            if (!this.IsDestroyed() && CurrentMachine != null && !CurrentMachine.World.Construction)
            {
                CurrentMachine.UpdateFigure(this);
            }
        }


        public void SetActiveMachine(BaseMachine machine)
        {
            CurrentMachine = machine ?? _machines.FirstOrDefault();
            if (CurrentMachine != null)
                CurrentMachine.AcceptFigure(this);
        }

        public void OnMachineEnter(BaseMachine machine)
        {
            _machines.Add(machine);
            if (CurrentMachine == null)
                SetActiveMachine(machine);
        }

        public void OnMachineExit(BaseMachine machine)
        {
            _machines.Remove(machine);
            if (CurrentMachine == machine)
                CurrentMachine = _machines.FirstOrDefault();
        }

        private static readonly object MergingLock = new();
        private static readonly List<int> MergingList = new();
        private static readonly ConcurrentQueue<Tuple<BaseFigure, BaseFigure>> ToMerge = new();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            BaseFigure other = collision.gameObject.GetComponentInParent<BaseFigure>();
            if (other != null)
            {
                int tid = gameObject.GetInstanceID();
                int oid = collision.gameObject.GetInstanceID();

                if (tid == oid)
                    return;

                lock (MergingLock)
                {
                    if (MergingList.All(id => id != tid && id != oid))
                    {
                        MergingList.Add(tid);
                        MergingList.Add(oid);
                        ToMerge.Enqueue(new Tuple<BaseFigure, BaseFigure>(this, other));
                    }
                }
            }
        }


        public void CombineToOtherFigure(BaseFigure other)
        {
            if (this.IsDestroyed() || other.IsDestroyed())
                return;

            int tid = gameObject.GetInstanceID();
            int oid = other.gameObject.GetInstanceID();


            LaunchNotification($"Merging {tid} {oid} starting");

            GameObject otherGameObject = other.gameObject;

            GameObject newFigure = new("Composite");
            GameObject newGraphics = new("Composite - Graphics");

            BaseFigure newBaseFigure = newFigure.AddComponent<BaseFigure>();

            List<Collider2D> colliders = gameObject.GetComponentsInChildren<Collider2D>().ToList();
            colliders.AddRange(otherGameObject.GetComponentsInChildren<Collider2D>());

            float foundMinX = float.PositiveInfinity;
            float foundMinY = float.PositiveInfinity;
            float foundMaxX = float.NegativeInfinity;
            float foundMaxY = float.NegativeInfinity;

            foreach (Collider2D collider in colliders)
            {
                float minX = collider.bounds.min.x;
                float minY = collider.bounds.min.y;
                float maxX = collider.bounds.max.x;
                float maxY = collider.bounds.max.y;

                foundMinX = Mathf.Min(minX, foundMinX);
                foundMinY = Mathf.Min(minY, foundMinY);
                foundMaxX = Mathf.Max(maxX, foundMaxX);
                foundMaxY = Mathf.Max(maxY, foundMaxY);

            }

            newBaseFigure.transform.position = new Vector3(
                (foundMinX + foundMaxX) / 2f,
                (foundMinY + foundMaxY) / 2f,
                0
            );

            newBaseFigure.Graphics = newGraphics;
            newGraphics.transform.parent = newFigure.transform;
            newGraphics.transform.localPosition = Vector3.zero;

            otherGameObject.transform.parent = newGraphics.transform;
            gameObject.transform.parent = newGraphics.transform;

            newBaseFigure.CurrentMachine = CurrentMachine;


            LaunchNotification($"Merging {tid} {oid} done");

            Destroy(other);
            Destroy(this);

            LaunchNotification($"Merging {tid} {oid} deleted");
        }
    }
}
