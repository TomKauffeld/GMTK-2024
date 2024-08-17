using Assets.Scripts.Machines;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class InventoryItem : BaseMonoBehaviour
    {
        public GameObject SelectedGameObject;

        public bool Selected
        {
            set
            {
                SelectedGameObject.SetActive(value);
                LaunchNotification($"switch:machine:{MachineType}");
            }
            get => SelectedGameObject.activeSelf;
        }

        public MachineType MachineType;

        public TextMeshProUGUI TitleUi;
        public TextMeshProUGUI CountUi;

        public int Count = 0;

        private void Start()
        {
            Register($"inventory:update:{MachineType}:*", OnInventoryUpdate);
            Register($"switch:machine:*", OnMachineSelected);


            TitleUi.text = $"{MachineType}";
            UpdateCount();
        }

        private void OnMachineSelected(string notification)
        {
            bool isThisMachine = notification == $"switch:machine:{MachineType}";
            if (isThisMachine != Selected)
                SelectedGameObject.SetActive(isThisMachine);
        }

        private void OnInventoryUpdate(string notification)
        {
            string data = notification.Split(':').Last();

            if (int.TryParse(data, out int count))
            {
                Count = count;
                UpdateCount();
            }
        }


        private void UpdateCount()
        {
            CountUi.text = Count >= 0 ? Count.ToString() : "";
        }

        public void OnClick()
        {
            Selected = true;
        }
    }
}
