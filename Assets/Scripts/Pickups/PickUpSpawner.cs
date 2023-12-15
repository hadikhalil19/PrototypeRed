using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe, ManaGlobe;

    [SerializeField] int maxRandRange = 10;
    [SerializeField] int healthRnumMin = 1;
    [SerializeField] int healthRnumMax = 2;
    [SerializeField] int manaRnumMin = 3;
    [SerializeField] int manaRnumMax = 4;
    [SerializeField] int goldRnumMin = 5;
    [SerializeField] int goldRnumMax = 6;
    [SerializeField] int goldDropMax = 3;
    [SerializeField] int goldDropMin = 1;


    public void DropItems() {
        int randomNum = Random.Range(1, maxRandRange);
        Debug.Log(randomNum);

        if (randomNum >= healthRnumMin && randomNum <= healthRnumMax) {
            Instantiate(healthGlobe, transform.position, Quaternion.identity); 
        } 

        if (randomNum >= manaRnumMin && randomNum <= manaRnumMax) {
            Instantiate(ManaGlobe, transform.position, Quaternion.identity); 
        }

        if (randomNum >= goldRnumMin && randomNum <= goldRnumMax) {
            int randomAmountOfGold = Random.Range(goldDropMin, goldDropMax+1);
            
            for (int i = 0; i < randomAmountOfGold; i++)
            {
                Instantiate(goldCoin, transform.position, Quaternion.identity);
            }
        }

    }

}
