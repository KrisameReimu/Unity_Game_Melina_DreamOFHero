using System;
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
        [SerializeField]
        private GameObject descriptionButtonPrefab;
        private GameObject descriptionButton;

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

        public void AddDescriptionButton(string name, Action onClickAction)
        {
            descriptionButton = Instantiate(descriptionButtonPrefab, transform);
            descriptionButton.GetComponent<Button>().onClick.AddListener(() => onClickAction());
            descriptionButton.GetComponentInChildren<TMP_Text>().text = name;
        }

        public void RemoveOldDescriptionButtons()
        {
            if (descriptionButton == null)
                return;
            //Debug.Log("remove");
            Destroy(descriptionButton);
            descriptionButton = null;
        }
    }
}