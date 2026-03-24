using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ammoMag, totalAmmo;
    [SerializeField]
    private Volume volume;
    private Vignette vignette;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volume.profile.TryGet<Vignette>(out vignette);
        UpdateAmmo();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAmmo()
    {

        ammoMag.text = GameManager.instance.GetGameData.Weapons[GameManager.instance.GetGameData.WeaponIndex].MagazineBullets;
        totalAmmo.text = GameManager.instance.GetGameData.Weapons[GameManager.instance.GetGameData.WeaponIndex].Bullets;


    }

    public void UpdateLife()
    {
        float percentage = 1 - (GameManager.instance.GetGameData.CurrentHP / GameManager.instance.GetGameData.MaxHP);
        vignette.intensity.value = percentage * 0.5f;
    }

}
 