using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Handout {
	public class CreateArrows : MonoBehaviour {
        public Vector3 arrowZeroPoint;
		public float height=1;
		public float width=0.1f;
		public float arrowHeadHeight = 0.1f;
        public float arrowHeadWidth = 0.1f;

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

			// Create fence
			for (int i = 0; i < 3; i++)
			{
				Vector3 offset = new Vector3(0,0,0);

                float degreesToRotate = -360 / 1 * i;

                // position vectors:
                Vector3 v1 = new Vector3(offset.x + width / 2f, offset.y, offset.z + -width / 2f);
                Vector3 v2 = new Vector3(offset.x + -width / 2f, offset.y, offset.z + -width / 2f);

                Vector3 v3 = new Vector3(offset.x + width / 2f, offset.y + height, offset.z + -width / 2f);
                Vector3 v4 = new Vector3(offset.x + -width / 2f, offset.y + height, offset.z + -width / 2f);

                Vector3 v5 = new Vector3(offset.x + width / 2f, offset.y, offset.z + width / 2f);
                Vector3 v6 = new Vector3(offset.x + -width / 2f, offset.y, offset.z + width / 2f);
                
                Vector3 v7 = new Vector3(offset.x + width / 2f, offset.y + height, offset.z + width / 2f);
                Vector3 v8 = new Vector3(offset.x + -width / 2f, offset.y + height, offset.z + width / 2f);

                // Arrow head
                Vector3 v9 = new Vector3(offset.x, offset.y + height + arrowHeadHeight, offset.z);

                Vector3 v10 = new Vector3(offset.x + width / 2f + arrowHeadWidth / 2f, offset.y + height, offset.z + -width / 2f + - arrowHeadWidth / 2f);
                Vector3 v11 = new Vector3(offset.x + -width / 2f + -arrowHeadWidth / 2f, offset.y + height, offset.z + -width / 2f + - arrowHeadWidth / 2f);

                Vector3 v12 = new Vector3(offset.x + width / 2f + arrowHeadWidth / 2f, offset.y + height, offset.z + width / 2f + arrowHeadWidth / 2f);
                Vector3 v13 = new Vector3(offset.x + -width / 2f + -arrowHeadWidth / 2f, offset.y + height, offset.z + width / 2f + arrowHeadWidth / 2f);
                //----------------//

                // front:
                // bottom:
                int v1_1 = builder.AddVertex(v1, new Vector2(1, 1));
                int v1_2 = builder.AddVertex(v1, new Vector2(0, 1));
                int v1_3 = builder.AddVertex(v1, new Vector2(1, 0));

                int v2_1 = builder.AddVertex(v2, new Vector2(0, 1));
                int v2_2 = builder.AddVertex(v2, new Vector2(0, 1));
                int v2_3 = builder.AddVertex(v2, new Vector2(1, 1));
                int v2_4 = builder.AddVertex(v2, new Vector2(0, 0));
                int v2_5 = builder.AddVertex(v2, new Vector2(0, 0));

                // top:
                int v3_1 = builder.AddVertex(v3, new Vector2(1, 0));
                int v3_2 = builder.AddVertex(v3, new Vector2(1, 0));
                int v3_3 = builder.AddVertex(v3, new Vector2(0, 0));
                int v3_4 = builder.AddVertex(v3, new Vector2(0, 0));
                int v3_5 = builder.AddVertex(v3, new Vector2(0.75f, 0.25f));
                int v3_6 = builder.AddVertex(v3, new Vector2(0.75f, 0.25f));
                int v3_7 = builder.AddVertex(v3, new Vector2(0.75f, 0.25f));

                int v4_1 = builder.AddVertex(v4, new Vector2(0, 0));
                int v4_2 = builder.AddVertex(v4, new Vector2(1, 0));
                int v4_3 = builder.AddVertex(v4, new Vector2(1, 0));
                int v4_4 = builder.AddVertex(v4, new Vector2(0.25f, 0.25f));
                int v4_5 = builder.AddVertex(v4, new Vector2(0.25f, 0.25f));
                int v4_6 = builder.AddVertex(v4, new Vector2(0.25f, 0.25f));
				//----------------//

                // back:
				// bottom:
                int v5_1 = builder.AddVertex(v5, new Vector2(0, 1));
                int v5_2 = builder.AddVertex(v5, new Vector2(1, 1));
                int v5_3 = builder.AddVertex(v5, new Vector2(1, 1));
                int v5_4 = builder.AddVertex(v5, new Vector2(1, 1));
                int v5_5 = builder.AddVertex(v5, new Vector2(1, 1));

                int v6_1 = builder.AddVertex(v6, new Vector2(1, 1));
                int v6_2 = builder.AddVertex(v6, new Vector2(1, 1));
                int v6_3 = builder.AddVertex(v6, new Vector2(0, 1));
                int v6_4 = builder.AddVertex(v6, new Vector2(0, 1));
                int v6_5 = builder.AddVertex(v6, new Vector2(0, 0.5f));

                // top:
                int v7_1 = builder.AddVertex(v7, new Vector2(0, 0));
                int v7_2 = builder.AddVertex(v7, new Vector2(0, 0));
                int v7_3 = builder.AddVertex(v7, new Vector2(1, 0));
                int v7_4 = builder.AddVertex(v7, new Vector2(0.75f, 0.75f));
                int v7_5 = builder.AddVertex(v7, new Vector2(0.75f, 0.75f));
                int v7_6 = builder.AddVertex(v7, new Vector2(0.75f, 0.75f));

                int v8_1 = builder.AddVertex(v8, new Vector2(1, 0));
                int v8_2 = builder.AddVertex(v8, new Vector2(0, 0));
                int v8_3 = builder.AddVertex(v8, new Vector2(0.25f, 0.75f));
                int v8_4 = builder.AddVertex(v8, new Vector2(0.25f, 0.75f));
                int v8_5 = builder.AddVertex(v8, new Vector2(0.25f, 0.75f));
                //----------------//

                // ArrowHead:
                int v9_1 = builder.AddVertex(v9, new Vector2(0.5f, 0));
				int v9_2 = builder.AddVertex(v9, new Vector2(0.5f, 0));
				int v9_3 = builder.AddVertex(v9, new Vector2(0.5f, 0));
				int v9_4 = builder.AddVertex(v9, new Vector2(0.5f, 0));

				int v10_1 = builder.AddVertex(v10, new Vector2(1, 0.5f));
				int v10_2 = builder.AddVertex(v10, new Vector2(1, 0.5f));
				int v10_3 = builder.AddVertex(v10, new Vector2(1, 0));
				int v10_4 = builder.AddVertex(v10, new Vector2(1, 0));
				int v10_5 = builder.AddVertex(v10, new Vector2(1, 0));

                int v11_1 = builder.AddVertex(v11, new Vector2(0, 0.5f));
                int v11_2 = builder.AddVertex(v11, new Vector2(0, 0.5f));
                int v11_3 = builder.AddVertex(v11, new Vector2(0, 0));
                int v11_4 = builder.AddVertex(v11, new Vector2(0, 0));
                int v11_5 = builder.AddVertex(v11, new Vector2(0, 0));

                int v12_1 = builder.AddVertex(v12, new Vector2(0, 0.5f));
                int v12_2 = builder.AddVertex(v12, new Vector2(0, 0.5f));
                int v12_3 = builder.AddVertex(v12, new Vector2(1, 1));
                int v12_4 = builder.AddVertex(v12, new Vector2(1, 1));
                int v12_5 = builder.AddVertex(v12, new Vector2(1, 1));

                int v13_1 = builder.AddVertex(v13, new Vector2(1, 0.5f));
                int v13_2 = builder.AddVertex(v13, new Vector2(1, 0.5f));
                int v13_3 = builder.AddVertex(v13, new Vector2(0, 1));
                int v13_4 = builder.AddVertex(v13, new Vector2(0, 1));
                int v13_5 = builder.AddVertex(v13, new Vector2(0, 1));
                //----------------//

                // Bottom
                builder.AddTriangle(v5_4, v2_4, v1_3);
                builder.AddTriangle(v2_5, v5_5, v6_5);

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

                // ArrowHead
                builder.AddTriangle(v9_1, v10_1, v11_1);
                builder.AddTriangle(v9_2, v11_2, v13_1);
                builder.AddTriangle(v13_2, v12_1, v9_3);
                builder.AddTriangle(v9_4, v12_2, v10_2);

                builder.AddTriangle(v11_3, v10_3, v3_5);
                builder.AddTriangle(v3_6, v4_4, v11_4);

                builder.AddTriangle(v13_3, v11_5, v4_5);
                builder.AddTriangle(v13_4, v4_6, v8_3);

                builder.AddTriangle(v12_3, v13_5, v8_4);
                builder.AddTriangle(v12_4, v8_5, v7_4);

                builder.AddTriangle(v10_4, v12_5, v7_5);
                builder.AddTriangle(v10_5, v7_6, v3_7);
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