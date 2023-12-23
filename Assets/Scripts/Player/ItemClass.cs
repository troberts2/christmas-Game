using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemClass : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;

    public abstract ItemClass GetItem();
    public abstract UpgradeClass GetUpgrade();

    public abstract CollectableClass GetCollectable();

}
