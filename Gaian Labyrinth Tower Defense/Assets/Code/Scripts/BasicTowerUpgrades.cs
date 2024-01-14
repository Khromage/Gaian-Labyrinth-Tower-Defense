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

    public void upgradeTowerStage(int updateStage, GameObject currentTower){
        switch(updateStage) {
        
            case 2:
                //generic tower upgrade stats
                Debug.Log("Tower upgraded to stage 2");
                BulletBehavior bulletToUpgrade = projectilePrefab.GetComponent<BulletBehavior>();

                //Placeholder Visual to change model, should actually change model here
                GameObject upgradeSphere = currentTower.transform.GetChild(0).gameObject;
                upgradeSphere.SetActive(true);
                currentTower.SetActive(true);

                currentDamage = 5f; 
                range = 10.2f;
                fireRate = 3f;
                cost = 200;
                multiPathUpgrade = true;

                break;

            case 10:
                currentTower.transform.Find("Head").GetComponent<Renderer>().material.SetColor("_Color", new Color(.1f, 0f, .1f, .1f));

                multiPathUpgrade = false;
                cost = 100;
                currentDamage = 10f;
                fireRate = 1f;
                break;

            case 20:
                currentTower.transform.Find("Head").GetComponent<Renderer>().material.SetColor("_Color", new Color(.1f, 0.92f, .016f, .1f));

                multiPathUpgrade = false;
                cost = 100;
                currentDamage = 2f;
                fireRate = 6f;
                break;

            case 30:
                currentTower.transform.Find("Head").GetComponent<Renderer>().material.SetColor("_Color", new Color(.1f, 0f, 0f, .1f));

                multiPathUpgrade = false;
                cost = 100;
                currentDamage = 7f;
                fireRate = 7f;
                break;

            default:
                Debug.Log("Tower upgrade stage not found");
                break;
        }
    }
}
