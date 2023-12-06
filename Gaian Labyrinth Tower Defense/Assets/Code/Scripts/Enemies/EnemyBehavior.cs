using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//currentTile
//goalTile = path.end  (set by spawnPoint when it gives the initial path? but there might not be a path when a wave starts if it's all blocked off...)
//when a tower is placed

public class EnemyBehavior : MonoBehaviour
{
    public delegate void EnemyDeath(GameObject deadEnemy);
    public static event EnemyDeath OnEnemyDeath;

    public delegate void EnemyReachedGoal(GameObject enemy);
    public static event EnemyReachedGoal OnEnemyReachedGoal;

    public GridTile currTile;
    public GridTile successorTile;
    public LayerMask Grid;
    private float moveSpeed = 3f;

    [SerializeField]
    private EnemyHealthBar HealthBar;
    private float maxHealth;
    public float currentHealth;

    public GameObject damageIndicator;

    private Camera cameraToWatch;

    //currency gain on death
    public int worth;

    //damage to core/remaining lives
    public int harm;

    //public float value? for when it dies

    public AudioSource EnemyHurtSFX;

    // Start is called before the first frame update
    void Start()
    {
        harm = 1;
        worth = 5;
        maxHealth = 12f;
        currentHealth = maxHealth;
        EnemyHurtSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //setGravityDir();
        updateCurrTile();
        moveAlongPath();
        
        if (currTile is GoalTile)
        {
            Debug.Log("reached end, presumably");
            OnEnemyReachedGoal?.Invoke(gameObject);
            //OnEnemyDeath?.Invoke(gameObject);
            Destroy(gameObject);
            Destroy(HealthBar.gameObject);
        }
    }

    public void setCamera(Camera camera)
    {
        cameraToWatch = camera;
        SetupHealthBar(camera);
    }
    public void SetupHealthBar(Camera Camera)
    {
        if(HealthBar.TryGetComponent<FaceCamera>(out FaceCamera faceCamera))
        {
            faceCamera.Camera = Camera;
        }
    }

    public void takeDamage(float damage, GameObject damagerBullet)
    {
        currentHealth -= damage;

        deployDamageIndicator(damage);

        HealthBar.SetHealth(currentHealth / maxHealth, 3);

        EnemyHurtSFX.Play();
        if(currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(gameObject);
            
            float destroyDelay = UnityEngine.Random.value;
            Destroy(gameObject, destroyDelay);
            Destroy(HealthBar.gameObject, destroyDelay);
        }
        
    }

    private void deployDamageIndicator(float damage)
    {
        GameObject dmgInd = Instantiate(damageIndicator, gameObject.transform);
        Destroy(dmgInd, 2f);
        TMP_Text dmgIndText = dmgInd.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        //dmgInd.GetComponent<ConstantForce>.force = currGravDir;
        dmgIndText.text = damage.ToString();

        if (damage >= 3)
            dmgIndText.color = Color.yellow;
        if (damage >= 10)
            dmgIndText.color = Color.red;

        if (dmgIndText.TryGetComponent<FaceCamera>(out FaceCamera faceCamera))
            faceCamera.Camera = cameraToWatch;

        //dmgInd.GetComponent<Rigidbody>().AddForce(new Vector3(Vector3.Dot(Vector3.right, transform.right) * UnityEngine.Random.value, 10f, Vector3.Dot(Vector3.forward, transform.forward) * UnityEngine.Random.value), ForceMode.Impulse);
        dmgInd.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.value * 3f - .5f, 12f, UnityEngine.Random.value * 3f - .5f), ForceMode.Impulse);
    }

    private void moveAlongPath()
    {
        //direction = normalize((successorPos + myHeightOffset) - currPos ); 
        //myHeightOffset = my height but rotated to the normal of the goal. Need to set that up. Matrix/vector multiplication
        Vector3 posToMoveToward = transform.position;
        if (successorTile != null)
            posToMoveToward = successorTile.transform.position;
        else
            Debug.Log("enemy no successor to move toward");
        Vector3 moveDirNormal = Vector3.Normalize((posToMoveToward + new Vector3(0f, .5f, 0f)) - transform.position);

        transform.Translate(moveDirNormal * moveSpeed * Time.deltaTime);
        //if (rb.velocity.magnitude < moveSpeed)
        //    rb.AddForce(moveDirNormal * moveSpeed * 10f, ForceMode.Force);

        //should also rotate toward where you're moving
    }

    private void updateCurrTile()
    {
        //get current tile. Might adjust this to check less often than on every frame.
        Ray ray = new Ray(this.transform.position, -this.transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, Grid))
        {
            //Debug.Log("hitting tile");
            currTile = hit.transform.GetComponent<GridTile>();
            successorTile = currTile.successor;
            currTile.enemyOnTile = true;
        }
    }

}
