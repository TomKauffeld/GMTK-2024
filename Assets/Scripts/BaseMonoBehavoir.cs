using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseMonoBehaviour : MonoBehaviour
    {
        private readonly LinkedList<Tuple<string, Action<string>>> _registrations = new();

        private NotificationManager _notificationManager;
        private NotificationManager NotificationManager
        {
            get
            {
                if (_notificationManager == null)
                    _notificationManager = FindFirstObjectByType<NotificationManager>();
                return _notificationManager;
            }
        }


        protected void Register(string pattern, Action<string> handler)
        {
            _registrations.AddLast(new Tuple<string, Action<string>>(pattern, handler));
            NotificationManager.Register(pattern, handler);
        }


        protected void Unregister(string pattern, Action<string> handler)
        {
            _registrations.Remove(new Tuple<string, Action<string>>(pattern, handler));
            NotificationManager.Unregister(pattern, handler);
        }

        private void OnDestroy()
        {
            if (NotificationManager != null)
            {
                foreach (Tuple<string, Action<string>> registration in _registrations)
                {
                    NotificationManager.Unregister(registration.Item1, registration.Item2);
                }
                _registrations.Clear();
            }
        }

        protected void LaunchNotification(string notification)
        {
            NotificationManager.LaunchNotification(notification);
        }
    }
}
