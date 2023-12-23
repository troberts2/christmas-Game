using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Collectable Class", menuName = "Item/Collectable")]
public class CollectableClass : ItemClass
{
    public CollectableType collectableType;
    public enum CollectableType{
        food,
        drink,
    }

    public override ItemClass GetItem()
    {
        return this;
    }
    public override UpgradeClass GetUpgrade()
    {
        return null;
    }
    public override CollectableClass GetCollectable()
    {
        return this;
    }
}
