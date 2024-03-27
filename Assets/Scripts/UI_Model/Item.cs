using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ItemSO/Item")]
public class Item : ScriptableObject
{
    public int ID => GetInstanceID();

    [field: SerializeField] 
    public bool isStackable {  get; set; }
    

    [field: SerializeField]
    public int maxStackSize { get; set; } = 1;

    [field: SerializeField]
    public string itemName { get; set; }

    [field: SerializeField]
    [field: TextArea]
    public string description;

    [field: SerializeField]
    public Sprite itemImage;
}
