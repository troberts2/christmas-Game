using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Upgrade Class", menuName = "Item/Upgrade")]
public class UpgradeClass : ItemClass
{
    [Header("Upgrade")]
    public UpgradeType upgradeType;
    public enum UpgradeType{
        weapon,
        movement,
    }
    public Sprite gunSprite;
    public override ItemClass GetItem()
    {
        return this;
    }
    public override UpgradeClass GetUpgrade()
    {
        return this;
    }
    public override CollectableClass GetCollectable()
    {
        return null;
    }
}
