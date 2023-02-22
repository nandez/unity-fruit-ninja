using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] protected float minSpawnDelay = 0.25f;
    [SerializeField] protected float maxSpawnDelay = 1f;
    [SerializeField] protected float minAngle = -15f;
    [SerializeField] protected float maxAngle = 15f;
    [SerializeField] protected float minForce = 15f;
    [SerializeField] protected float maxForce = 25f;
    [SerializeField] protected float maxLifeTime = 5f;
    [SerializeField] protected List<GameObject> fruitPrefabs;

    [Header("Bomb Settings")]
    [SerializeField] [Range(0,1)] protected float bombRate = 0.05f;
    [SerializeField] protected GameObject bombPrefab;

    private Collider spawnArea;

    void Awake()
    {
        spawnArea = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1f);

        while (enabled)
        {
            // Creamos el vector con la posición donde instanciar la fruta.
            var spawnPosition = new Vector3();
            spawnPosition.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            spawnPosition.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            spawnPosition.z = 0f;

            // Seteamos la rotación de la fruta.
            var spawnRotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

            // Seleccionamos una fruta aleatoria.
            var prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Count)];

            // Si el número aleatorio es menor que la probabilidad de bomba, cambiamos la fruta por una bomba.
            if(Random.value < bombRate)
                prefab = bombPrefab;

            // Instanciamos el prefab y lo destruimos después de un tiempo.
            var element = Instantiate(prefab, spawnPosition, spawnRotation);
            Destroy(element, maxLifeTime);

            // Añadimos una fuerza a la fruta para que salte.
            element.GetComponent<Rigidbody>().AddForce(element.transform.up * Random.Range(minForce, maxForce), ForceMode.Impulse);

            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }
}
