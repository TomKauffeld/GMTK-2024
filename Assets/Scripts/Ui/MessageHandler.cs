using System.Collections.Generic;

namespace Assets.Scripts.Ui
{
    public class MessageHandler : BaseMonoBehaviour
    {
        private struct MessageItem
        {
            public string Text;
            public float Timeout;
        }

        public float DefaultTimeout;
        public Message MessagePopup;

        private readonly Queue<MessageItem> _messages = new();


        private void Start()
        {
            MessagePopup.gameObject.SetActive(false);
            MessagePopup.Timeout += OnMessageTimeout;
            Register("show:message:*", OnMessage);
        }

        private void OnMessage(string notification)
        {
            string[] parts = notification.Split(':');
            switch (parts.Length)
            {
                case 3:
                    _messages.Enqueue(new MessageItem
                    {
                        Text = parts[2],
                        Timeout = DefaultTimeout
                    });
                    CheckMessages();
                    break;
                case 4 when float.TryParse(parts[3], out float timeout):
                    _messages.Enqueue(new MessageItem
                    {
                        Text = parts[2],
                        Timeout = timeout
                    });
                    CheckMessages();
                    break;
            }
        }

        private void CheckMessages()
        {
            if (!MessagePopup.gameObject.activeSelf && _messages.TryDequeue(out MessageItem message))
            {
                MessagePopup.Text = message.Text;
                MessagePopup.Remaining = message.Timeout;
                MessagePopup.gameObject.SetActive(true);
            }
        }

        private void OnMessageTimeout(Message message)
        {
            MessagePopup.gameObject.SetActive(false);
            CheckMessages();
        }
    }
}
