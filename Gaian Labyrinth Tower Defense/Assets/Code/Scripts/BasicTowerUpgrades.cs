using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTowerUpgrades : TowerBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        StartTowerBehavior();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTowerBehavior();
    }

    public void towerCostAfterPlacement(){
        cost = 100;
    }

    public void upgradeTowerStage(int updateStage){
        switch(updateStage) {
        
            case 2:
                //generic tower upgrade stats
                Debug.Log("Tower upgraded to stage 2");
                BulletBehavior bulletToUpgrade = projectilePrefab.GetComponent<BulletBehavior>();

                //Placeholder Visual to change model, should actually change model here
                GameObject upgradeSphere = transform.GetChild(0).gameObject;
                upgradeSphere.SetActive(true);
                

                currentDamage = 2f; 
                range = 10.2f;
                fireRate = 3f;
                cost = 200;
                multiPathUpgrade = true;

                break;

            case 10:
                Debug.Log("Tower upgraded to stage 10");
                transform.Find("UpgradeSphere").GetComponent<Renderer>().material.SetColor("_Color", new Color(100, 0f, .1f, .1f));

                multiPathUpgrade = false;
                cost = 100;
                currentDamage = 5f;
                fireRate = 1f;
                break;

            case 20:
                transform.Find("UpgradeSphere").GetComponent<Renderer>().material.SetColor("_Color", new Color(0,100,0, .1f));

                multiPathUpgrade = false;
                cost = 100;
                currentDamage = 2f;
                fireRate = 6f;
                break;

            case 30:
                transform.Find("UpgradeSphere").GetComponent<Renderer>().material.SetColor("_Color", new Color(0,0,100, .1f));

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
