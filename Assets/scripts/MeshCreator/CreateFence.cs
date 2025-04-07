using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Handout {
	public class CreateFence : MonoBehaviour {
		public int numberOfPoles = 10;
		// The dimensions of a single step of the staircase:
		public float width=3;
		public float height=1;
		public float depth=1;
		public float triangleHeight = 0.1f;

		MeshBuilder builder;

		void Start () {
			builder = new MeshBuilder ();
			CreateShape ();
			GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
		}

		void CreateShape()
		{
			builder.Clear();

			// Create fence
			for (int i = 0; i < numberOfPoles; i++)
			{
				Vector3 offset = new Vector3(Mathf.Sin((i * Mathf.PI) / (numberOfPoles / 2f)), 0, Mathf.Cos((i * Mathf.PI) / (numberOfPoles / 2f)));
                Vector3 point = offset;
                point.z += depth / 2f;

                float degreesToRotate = -360 / numberOfPoles * i;

                // position vectors:
                Vector3 v1 = RotateAroundPoint(new Vector3(offset.x + width / 2f, offset.y, offset.z), point, degreesToRotate);
                Vector3 v2 = RotateAroundPoint(new Vector3(offset.x + -width / 2f, offset.y, offset.z), point, degreesToRotate);

                Vector3 v3 = RotateAroundPoint(new Vector3(offset.x + width / 2f, offset.y + height, offset.z), point, degreesToRotate);
                Vector3 v4 = RotateAroundPoint(new Vector3(offset.x + -width / 2f, offset.y + height, offset.z), point, degreesToRotate);

                Vector3 v5 = RotateAroundPoint(new Vector3(offset.x + width / 2f, offset.y, offset.z + depth), point, degreesToRotate);
                Vector3 v6 = RotateAroundPoint(new Vector3(offset.x + -width / 2f, offset.y, offset.z + depth), point, degreesToRotate);
                
                Vector3 v7 = RotateAroundPoint(new Vector3(offset.x + width / 2f, offset.y + height, offset.z + depth), point, degreesToRotate);
                Vector3 v8 = RotateAroundPoint(new Vector3(offset.x + -width / 2f, offset.y + height, offset.z + depth), point, degreesToRotate);

                Vector3 v9 = RotateAroundPoint(new Vector3(offset.x, offset.y + height + triangleHeight, offset.z), point, degreesToRotate);
                Vector3 v10 = RotateAroundPoint(new Vector3(offset.x, offset.y + height + triangleHeight, offset.z + depth), point, degreesToRotate);
                //----------------//

                // front:
                // bottom:
                int v1_1 = builder.AddVertex(v1, new Vector2(1, 0.5f));
                int v1_2 = builder.AddVertex(v1, new Vector2(0, 0.5f));

                int v2_1 = builder.AddVertex(v2, new Vector2(0, 0.5f));
                int v2_2 = builder.AddVertex(v2, new Vector2(0, 0.5f));
                int v2_3 = builder.AddVertex(v2, new Vector2(1, 0.5f));

                // top:
                int v3_1 = builder.AddVertex(v3, new Vector2(1, 0));
                int v3_2 = builder.AddVertex(v3, new Vector2(1, 0));
                int v3_3 = builder.AddVertex(v3, new Vector2(0, 0));
                int v3_4 = builder.AddVertex(v3, new Vector2(0, 0));
                int v3_5 = builder.AddVertex(v3, new Vector2(1, 0.5f));
                int v3_6 = builder.AddVertex(v3, new Vector2(0, 0.5f));

                int v4_1 = builder.AddVertex(v4, new Vector2(0, 0));
                int v4_2 = builder.AddVertex(v4, new Vector2(1, 0));
                int v4_3 = builder.AddVertex(v4, new Vector2(1, 0));
                int v4_4 = builder.AddVertex(v4, new Vector2(0, 0.5f));
                int v4_5 = builder.AddVertex(v4, new Vector2(1, 0.5f));
				//----------------//

                // back:
				// bottom:
                int v5_1 = builder.AddVertex(v5, new Vector2(0, 0.5f));
                int v5_2 = builder.AddVertex(v5, new Vector2(1, 0.5f));
                int v5_3 = builder.AddVertex(v5, new Vector2(1, 0.5f));

                int v6_1 = builder.AddVertex(v6, new Vector2(1, 0.5f));
                int v6_2 = builder.AddVertex(v6, new Vector2(1, 0.5f));
                int v6_3 = builder.AddVertex(v6, new Vector2(0, 0.5f));
                int v6_4 = builder.AddVertex(v6, new Vector2(0, 0.5f));

                // top:
                int v7_1 = builder.AddVertex(v7, new Vector2(0, 0));
                int v7_2 = builder.AddVertex(v7, new Vector2(0, 0));
                int v7_3 = builder.AddVertex(v7, new Vector2(1, 0));
                int v7_4 = builder.AddVertex(v7, new Vector2(0, 0.5f));
                int v7_5 = builder.AddVertex(v7, new Vector2(1, 0.5f));
                int v7_6 = builder.AddVertex(v7, new Vector2(1, 0.5f));

                int v8_1 = builder.AddVertex(v8, new Vector2(1, 0));
                int v8_2 = builder.AddVertex(v8, new Vector2(0, 0));
                int v8_3 = builder.AddVertex(v8, new Vector2(1, 0.5f));
                int v8_4 = builder.AddVertex(v8, new Vector2(0, 0.5f));
                int v8_5 = builder.AddVertex(v8, new Vector2(0, 0.5f));
                //----------------//

                // Triangle:
                int v9_1 = builder.AddVertex(v9, new Vector2(0.5f, 0));
				int v9_2 = builder.AddVertex(v9, new Vector2(0, 0));
				int v9_3 = builder.AddVertex(v9, new Vector2(0, 0));
				int v9_4 = builder.AddVertex(v9, new Vector2(1, 0));
				int v9_5 = builder.AddVertex(v9, new Vector2(1, 0));

				int v10_1 = builder.AddVertex(v10, new Vector2(0.5f, 0));
                int v10_2 = builder.AddVertex(v10, new Vector2(0, 0));
                int v10_3 = builder.AddVertex(v10, new Vector2(0, 0));
                //----------------//

                // front
                builder.AddTriangle(v1_1, v2_1, v3_1);
				builder.AddTriangle(v2_2, v4_1, v3_2);
				

				// back
				builder.AddTriangle(v7_1, v6_1, v5_1);
				builder.AddTriangle(v7_2, v8_1, v6_2);
				

				// sides
				builder.AddTriangle(v1_2, v3_3, v5_2);
				builder.AddTriangle(v3_4, v7_3, v5_3);
				builder.AddTriangle(v6_3, v4_2, v2_3);
				builder.AddTriangle(v6_4, v8_2, v4_3);

                // triangle
                builder.AddTriangle(v3_5, v4_4, v9_1);
                builder.AddTriangle(v7_4, v10_1, v8_3);

				builder.AddTriangle(v9_2, v7_5, v3_6);
				builder.AddTriangle(v9_3, v10_2, v7_6);

				builder.AddTriangle(v4_5, v8_4, v9_4);
				builder.AddTriangle(v8_5, v10_3, v9_5);
            }
		}
        private Vector3 RotateAroundPoint(Vector3 vectorToRotate, Vector3 point, float angle)
        {
            angle *= Mathf.Deg2Rad;

            Vector2 v = new Vector2(vectorToRotate.x - point.x, vectorToRotate.z - point.z);

            v = new Vector2(v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle), v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle));

            return new Vector3(v.x + point.x, vectorToRotate.y, v.y + point.z);
        }
	}
}