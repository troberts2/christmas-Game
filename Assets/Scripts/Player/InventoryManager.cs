using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public UpgradeClass[] upgradesGun;
    public UpgradeClass[] upgradeMove;
    public List<UpgradeClass> currentUpgrades = new List<UpgradeClass>();
    public Stack<CollectableClass> currentCollectables = new Stack<CollectableClass>();
    private UpgradeClass incomingUpgradeSO;
    [SerializeField] private Image incomingUpgrade;
    [SerializeField] private TextMeshProUGUI incomingUpgradeText;
    private int collectableCount = 0;
    [SerializeField] private AudioClip select;
    private AudioSource audioSource;

    [SerializeField] private Image replace1;
    [SerializeField] private TextMeshProUGUI replace1Text;
    [SerializeField] private Image replace2;
    [SerializeField] private TextMeshProUGUI replace2Text;
    private dialogue dialogue;


    [SerializeField] private GameObject upgradeCanvas;
    [SerializeField] private GameObject slotHolderUpgrades;
    private GameObject[] slotsUpgrades;
    [SerializeField] private GameObject slotHolderInv;
    private GameObject[] slotsInv;
    private SantaBehavior santa;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        slotsUpgrades = new GameObject[slotHolderUpgrades.transform.childCount];
        //set slots for upgrades
        for(int i = 0; i < slotHolderUpgrades.transform.childCount; i ++){
            slotsUpgrades[i] = slotHolderUpgrades.transform.GetChild(i).gameObject;
        }

        //slots for inventory on pause
        slotsInv = new GameObject[slotHolderInv.transform.childCount];
        for(int i = 0; i < slotHolderInv.transform.childCount; i ++){
            slotsInv[i] = slotHolderInv.transform.GetChild(i).gameObject;
        }
        dialogue = FindObjectOfType<dialogue>();
        santa = FindObjectOfType<SantaBehavior>();
        RefreshUI();
    }

    public void RefreshUI(){
        //upgrade screen update
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
                if(currentUpgrades[0] != null){
                    replace1.sprite = currentUpgrades[0].itemIcon;
                    replace1Text.text = currentUpgrades[0].itemName;
                }
                else{
                    replace1.sprite = null;
                    replace1Text.text = null;
                }

                if(currentUpgrades[1] != null){
                    replace2.sprite = currentUpgrades[1].itemIcon;
                    replace2Text.text = currentUpgrades[1].itemName;
                }
                else{
                    replace2.sprite = null;
                    replace2Text.text = null;
                }
            }
            else if(incomingUpgradeSO.upgradeType == UpgradeClass.UpgradeType.movement){
                if(currentUpgrades[2] != null){
                    replace1.sprite = currentUpgrades[2].itemIcon;
                    replace1Text.text = currentUpgrades[2].itemName;
                }
                else{
                    replace1.sprite = null;
                    replace1Text.text = null;
                }
                if(currentUpgrades[3] != null){
                    replace2.sprite = currentUpgrades[3].itemIcon;
                    replace2Text.text = currentUpgrades[3].itemName;
                }
                else{
                    replace2.sprite = null;
                    replace2Text.text = null;
                }
            }
        }


        //Inventory update
        for(int i = 0; i < slotsInv.Length - 2; i++){
            try{
                slotsInv[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slotsInv[i].transform.GetChild(0).GetComponent<Image>().sprite = currentUpgrades[i].itemIcon;
            }catch{
                slotsInv[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slotsInv[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
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
        collectableCount++;
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
            showUpgradeScreen();
        }
        audioSource.clip = select;
        audioSource.Play();
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
            showUpgradeScreen();
        }
        audioSource.clip = select;
        audioSource.Play();
    }
    public void CancelUpgrade(){
        if(currentCollectables.Count < 1){
            upgradeCanvas.SetActive(false);
        }else{
            showUpgradeScreen();
        }
        audioSource.clip = select;
        audioSource.Play();
    }

    public IEnumerator PlayerInteractWithSanta(){
        if(currentCollectables.Count > 0){
            if(collectableCount == 6){
                SceneManager.LoadScene("WinScreen");
            }
            santa.animator.SetTrigger("walk");
            santa.timerLength += 60;
            GetComponent<PlayerMovement>().curHp = GetComponent<PlayerMovement>().maxHp;
            yield return new WaitForSeconds(4f);
            showUpgradeScreen();
        }else{
            dialogue.StartDialogue();
        }
        yield return null;
    }
    private void showUpgradeScreen(){
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
