using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    private Rigidbody rigidbody;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float forcejump = 400;
    [SerializeField] private TextMeshProUGUI namePlayer;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    // Start is called before the first frame update
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        namePlayer.text = photonView.Owner.NickName;
        if (photonView.IsMine)
            namePlayer.color = Color.green;
    }
    // Update is called once per frame
    void Update()
    {
        //Player movement (Check if the player is mine)
        if (photonView.IsMine)
        {
            Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
            velocity.y = rigidbody.velocity.y;
            rigidbody.velocity = velocity;
            if (Input.GetKeyDown(KeyCode.Space)) rigidbody.AddForce(Vector3.up * forcejump);
        }

    }
 

    public void OnCollisionEnter(Collision collision)
    {


        //Switch between scene when we collide with floor with the tag switch level
        if (collision.gameObject.tag == "switchlevel")
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
                PhotonNetwork.LoadLevel("GameRoom2");
            else
                PhotonNetwork.LoadLevel("GameRoom");

        }
    }
}
  
