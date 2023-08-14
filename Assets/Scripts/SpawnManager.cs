using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate the player prefab when he join the scene
        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(Random.Range(-3f,5f), 2f, 0f), Quaternion.identity, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
