using UnityEngine;

// Clase encargada de gestionar el movimiento de la camara y el zoom
public class CameraController2 : MonoBehaviour
{
    private Camera cam;

    [Header("Configuracion")]
    public float sensibilidad = 0.5f;

    [Header("Limites de movimiento de la camara")]
    public float minX = -1.58f; // Tope izquierda
    public float maxX = -1.34f;  // Tope derecha
    public float minZ = -14.8f; // Tope abajo
    public float maxZ = -7.38f;  // Tope arriba

    [Header("Zoom camara")]
    public float zoomSpeed = 2f;
    public float minZoom = 1.5f;
    public float maxZoom = 3.52f;

    private float mapTop, mapBottom, mapLeft, mapRight;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        UpdateMapBounds();

        if (Input.GetMouseButton(1))
        {
            float moveX = Input.GetAxis("Mouse X");
            float moveZ = Input.GetAxis("Mouse Y");

            // Se multiplica por el zoom actual
            float fuerza = sensibilidad * cam.orthographicSize * 0.1f;
            Vector3 movimiento = new Vector3(-moveX * fuerza, 0, -moveZ * fuerza);

            transform.Translate(movimiento, Space.World);
        }

        // Zoom de la camara
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize -= scroll * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

        // Limitar movimiento de la camara
        DinamicClamp();
    }


    void UpdateMapBounds()
    {
        float vertExtent = maxZoom;
        float horzExtent = maxZoom * cam.aspect;

        mapLeft = minX - horzExtent;
        mapRight = maxX + horzExtent;
        mapBottom = minZ - vertExtent;
        mapTop = maxZ + vertExtent;
    }

    // Funcion para limitar el movimiento de la camara tenga + o - zoom (limite dinamico: se recalcula al modificar el zoom)
    void DinamicClamp()
    {
        // Se calculan las medidas de la camara
        float vertExtent = cam.orthographicSize;
        float horzExtent = cam.orthographicSize * cam.aspect;

        Vector3 pos = transform.position;

        // Limite de la camara dinamico: limite con zoom maximo - tamaño actual de la camara
        //  Camara pequeña -> se expande el limite
        //  Camara grande -> limite marcado en inspector
        float currentMinX = mapLeft + horzExtent;
        float currentMaxX = mapRight - horzExtent;
        float currentMinZ = mapBottom + vertExtent;
        float currentMaxZ = mapTop - vertExtent;

        if (currentMinX > currentMaxX)
        {
            pos.x = (minX + maxX) / 2;
        }
        else
        {
            pos.x = Mathf.Clamp(pos.x, currentMinX, currentMaxX);
        }

        if (currentMinZ > currentMaxZ)
        {
            pos.z = (minZ + maxZ) / 2;
        }
        else
        {
            pos.z = Mathf.Clamp(pos.z, currentMinZ, currentMaxZ);
        }

        transform.position = pos;
    }

    // Funcion para dibujar los limites en la escena
    private void OnDrawGizmos()
    {
        if (cam == null) cam = GetComponent<Camera>();

        Gizmos.color = Color.yellow;
        Vector3 centerUser = new Vector3((minX + maxX) / 2, 0, (minZ + maxZ) / 2);
        Vector3 sizeUser = new Vector3(maxX - minX, 1, maxZ - minZ);
        Gizmos.DrawWireCube(centerUser, sizeUser);
    }
}
