using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseMonoBehaviour : MonoBehaviour
    {
        private readonly LinkedList<Tuple<string, Action<string>>> _registrations = new();

        private GeneralEventManager _eventManager;
        private GeneralEventManager EventManager
        {
            get
            {
                if (_eventManager == null)
                    _eventManager = FindFirstObjectByType<GeneralEventManager>();
                return _eventManager;
            }
        }


        protected void Register(string pattern, Action<string> handler)
        {
            _registrations.AddLast(new Tuple<string, Action<string>>(pattern, handler));
            EventManager.Register(pattern, handler);
        }


        protected void Unregister(string pattern, Action<string> handler)
        {
            _registrations.Remove(new Tuple<string, Action<string>>(pattern, handler));
            EventManager.Unregister(pattern, handler);
        }

        private void OnDestroy()
        {
            foreach (Tuple<string, Action<string>> registration in _registrations)
            {
                EventManager.Unregister(registration.Item1, registration.Item2);
            }
            _registrations.Clear();
        }

        protected void LaunchEvent(string eventName)
        {
            EventManager.LaunchEvent(eventName);
        }



    }
}
