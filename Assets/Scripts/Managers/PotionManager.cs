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
        potionUI.DisplayPotion(potionNum);

        potionUI.potionUseDelegate += UsePotion;
    }

    bool UsePotion()
    {
        if (potionNum <= 0) return false;

        potionNum--;
        potionUI.DisplayPotion(potionNum);

        // 플레이어 회복
        playerCharacter.AddToHP(50);

        return true;
    }
}
