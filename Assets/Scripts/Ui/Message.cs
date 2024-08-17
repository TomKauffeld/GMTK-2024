using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class Message : MonoBehaviour
    {
        public event Action<Message> Timeout;

        public string Text;
        public float Remaining = 5;

        private void OnEnable()
        {
            GetComponentInChildren<TextMeshProUGUI>().text = Text;
        }

        private void Update()
        {
            Remaining -= Time.deltaTime;
            if (Remaining < 0)
                OnTimeout();
        }

        private void OnTimeout()
        {
            Timeout?.Invoke(this);
        }
    }
}
