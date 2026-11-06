using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LightsaberController : MonoBehaviour
{
    public GameObject LIGHTSABER;

    public AudioClip powerOnSound;
    public AudioClip powerOffSound;

    public Light saberLight;
    private float maxLightIntensity;

    private AudioSource soundSource;
    private Material bladeMaterial;

    private float extendSpeed = 0.1f;
    private bool weaponActivate;
    private const float SCALE_MIN = 0f;
    private float maxScaler;
    private float changeExt;
    private float currScale;
    private float localScaleX;
    private float localScaleZ;

    void Start()
    {
        localScaleX = transform.localScale.x;
        localScaleZ = transform.localScale.z;

        maxScaler = transform.localScale.y;
        currScale = maxScaler;

        changeExt = maxScaler / extendSpeed;
        weaponActivate = true;

        soundSource = GetComponent<AudioSource>();
        bladeMaterial = LIGHTSABER.GetComponent<Renderer>().material;

        maxLightIntensity = saberLight.intensity;
        saberLight.color = bladeMaterial.GetColor("_BladeColor");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            changeExt = weaponActivate
                ? -Mathf.Abs(changeExt)
                : Mathf.Abs(changeExt);

            soundSource.Stop();

            if (weaponActivate)
            {
                soundSource.clip = powerOffSound;
            }
            else
            {
                soundSource.clip = powerOnSound;
            }

            if (soundSource.clip != null)
            {
                soundSource.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bladeMaterial.SetColor("_BladeColor", Color.blue);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bladeMaterial.SetColor("_BladeColor", Color.green);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bladeMaterial.SetColor("_BladeColor", Color.red);
        }

        currScale += changeExt * Time.deltaTime;

        currScale = Mathf.Clamp(
            currScale,
            SCALE_MIN,
            maxScaler
        );

        transform.localScale = new Vector3(
            localScaleX,
            currScale,
            localScaleZ
        );

        weaponActivate = currScale > 0;

        if (weaponActivate && !LIGHTSABER.activeSelf)
        {
            LIGHTSABER.SetActive(true);
        }
        else if (!weaponActivate && LIGHTSABER.activeSelf)
        {
            LIGHTSABER.SetActive(false);
        }
        saberLight.color = bladeMaterial.GetColor("_BladeColor");

        saberLight.intensity = maxLightIntensity * (currScale / maxScaler);

        saberLight.enabled = weaponActivate;

        saberLight.transform.position = LIGHTSABER.transform.position;

    }

    void OnDestroy()
    {
        if (bladeMaterial != null)
        {
            Destroy(bladeMaterial);
        }
    }
}