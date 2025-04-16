using UnityEngine;

public class ParallaxLayerInfinite : MonoBehaviour
{
    public Transform cameraTransform;   // Transform kamery
    public Transform bg1;               // Prvý sprite pozadia
    public Transform bg2;               // Druhý sprite pozadia
    public float parallaxFactor = 0.5f; // Èím menšie èíslo, tým pomalší parallax

    private float spriteWidth;          // Šírka jedného spritu
    private float lastCameraX;          // Posledná pozícia kamery
    private Camera cam;                 // Kamera na výpoèet hraníc

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main Camera not found");
            return;
        }

        if (cameraTransform == null)
            cameraTransform = cam.transform;

        SpriteRenderer sr = bg1.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            spriteWidth = sr.bounds.size.x;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on " + bg1.name);
        }

        lastCameraX = cameraTransform.position.x; // Na zaèiatku uložíme X pozíciu kamery
    }

    void Update()
    {
        float cameraRightEdge = cameraTransform.position.x + (cam.orthographicSize * cam.aspect);
        float cameraLeftEdge = cameraTransform.position.x - (cam.orthographicSize * cam.aspect);

        // Posun pozadia pod¾a parallax efektu
        float deltaX = cameraTransform.position.x - lastCameraX;
        bg1.position += Vector3.right * (deltaX * parallaxFactor);
        bg2.position += Vector3.right * (deltaX * parallaxFactor);
        lastCameraX = cameraTransform.position.x;

        // Posuò BG1 dopredu, ak je úplne v¾avo mimo doh¾adu
        if (cameraLeftEdge > bg1.position.x + spriteWidth / 2)
        {
            bg1.position = new Vector3(bg2.position.x + spriteWidth, bg1.position.y, bg1.position.z);
            SwapBackgrounds();
        }

        // Posuò BG2 dopredu, ak je úplne v¾avo mimo doh¾adu
        if (cameraLeftEdge > bg2.position.x + spriteWidth / 2)
        {
            bg2.position = new Vector3(bg1.position.x + spriteWidth, bg2.position.y, bg2.position.z);
            SwapBackgrounds();
        }

        // Posuò BG1 dozadu, ak je úplne vpravo mimo doh¾adu
        if (cameraRightEdge < bg1.position.x - spriteWidth / 2)
        {
            bg1.position = new Vector3(bg2.position.x - spriteWidth, bg1.position.y, bg1.position.z);
            SwapBackgrounds();
        }

        // Posuò BG2 dozadu, ak je úplne vpravo mimo doh¾adu
        if (cameraRightEdge < bg2.position.x - spriteWidth / 2)
        {
            bg2.position = new Vector3(bg1.position.x - spriteWidth, bg2.position.y, bg2.position.z);
            SwapBackgrounds();
        }
    }

    private void SwapBackgrounds()
    {
        // Prehodenie referencií, aby logika zostala konzistentná
        Transform temp = bg1;
        bg1 = bg2;
        bg2 = temp;
    }
}
