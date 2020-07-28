using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Poems_instantiate : MonoBehaviour
{

    public TextAsset poem_text_file;
    public GameObject textfab;
    private GameObject[] texts;
    private int num_texts = 7;
    private float z_offset = 0;
    private float speed = 0.009f;
    private float scalar = 0.35f;
    private float height = 1.5f;
    private int num_poems = 0;
    private float[] poem_heights;
    private float poem_depth = 4.5f; // how far on the z axis the poems are spaced.

    // Audio-related
    private Transform listener;
    private AudioSource source;
    private float maxDistance = 80;
    private float initialVolume = 1;

    void Start()
    {
        // set the listener transform to the audio listener
        AudioListener tmp_listener = (AudioListener)FindObjectOfType(typeof(AudioListener));
        listener = tmp_listener.transform;

        // scale the overall height (distance between objects) using the scalar (the size of the objects)
        height = height * scalar / 0.05f;

        texts = new GameObject[num_texts];
        string poem_text = poem_text_file.text;
        string[] poem_array = poem_text.Split('*');
        num_poems = poem_array.Length;
        poem_heights = new float[num_texts];
        float poem_heights_total = 0;

        for (int i = 0; i < num_texts; i++)
        {
            texts[i] = Instantiate(textfab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            texts[i].transform.localRotation = Quaternion.Euler(90, 180, 0);
            texts[i].transform.parent = GameObject.Find("ImageTarget").transform;
            texts[i].transform.localScale = new Vector3(scalar, scalar, scalar);
            ScriptExample1 scr = texts[i].GetComponent<ScriptExample1>();

            texts[i].GetComponent<TextMeshPro>().SetText(poem_array[i%num_poems]);

            texts[i].GetComponent<TextMeshPro>().ForceMeshUpdate();

            float text_height = texts[i].GetComponent<TextMeshPro>().textBounds.size.y;
            poem_heights_total += text_height;
            poem_heights[i] = text_height;

            // set audio clip
            AudioSource audio_src = texts[i].GetComponent<AudioSource>();
            string audio_clip_name = "Climate_loop_" + i;
            AudioClip audio_clip = (AudioClip) Resources.Load(audio_clip_name);
            audio_src.clip = audio_clip;
            //audio_src.pitch = (i/(float)num_texts) + 1;
            audio_src.loop = true;
            audio_src.Play();
            //Debug.Log("audio clip for " + i + " is " + audio_src.clip.);
        }

        float poem_height_total_normalized = 0;
        float poem_height_total_previous = 0;
        for (int i = 0; i < num_texts; i++)
        {
            poem_height_total_previous = poem_heights[i] / poem_heights_total;
            poem_heights[i] = poem_height_total_normalized;
            poem_height_total_normalized += poem_height_total_previous;
        }

    }

    private void Update()
    {
        z_offset += Time.deltaTime * speed;

        for (int i = 0; i < num_texts; i++)
        {
            //float relative_offset = i / (float)num_texts;
            float relative_offset = 1.0f - poem_heights[i];

            float z_pos_normalized = (z_offset + relative_offset) % 1.0f;
            float z_pos = z_pos_normalized * -height + height / 2f;
            texts[i].transform.localPosition = new Vector3(0, i/(float)num_texts * poem_depth, z_pos);

            float dilation = Mathf.Cos(z_pos_normalized * Mathf.PI * 2.0f + Mathf.PI) * 0.5f - 0.5f;
            float audio_dilation = dilation + 1; // rescale dilation to be 0-1 for audio scaling
            Material text_mat = texts[i].GetComponent<TextMeshPro>().fontMaterial;
            text_mat.SetFloat("_FaceDilate", dilation);

            // set audio volume
            Transform text_transform = texts[i].GetComponent<Transform>();
            AudioSource text_audio_src = texts[i].GetComponent<AudioSource>();

            Vector3 distVec = text_transform.position - listener.position;
            //distVec.z = 0; // ignore the z axis
            float dist = distVec.magnitude;

            // use this to overwrite the 3D sound falloff
            //float falloff = Mathf.Clamp01(1.0f - (dist / maxDistance)) * audio_dilation;
            float falloff = audio_dilation;

            text_audio_src.volume = falloff;

        }
    }
}
