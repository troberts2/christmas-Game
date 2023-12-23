using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventoryManager : MonoBehaviour
{
    public UpgradeClass[] upgradesGun;
    public UpgradeClass[] upgradeMove;
    public List<UpgradeClass> currentUpgrades = new List<UpgradeClass>();
    public Stack<CollectableClass> currentCollectables = new Stack<CollectableClass>();
    private UpgradeClass incomingUpgradeSO;
    [SerializeField] private Image incomingUpgrade;
    [SerializeField] private TextMeshProUGUI incomingUpgradeText;

    [SerializeField] private Image replace1;
    [SerializeField] private TextMeshProUGUI replace1Text;
    [SerializeField] private Image replace2;
    [SerializeField] private TextMeshProUGUI replace2Text;


    [SerializeField] private GameObject upgradeCanvas;
    [SerializeField] private GameObject slotHolderUpgrades;
    private GameObject[] slotsUpgrades;

    private void Start() {
        slotsUpgrades = new GameObject[slotHolderUpgrades.transform.childCount];
        //set slots for upgrades
        for(int i = 0; i < slotHolderUpgrades.transform.childCount; i ++){
            slotsUpgrades[i] = slotHolderUpgrades.transform.GetChild(i).gameObject;
        }
        RefreshUI();
    }

    public void RefreshUI(){
        for(int i = 0; i < slotsUpgrades.Length - 2; i++){
            try{
                slotsUpgrades[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slotsUpgrades[i].transform.GetChild(0).GetComponent<Image>().sprite = currentUpgrades[i].itemIcon;
            }catch{
                slotsUpgrades[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slotsUpgrades[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
        if(incomingUpgradeSO != null){
            if(incomingUpgradeSO.upgradeType == UpgradeClass.UpgradeType.weapon){
                replace1.sprite = currentUpgrades[0].itemIcon;
                replace1Text.text = currentUpgrades[0].itemName;
                if(currentUpgrades[1] != null){
                    replace2.sprite = currentUpgrades[1].itemIcon;
                    replace2Text.text = currentUpgrades[1].itemName;
                    replace2.enabled = true;
                    replace2Text.enabled = true;
                }
            }
            else if(incomingUpgradeSO.upgradeType == UpgradeClass.UpgradeType.movement){
                replace1.sprite = currentUpgrades[2].itemIcon;
                replace1Text.text = currentUpgrades[2].itemName;
                if(currentUpgrades[3] != null){
                    replace2.sprite = currentUpgrades[3].itemIcon;
                    replace2Text.text = currentUpgrades[3].itemName;
                    replace2.enabled = true;
                    replace2Text.enabled = true;
                }
            }
        }


    }

    public void AddUpgrade(UpgradeClass item, int index){
        currentUpgrades.Insert(index, item);
    }

    public void RemoveUpgrade(int index){
        currentUpgrades.RemoveAt(index);
    }

    public void SwapUpgrade(UpgradeClass item, int index){
        RemoveUpgrade(index);
        AddUpgrade(item, index);
        RefreshUI();
    }

    public void AddCollectable(CollectableClass item){
        currentCollectables.Push(item);
        Debug.Log(currentCollectables.Count);
        RefreshUI();
    }

    public void RemoveCollectable(){
        Debug.Log(currentCollectables.Count);
        currentCollectables.Pop();
    }
    public void GiveUpgradeGun(){
        UpgradeClass upgrade = upgradesGun[UnityEngine.Random.Range(0, 3)];
        incomingUpgradeSO = upgrade;
        incomingUpgrade.sprite = upgrade.itemIcon;
        incomingUpgradeText.text = upgrade.itemName;
        RefreshUI();
    }
    public void GiveUpgradeMovement(){
        UpgradeClass upgrade = upgradeMove[UnityEngine.Random.Range(0, 3)];
        incomingUpgradeSO = upgrade;
        incomingUpgrade.sprite = upgrade.itemIcon;
        incomingUpgradeText.text = upgrade.itemName;
        RefreshUI();
    }
    public void Replace1(){
        if(incomingUpgradeSO.upgradeType == UpgradeClass.UpgradeType.weapon){
            SwapUpgrade(incomingUpgradeSO, 0);
        }
        else{
            SwapUpgrade(incomingUpgradeSO, 2);
        }
        if(currentCollectables.Count < 1){
            upgradeCanvas.SetActive(false);
        }else{
            PlayerInteractWithSanta();
        }
    }
    public void Replace2(){
        if(incomingUpgradeSO.upgradeType == UpgradeClass.UpgradeType.weapon){
            SwapUpgrade(incomingUpgradeSO, 1);
        }
        else{
            SwapUpgrade(incomingUpgradeSO, 3);
        }
        if(currentCollectables.Count < 1){
            upgradeCanvas.SetActive(false);
        }else{
            PlayerInteractWithSanta();
        }
    }

    public void PlayerInteractWithSanta(){
        if(currentCollectables.Count > 0){
            upgradeCanvas.SetActive(true);
            if(currentCollectables.Peek().collectableType == CollectableClass.CollectableType.food){
                currentCollectables.Pop();
                GiveUpgradeGun();
            }
            else if(currentCollectables.Peek().collectableType == CollectableClass.CollectableType.drink){
                currentCollectables.Pop();
                GiveUpgradeMovement();
            }
            Debug.Log(currentCollectables.Count + " food left");
        }
    }
}
