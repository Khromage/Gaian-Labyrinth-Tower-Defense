using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTowerUpgrades : TowerBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void towerCostAfterPlacement(){
        cost = 100;
    }

    public void upgradeTower(int updateStage, GameObject currentTower){
        switch(updateStage) {
        
            case 2:
                //generic tower upgrade stats
                Debug.Log("Tower upgraded to stage 2");
                BulletBehavior bulletToUpgrade = bulletPrefab.GetComponent<BulletBehavior>();
                GameObject upgradeSphere = currentTower.transform.GetChild(0).gameObject;
                upgradeSphere.SetActive(true);
                currentTower.SetActive(true);

                currentDamage = 5f; 
                range = 10.2f;
                fireRate = 3f;
                cost = 200;

                break;

            case 3:
                cost = 100;
                break;

            case 4:
                break;

            default:
                Debug.Log("Tower upgrade stage not found");
                break;
        }
    }
}
