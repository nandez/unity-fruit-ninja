using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private bool isCutting;
    private Collider swordCollider;

    [SerializeField] protected Camera mainCamera;
    [SerializeField] protected float minVelocity = 0.01f;
    [SerializeField] protected TrailRenderer trail;

    public Vector3 Direction { get; private set; }

    private void Awake()
    {
        swordCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCut();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndCut();
        }
        else if (isCutting)
        {
            UpdateCut();
        }
    }

    void StartCut()
    {
        transform.position = GetMousePosition();

        isCutting = true;
        swordCollider.enabled = true;
        trail.enabled = true;
        trail.Clear();
    }

    void EndCut()
    {
        isCutting = false;
        swordCollider.enabled = false;
        trail.enabled = false;
    }

    void UpdateCut()
    {
        var newPosition = GetMousePosition();

        // Calculamos la dirección del corte.
        Direction = newPosition - transform.position;

        // Calculamos la velocidad del corte.
        var velocity = Direction.magnitude / Time.deltaTime;
        swordCollider.enabled = velocity > minVelocity;

        // Actualizamos la posición de la espada.
        transform.position = newPosition;
    }

    private void OnEnable()
    {
        EndCut();
    }

    private void OnDisable()
    {
        EndCut();
    }

    protected Vector3 GetMousePosition()
    {
        var pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;

        return pos;
    }
}
