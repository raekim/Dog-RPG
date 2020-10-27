using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    public Character playerCharacter;
    public PotionUI potionUI;
    public int startPotionNum;
    int potionNum;

    // Start is called before the first frame update
    void Start()
    {
        potionNum = startPotionNum;
        OnPotionNumChange();

        potionUI.potionUseDelegate += UsePotion;
    }

    public void AddToPotionNum(int amount)
    {
        potionNum += amount;
        OnPotionNumChange();
    }

    bool UsePotion()
    {
        if (potionNum <= 0) return false;

        potionNum--;
        OnPotionNumChange();

        // 플레이어 회복
        playerCharacter.AddToHP(50);

        return true;
    }

    void OnPotionNumChange()
    {
        potionUI.DisplayPotion(potionNum);
    }
}
