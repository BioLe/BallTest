using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class PlayerController : MonoBehaviour {

    public Rigidbody rb;
    public GameObject player;
    public float moveHorizontal, moveVertical, speed;

    // InputHorizontal , InputVertical, PosX, PosY
    private List<string[]> rowData = new List<string[]>();
    string[] rowDataTemp = new string[6];




    void Start () {

        rb = GetComponent<Rigidbody>();
        player = GetComponent<GameObject>();
        

        //Header csv
        rowDataTemp[0] = "InputHorizontal";
        rowDataTemp[1] = "InputVertical";
        rowDataTemp[2] = "PlayerPosX";
        rowDataTemp[3] = "PlayerPosY";
        rowDataTemp[4] = "PlayerPosZ";
        rowDataTemp[5] = "TimeDeltaTime";
        rowData.Add(rowDataTemp);

    }
	
	void Update () {
        
	}

    void FixedUpdate(){

        //movimento
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        rb.AddForce(movement * speed);

        addRowData(moveHorizontal, moveVertical, transform.position.x, transform.position.y, transform.position.z);

    }

    

    private void LateUpdate() {
        if (Input.GetKey("r")) resetPosition();
        if (Input.GetKey("g")) exportCSV();
    }

    void OnApplicationQuit() {

    }


    void resetPosition(){
        moveHorizontal = 0;
        moveVertical = 0;

        rb.isKinematic = true;
        
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = new Vector3(0f, 0.5f, 0f);
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        rb.isKinematic = false;

    }

    //Gravar valores da frame atual
    private void addRowData(float inputHorizontal, float inputVertical, float playerPositionX, float playerPositionY, float playerPositionZ) {
        rowDataTemp = new string[6];
        rowDataTemp[0] = "" + inputHorizontal;
        rowDataTemp[1] = "" + inputVertical;
        rowDataTemp[2] = "" + playerPositionX;
        rowDataTemp[3] = "" + playerPositionY; //quando existir variação quer dizer q colidiu com a parede
        rowDataTemp[4] = "" + playerPositionZ;
        rowDataTemp[5] = "" + Time.time;
        rowData.Add(rowDataTemp);
    }


    private void exportCSV() {
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++) {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        DateTime dt = System.DateTime.Now;
        string datetime = dt.ToString("yyyy-MM-dd_HH-mm-ss");

        String filePath = Application.dataPath + "/" + "data_" + datetime + ".csv";

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
}
