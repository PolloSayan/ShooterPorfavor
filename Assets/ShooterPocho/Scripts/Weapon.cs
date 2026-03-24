using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private int currentBullets;
    [SerializeField]
    private int maxMagazine;
    [SerializeField]
    private int totalBullets;
    //MuzzleFlash
    //Damage
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private bool automatic;
    private float timePass;
    private bool triggaPressed;
    private LevelManager lm;

    public UnityEvent ReloadEnemy;
    private void Start()
    {
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }


    private void Update()
    {
        if (triggaPressed == true)
        {
            Shoot();
            if (automatic != true)
            {
                triggaPressed = false;

            }
        }
        timePass += Time.deltaTime;
    }

    public void Shoot()
    {
        if (currentBullets > 0 && timePass >= fireRate)
        {
            //Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            //desde un punto exacto en PIXELES de la pantalla

            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            //desde un porcentaje, 0, 0 = esquina inferior izq.
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 bulletDirection = (hit.point - bulletSpawnPoint.position).normalized;
                GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bulletClone.GetComponent<Rigidbody>().linearVelocity = bulletDirection * bulletSpeed;
                //AudioManager.instance.PlaySFX(, bulletSpawnPoint.position);
                //VFX MuzzleFlash
            }
            else 
            {
                GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bulletClone.GetComponent<Rigidbody>().linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
            }
                currentBullets -= 1;
                timePass = 0;
                lm.UpdateAmmo();
        }
        
    }

    public void EnemyShoot(Transform _player)
    {
        if (timePass >= fireRate)
        { 
            if (currentBullets > 0)
            {
                GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                Vector3 direction = (_player.position+new Vector3(0,0.4f,0) - bulletSpawnPoint.position).normalized;
                bulletClone.GetComponent<Rigidbody>().linearVelocity = direction * bulletSpeed;
                timePass = 0;
                currentBullets -= 1;
            }
            else
            {
                ReloadEnemy.Invoke();
            }
        }
        /*if (currentBullets == 0)
        {
            Reload();
        
        } */  
    }

    public void TriggerDown() //aprieta el gatillo
    {
        triggaPressed = true;
    }

    public void TriggerUp()
    { 
    
    }

    public void Reload()
    {
        int bulletsToReload = maxMagazine - currentBullets;
        if (bulletsToReload < totalBullets)
        {
            currentBullets = maxMagazine;
            totalBullets -= bulletsToReload;
        }
        else 
        {
            currentBullets += totalBullets;
            totalBullets = 0;
        }
    }

    public string MagazineBullets
    {
        get { return currentBullets.ToString() + "/" + maxMagazine.ToString(); }
    
    }

    public string Bullets
    { 
        get { return totalBullets.ToString(); }
    }
}
