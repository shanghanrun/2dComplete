using UnityEngine;
using System.Collections;

public class ObjectCreator : MonoBehaviour
{
    public static ObjectCreator instance;

    [Header("Traps")]
    public GameObject arrowPrefab;
    public float delay = 1f;

    void Awake(){
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateObject(GameObject prefab, Transform target, bool shouldBeDestroyed=false)
    {
        StartCoroutine(CreateObjectRoutine(prefab, target, shouldBeDestroyed));
    }

    IEnumerator CreateObjectRoutine(GameObject prefab, Transform target, bool shouldBeDestroyed)
    {
        Vector3 newPos = target.position;
        yield return new WaitForSeconds(delay);

        GameObject newObject = Instantiate(prefab, newPos, Quaternion.identity);

        if(shouldBeDestroyed){
            Destroy(newObject, 15);
        }
    }
}
