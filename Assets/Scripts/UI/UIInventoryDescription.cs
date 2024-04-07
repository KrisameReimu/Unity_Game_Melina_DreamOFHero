using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TMP_Text itemName;
        [SerializeField]
        private TMP_Text description;

        private void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            itemImage.gameObject.SetActive(false);
            itemName.text = "";
            description.text = "";
        }

        public void SetDescription(Sprite sprite, string name, string descriptionText)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            itemName.text = name;
            description.text = descriptionText;
        }
    }
}