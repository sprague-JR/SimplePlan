using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexMaker : MonoBehaviour
{
	// Start is called before the first frame update
	float size = 3f;
	int resolution = 1;

	public float navRadius = 0.45f;
	public LayerMask lm;
	private Triangulation tri;
	

    void Awake()
    {

		tri = GetComponentInParent<Triangulation>();

		Vector3 origin = transform.position + (Vector3.up * 1.1f);

		//origin = origin + (Vector3.forward * 1.5f) + (Vector3.right * 1.5f);

		for (int j = 0; j < 4; j++)
		{
			Vector3 offset = new Vector3( j%2 == 0 ? ( 2f * navRadius + 0.05f) : size - (2f * navRadius + 0.05f), 0, j < 2 ? (2f * navRadius + 0.05f) : size - ( 2f * navRadius + 0.05f));
			bool[] hits = new bool[8];

			for (int i = 0; i < 8; i++)
			{

				//rotation base indicates how many directions the node checks for obstructions in
				Vector3 dir = new Vector3(Mathf.Sin((Mathf.PI * 2 * i) / 8), 0, Mathf.Cos((Mathf.PI * i * 2) / 8)) * (navRadius * 2.0f);


				if (Physics.CapsuleCast((origin + offset) - Vector3.up * 0.5f, (origin + offset) + Vector3.up * 0.5f, navRadius, dir, (navRadius * 2.0f), lm))
				{
					//Debug.DrawRay((origin + offset), dir, Color.green, Mathf.Infinity, false);
					hits[i] = true;
				}
				else
				{
					//Debug.DrawRay((origin + offset), dir, Color.red, Mathf.Infinity, false);
					hits[i] = false;
				}


			}

			for(int i = 0; i < 8; i++)
			{
				if (hits[i])
					continue;

				int consecutive = 1;

				while(hits[(i + consecutive) % 8])
				{
					consecutive++;
				}

				if(consecutive != 1 && (consecutive != 4 || i%2 == 1))
				{
					offset = new Vector3(j % 2 == 0 ? (2f * navRadius + 0.05f) : size - (2f * navRadius + 0.05f), 0, j < 2 ? (2f * navRadius + 0.05f) : size - (2f * navRadius + 0.05f));
					Debug.DrawRay(transform.position + offset, Vector3.up * 2.0f,Color.yellow,Mathf.Infinity);

					if (tri != null)
						tri.AddCorner(transform.position + offset + Vector3.up);
					else
						Debug.Log("no triangulation found in parent, vertex discarded");
					break;

				}
			}
		}
    }

}
