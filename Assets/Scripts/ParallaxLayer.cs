using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxSpeedMultiplier;

    private Transform cameraTransform;
    private Vector2 lastCameraPosition;
    private float textureUnitSizeX;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 deltaMovement = (Vector2)cameraTransform.position - lastCameraPosition;
        transform.position += (Vector3)new Vector2(deltaMovement.x * parallaxSpeedMultiplier.x, deltaMovement.y * parallaxSpeedMultiplier.y);
        lastCameraPosition = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector2(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }
}
