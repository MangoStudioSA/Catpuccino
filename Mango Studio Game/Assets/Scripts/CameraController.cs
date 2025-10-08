using UnityEngine;

public class CameraController : MonoBehaviour
{
    //MOVIMIENTO
    [SerializeField] float CamSpeed = 0.5f; //velocidad de movimiento de la camara
    [SerializeField] float minX = -10f;
    [SerializeField] float maxX = 10f;
    [SerializeField] float minY = 5f;
    [SerializeField] float maxY = 15;

    Vector3 lastMousePos;

    //ZOOM
    [SerializeField] float zoomSpeed = 10f; //velocidad del zoom
    [SerializeField] float minZoom = 25f;
    [SerializeField] float maxZoom = 60f;


    // Update is called once per frame
    void Update()
    {
        //si mantienes el click derecho (1) se mueve la camara
        //se almacena la posicion inicial del cursor al hacer click
        if (Input.GetMouseButtonDown(1)) //(solo el primer frame)
        {
            lastMousePos = Input.mousePosition;
            //este es el punto de partid aque usamos para saber desde donde mover la camara
        }

        //mover la camara mientras se esta manteniendo el click
        if (Input.GetMouseButton(1))  //(todo el tiempo que se mantenga)
        {
            //diferencia entre el punto de partida y el actual
            Vector3 a = Input.mousePosition - lastMousePos;
            //se crea un vector nuevo, este defina la direccion del movimiento
            Vector3 move = new Vector3(-a.x * CamSpeed * Time.deltaTime, -a.y * CamSpeed * Time.deltaTime, 0);
            //mueve la camara en funcion al vector creado
            transform.Translate(move, Space.World);

            //limites
            //variable temporal para la psocion actual de la camra
            Vector3 camPos = transform.position;
            //limitamos la posicion entre dos puntos con un clamp
            camPos.x = Mathf.Clamp(camPos.x, minX, maxX); 
            camPos.y = Mathf.Clamp(camPos.y, minY, maxY);
            transform.position = camPos;

            //se actualiza la posicion
            lastMousePos = Input.mousePosition;
        }

        //ZOOM
        float zoom = Input.GetAxis("Mouse ScrollWheel"); //detecta el movimiento del raton
        Camera.main.fieldOfView -= zoom * zoomSpeed; //fieldOfView define que porcentaje de la camara ve el jugador

        //limites del zoom
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoom, maxZoom); //nos aseguramos de que se mantenga el zoom en el rango
    }
}
