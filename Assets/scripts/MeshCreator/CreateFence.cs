using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Handout {
	public class CreateMesh : MonoBehaviour {
        [SerializeField] private float brickSize = 1;
        [SerializeField] private int pyramidLayers = 4;
		MeshBuilder builder;
		void Start () {
			builder = new MeshBuilder ();
			CreateShape ();
			GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
		}
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                builder = new MeshBuilder();
                CreateShape();
                GetComponent<MeshFilter>().mesh = builder.CreateMesh(true);
            }
        }
		void CreateShape()
		{
			builder.Clear();

			// Create Pyramid
			for (int i = 1; i <= pyramidLayers; i++)
			{
                float length = brickSize * i - 1;
                float bricksInRow = i;

                for (int o = 0; o < bricksInRow; o++)
                {
                    for (int p = 0; p < bricksInRow; p++)
                    {
                        Vector3 offset = new Vector3(-length / 2f, brickSize * pyramidLayers, -length / 2f);

                        offset.x += brickSize * o;
                        offset.y -= brickSize * i;
                        offset.z += brickSize * p;

                        CreateBrick(offset);
                    }
                }                
            }
		}
		void CreateBrick(Vector3 offset)
		{
            // position vectors:
            // Front
            Vector3 vec1 = new Vector3(offset.x + brickSize / 2f, offset.y, offset.z - brickSize / 2f);
            Vector3 vec2 = new Vector3(offset.x - brickSize / 2f, offset.y, offset.z - brickSize / 2f);
            Vector3 vec3 = new Vector3(offset.x + brickSize / 2f, offset.y + brickSize, offset.z - brickSize / 2f);
            Vector3 vec4 = new Vector3(offset.x - brickSize / 2f, offset.y + brickSize, offset.z - brickSize / 2f);

            // Back
            Vector3 vec5 = new Vector3(offset.x + brickSize / 2f, offset.y, offset.z + brickSize / 2f);
            Vector3 vec6 = new Vector3(offset.x - brickSize / 2f, offset.y, offset.z + brickSize / 2f);
            Vector3 vec7 = new Vector3(offset.x + brickSize / 2f, offset.y + brickSize, offset.z + brickSize / 2f);
            Vector3 vec8 = new Vector3(offset.x - brickSize / 2f, offset.y + brickSize, offset.z + brickSize / 2f);
            //----------------//

            // Front
            int v1_1 = builder.AddVertex(vec1, new Vector2(1, 1));
            int v1_2 = builder.AddVertex(vec1, new Vector2(0, 1));
            int v1_3 = builder.AddVertex(vec1, new Vector2(0, 1));
            int v1_4 = builder.AddVertex(vec1, new Vector2(1, 1));

            int v2_1 = builder.AddVertex(vec2, new Vector2(0, 1));
            int v2_2 = builder.AddVertex(vec2, new Vector2(0, 1));
            int v2_3 = builder.AddVertex(vec2, new Vector2(1, 1));
            int v2_4 = builder.AddVertex(vec2, new Vector2(0, 1));
            int v2_5 = builder.AddVertex(vec2, new Vector2(0, 1));

            int v3_1 = builder.AddVertex(vec3, new Vector2(1, 0));
            int v3_2 = builder.AddVertex(vec3, new Vector2(1, 0));
            int v3_3 = builder.AddVertex(vec3, new Vector2(0, 0));
            int v3_4 = builder.AddVertex(vec3, new Vector2(0, 0));
            int v3_5 = builder.AddVertex(vec3, new Vector2(1, 1));

            int v4_1 = builder.AddVertex(vec4, new Vector2(0, 0));
            int v4_2 = builder.AddVertex(vec4, new Vector2(0, 0));
            int v4_3 = builder.AddVertex(vec4, new Vector2(1, 0));
            int v4_4 = builder.AddVertex(vec4, new Vector2(1, 0));
            int v4_5 = builder.AddVertex(vec4, new Vector2(0, 1));
            int v4_6 = builder.AddVertex(vec4, new Vector2(0, 1));


            // Back
            int v5_1 = builder.AddVertex(vec5, new Vector2(1, 1));
            int v5_2 = builder.AddVertex(vec5, new Vector2(1, 1));
            int v5_3 = builder.AddVertex(vec5, new Vector2(0, 1));
            int v5_4 = builder.AddVertex(vec5, new Vector2(1, 0));
            int v5_5 = builder.AddVertex(vec5, new Vector2(1, 0));

            int v6_1 = builder.AddVertex(vec6, new Vector2(0, 1));
            int v6_2 = builder.AddVertex(vec6, new Vector2(0, 1));
            int v6_3 = builder.AddVertex(vec6, new Vector2(1, 1));
            int v6_4 = builder.AddVertex(vec6, new Vector2(1, 1));
            int v6_5 = builder.AddVertex(vec6, new Vector2(0, 0));

            int v7_1 = builder.AddVertex(vec7, new Vector2(1, 0));
            int v7_2 = builder.AddVertex(vec7, new Vector2(0, 0));
            int v7_3 = builder.AddVertex(vec7, new Vector2(0, 0));
            int v7_4 = builder.AddVertex(vec7, new Vector2(1, 0));
            int v7_5 = builder.AddVertex(vec7, new Vector2(1, 0));

            int v8_1 = builder.AddVertex(vec8, new Vector2(0, 0));
            int v8_2 = builder.AddVertex(vec8, new Vector2(1, 0));
            int v8_3 = builder.AddVertex(vec8, new Vector2(0, 0));
            //----------------//

            // Front
            builder.AddTriangle(v1_1, v2_1, v3_1);
            builder.AddTriangle(v3_2, v2_2, v4_2);

            // Left
            builder.AddTriangle(v6_1, v4_3, v2_3);
            builder.AddTriangle(v4_4, v6_2, v8_1);

            // Right
            builder.AddTriangle(v1_2, v3_3, v5_1);
            builder.AddTriangle(v7_1, v5_2, v3_4);

            // Back
            builder.AddTriangle(v7_2, v6_3, v5_3);
            builder.AddTriangle(v8_2, v6_4, v7_3);

            // Top
            builder.AddTriangle(v3_5, v4_5, v7_4);
            builder.AddTriangle(v8_3, v7_5, v4_6);

            // Bottom
            builder.AddTriangle(v5_4, v2_4, v1_4);
            builder.AddTriangle(v2_5, v5_5, v6_5);
        }
	}
}