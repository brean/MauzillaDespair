using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/**
 * Mit diesem Camera Manager kann man die Bewegung der Kamera steuen.
 * Es ist gedacht diesen Manager an an ein 'Managers' Object in der
 * Szene zu heften, Genauso wie den GameManager.
 * Aktuell sorgt dieser Camera Manager für einen Box Collider, welcher
 * die Spieler davon abhält aus dem Bild zu laufen.
 * Dabei wird die Kamera immer mittig zwischen den beiden äußersten
 * Spielern positioniert.
 * Im Einsatz bei UltimateSuperSmashSibs
 */
public class CameraManager : MonoBehaviour
{
    [Tooltip("max x of players")]
    public float xPlayerMax = 0;

    [Tooltip("min x of players")]
    public float xPlayerMin = 0;

    public float camHalfWidth = 0;
    public float mapBorderOffset = 0.5f;

    new GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
        camera = Camera.main.gameObject;
        camHalfWidth = ((2f * cam.orthographicSize) * cam.aspect) / 2f;

        camera.AddComponent<BoxCollider2D>();
        camera.AddComponent<BoxCollider2D>();

        BoxCollider2D[] colliders = camera.GetComponents<BoxCollider2D>();
        if (colliders.Length == 2)
        {
            //start
            colliders[0].offset = new Vector2(-camHalfWidth + mapBorderOffset - 10.5f, 0);
            colliders[0].size = new Vector2(20, 100);
            //end
            colliders[1].offset = new Vector2(camHalfWidth - mapBorderOffset + 10.5f, 0);
            colliders[1].size = new Vector2(20, 100);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // currently not working, because players do not have the tag Player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if(players.Length != 0)
        {
            List<float> xess = new List<float>();

            foreach (GameObject player in players)
            {
                try
                {
                    xess.Add(player.transform.position.x);
                }
                catch (NullReferenceException e)
                {
                    //mmmhh, guter code ...
                }

            }

            if (xess.Count != 0)
            {
                xPlayerMin = xess.Min();
                xPlayerMax = xess.Max();
            }

            // camera is always in the middle of all players
            // camera ignores padding
            float newCamPosX = xPlayerMin + ((xPlayerMax - xPlayerMin) / 2);
            if (newCamPosX < camHalfWidth)
            {
                camera.transform.position = new Vector3(camHalfWidth, camera.transform.position.y, camera.transform.position.z);
            }
            else
            {
                camera.transform.position = new Vector3(newCamPosX, camera.transform.position.y, camera.transform.position.z);
            }
        }
    }
}
