using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScriptExample1 : MonoBehaviour {

    public float phase_offset;
    private float distance_per_second = 1;
    private float elapsed_time = 0;
    private float loop_len_seconds = 37;

    private string src_text = "Human-induced warming reached approximately 1°C(±0.2°C likely range) above pre-industrial levels in 2017, increasing at 0.2°C(±0.1°C) per decade(high confidence). Global warming is defined in this report as an increase in combined surface air and sea surface temperatures averaged over the globe and a 30-year period.Unless otherwise specified, warming is expressed relative to the period 1850-1900, used as an approximation of pre-industrial temperatures in AR5.For periods shorter than 30 years, warming refers to the estimated average temperature over the 30 years centered on that shorter period, accounting for the impact of any temperature fluctuations or trend within those 30 years.Accordingly, warming up to the decade 2006-2015 is assessed at 0.87°C (±0.12°C likely range). Since 2000, the estimated level of human-induced warming has been equal to the level of observed warming with a likely range of ±20% accounting for uncertainty due to contributions from solar and volcanic activity over the historical period(high confidence).";

	// Use this for initialization
	void Start () {
        elapsed_time += loop_len_seconds * phase_offset;
        GetComponent<TextMeshPro>().SetText(src_text);
     }
	
	// Update is called once per frame
	void Update () {
        elapsed_time += distance_per_second * Time.deltaTime;
        elapsed_time %= loop_len_seconds;
        float frame_ratio = elapsed_time / loop_len_seconds;
        float nu_height = frame_ratio * 30 - 15;

        Transform tr = GetComponent<Transform>();
        Vector3 pos = tr.position;
        Quaternion rot = tr.rotation;
        rot.SetEulerRotation(Mathf.PI*0.5f, 0, 0);
        Vector3 new_pos = new Vector3(pos.x, nu_height, pos.z);
        tr.SetPositionAndRotation(new_pos, rot);

        Material mater = GetComponent<TextMeshPro>().fontMaterial;
        float dilation = Mathf.Cos(frame_ratio * Mathf.PI * 2f + Mathf.PI) * 0.5f - 0.5f;
        mater.SetFloat("_FaceDilate", dilation);
    }
}
