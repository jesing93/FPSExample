using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private int objectNumberOnStart;

    private List<GameObject> poolObjects = new List<GameObject>();

    private void Start()
    {
        //Instantiate the objects
        for (int i = 0; i < objectNumberOnStart; i++)
        {
            CreateNewObject();
        }
    }

    /// <summary>
    /// Instantiate object and add it to the list
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private GameObject CreateNewObject()
    {
        GameObject gameObject = Instantiate(prefabObject);
        gameObject.SetActive(false); //Deactivate
        poolObjects.Add(gameObject); //Add to the list

        return gameObject;
    }

    /// <summary>
    /// Take from the list an available object or create one there is none available
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject()
    {
        //Find in the poolObject an object that is Inactive in the game
        GameObject item = poolObjects.Find(x => x.activeInHierarchy == false);
        //If not found, create object
        if (item == null)
        {
            item = CreateNewObject();
        }
        //Activate the selected object
        item.SetActive(true);

        return item;
    }
}
