using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInstantiate : MonoBehaviour {

    public TextAsset blue_text_file;
    public GameObject textfab;
    private GameObject[] texts;
    private int num_texts = 20;
    private float z_offset = 0;
    private float speed = 0.03f;
    private float scalar = 1f;
    private float height = 10f;

    void Start()
    {
        texts = new GameObject[num_texts];
        string blue_text = blue_text_file.text;

        for (int i = 0; i < num_texts; i++)
        {

            texts[i] = Instantiate(textfab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            texts[i].transform.localRotation = Quaternion.Euler(180, 180, 0);
            texts[i].transform.parent = GameObject.Find("ImageTarget").transform;
            texts[i].transform.localScale = new Vector3(scalar, scalar, scalar);
            ScriptExample1 scr = texts[i].GetComponent<ScriptExample1>();

            texts[i].GetComponent<TextMeshPro>().SetText(blue_text);
        }
        
    }

    private void Update()
    {
        z_offset += Time.deltaTime * speed;

        for(int i = 0; i < num_texts; i++)
        {
            float relative_offset = i / (float)num_texts;

            float z_pos_normalized = (z_offset + relative_offset) % 1.0f;
            float z_pos = z_pos_normalized * -height + height/2f;
            texts[i].transform.localPosition = new Vector3(0, 0, z_pos);

            float dilation = Mathf.Cos(z_pos_normalized * Mathf.PI * 2.0f + Mathf.PI) * 0.5f - 0.5f;
            Material text_mat = texts[i].GetComponent<TextMeshPro>().fontMaterial;
            text_mat.SetFloat("_FaceDilate", dilation);

        }
    }
}
