using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour {

	private static MeshGenerator instance;
	public static MeshGenerator Instance
	{
		get
		{
			if (instance == null) {
				instance=GameObject.FindObjectOfType<MeshGenerator>() as MeshGenerator;
			}
			return instance;
		}
	}

	public MeshFilter insideWalls;
	public MeshFilter outsideWalls;
	public MeshFilter floor;

	public MeshFilter northWall;
	public MeshFilter southWall;
	public MeshFilter westWall;
	public MeshFilter eastWall;

	public SquareGrid squareGrid;
	public float wallHeight = 100;
	List<Vector3> vertices;
	List<int> triangles;
	Dictionary<int, List<Triangle>> triangleDictionary =new Dictionary<int, List<Triangle>>();
	List<List<int>> edges=new List<List<int>>();
	HashSet<int> checkedVertices = new HashSet<int> ();

	public delegate void OnMeshCreatedDelegate();
	public event OnMeshCreatedDelegate OnMeshCreated; 

	public void Awake ()
	{

	}

	struct Triangle
	{
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;
		int[] vertices;

		public Triangle(int a, int b, int c)
		{
			vertexIndexA=a;
			vertexIndexB=b;
			vertexIndexC=c;

			vertices=new int[3];
			vertices[0]=a;
			vertices[1]=b;
			vertices[2]=c;

		}

		public int this[int i]
		{
			get{return vertices[i];}
		}

		public bool Contains(int vertexIndex)
		{
			return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
		}
	}

	public void GenerateMesh(int[,] map, float squareSize)
	{
		edges.Clear ();
		checkedVertices.Clear ();
		triangleDictionary.Clear ();

		squareGrid = new SquareGrid (map, squareSize);
		vertices = new List<Vector3> ();
		triangles = new List<int> ();
		for(int i=0;i<squareGrid.squares.GetLength(0);i++)
		{
			for(int j=0;j<squareGrid.squares.GetLength(1);j++)
			{
				TriangulateSquare(squareGrid.squares[i,j]);
			}
		}

		Mesh mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = mesh;
		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.RecalculateNormals ();

		CreateWallMesh ();
		CreateFloorMesh (squareGrid);

		OnMeshCreated ();
	}

	void CreateFloorMesh (SquareGrid grid)
	{


		Mesh m = new Mesh ();
		m.vertices = new Vector3[] {
			new Vector3(grid.squares[0,0].topLeft.position.x, -wallHeight, grid.squares[0,0].topLeft.position.z),
			new Vector3(grid.squares[grid.squares.GetLength(0)-1,0].topRight.position.x, -wallHeight, grid.squares[grid.squares.GetLength(0)-1,0].topRight.position.z),
			new Vector3(grid.squares[grid.squares.GetLength(0)-1,grid.squares.GetLength(1)-1].bottomRight.position.x, -wallHeight, grid.squares[grid.squares.GetLength(0)-1,grid.squares.GetLength(1)-1].bottomRight.position.z),
			new Vector3(grid.squares[0,grid.squares.GetLength(1)-1].bottomLeft.position.x, -wallHeight, grid.squares[0,grid.squares.GetLength(1)-1].bottomLeft.position.z),
		};

		m.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2(1, 1),
			new Vector2 (1, 0)
		};
		m.triangles = new int[] { 0, 1, 2, 0, 2, 3};
		m.triangles = new int[] { 3, 2, 0, 2, 1, 0};
		m.RecalculateNormals();
		floor.mesh = m;


	}





	void CreateWallMesh() {
		CalculateMeshOutlines ();

		
		List<Vector3> wallVertices = new List<Vector3> ();
		List<int> wallTriangles = new List<int> ();
		Mesh wallMesh = new Mesh ();
		Mesh outsideWallsMesh = new Mesh ();

		
		foreach (List<int> outline in edges) {
			for (int i = 0; i < outline.Count -1; i ++) {
				int startIndex = wallVertices.Count;
				wallVertices.Add(vertices[outline[i]]); // left
				wallVertices.Add(vertices[outline[i+1]]); // right
				wallVertices.Add(vertices[outline[i]] - Vector3.up * wallHeight); // bottom left
				wallVertices.Add(vertices[outline[i+1]] - Vector3.up * wallHeight); // bottom right
				
				wallTriangles.Add(startIndex + 0);
				wallTriangles.Add(startIndex + 2);
				wallTriangles.Add(startIndex + 3);
				
				wallTriangles.Add(startIndex + 3);
				wallTriangles.Add(startIndex + 1);
				wallTriangles.Add(startIndex + 0);
			}
		}



			Mesh westMesh = new Mesh();
			westMesh.vertices = new Vector3[] {
			new Vector3(squareGrid.squares[0,0].topLeft.position.x,-wallHeight, squareGrid.squares[0,0].topLeft.position.z-squareGrid.size),
			new Vector3(squareGrid.squares[0,squareGrid.squares.GetLength(1)-1].topLeft.position.x,-wallHeight, squareGrid.squares[0,squareGrid.squares.GetLength(1)-1].bottomLeft.position.z+squareGrid.size),

			new Vector3(squareGrid.squares[0,squareGrid.squares.GetLength(1)-1].bottomLeft.position.x,0, squareGrid.squares[0,squareGrid.squares.GetLength(1)-1].bottomLeft.position.z+squareGrid.size),
			new Vector3(squareGrid.squares[0,0].topLeft.position.x,0, squareGrid.squares[0,0].topLeft.position.z-squareGrid.size)

			};

		westMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3};
		westMesh.RecalculateNormals();
		westWall.mesh = westMesh;




			Mesh eastMesh = new Mesh();
			eastMesh.vertices = new Vector3[] {

			new Vector3(squareGrid.squares[squareGrid.squares.GetLength(0)-1,0].topLeft.position.x+squareGrid.size,-wallHeight, squareGrid.squares[0,0].topLeft.position.z-squareGrid.size),
			new Vector3(squareGrid.squares[squareGrid.squares.GetLength(0)-1,squareGrid.squares.GetLength(1)-1].topLeft.position.x+squareGrid.size,-wallHeight, squareGrid.squares[0,squareGrid.squares.GetLength(1)-1].bottomLeft.position.z+squareGrid.size),
			
			new Vector3(squareGrid.squares[squareGrid.squares.GetLength(0)-1,squareGrid.squares.GetLength(1)-1].bottomLeft.position.x+squareGrid.size,0, squareGrid.squares[0,squareGrid.squares.GetLength(1)-1].bottomLeft.position.z+squareGrid.size),
			new Vector3(squareGrid.squares[squareGrid.squares.GetLength(0)-1,0].topLeft.position.x+squareGrid.size,0, squareGrid.squares[0,0].topLeft.position.z-squareGrid.size)

		};
		
		eastMesh.triangles = new int[] { 3, 2, 0, 2, 1, 0};
		eastMesh.RecalculateNormals();
		eastWall.mesh = eastMesh;





		Mesh southMesh = new Mesh();
		southMesh.vertices = new Vector3[] {
			
			new Vector3(squareGrid.squares[0,0].topLeft.position.x,0, squareGrid.squares[0,0].bottomLeft.position.z),
			new Vector3(squareGrid.squares[0,0].topLeft.position.x,-wallHeight, squareGrid.squares[0,0].bottomLeft.position.z),
			
			new Vector3(squareGrid.squares[squareGrid.squares.GetLength(0)-1,squareGrid.squares.GetLength(1)-1].bottomLeft.position.x+squareGrid.size,-wallHeight, squareGrid.squares[0,0].bottomLeft.position.z),
			new Vector3(squareGrid.squares[squareGrid.squares.GetLength(0)-1,squareGrid.squares.GetLength(1)-1].bottomLeft.position.x+squareGrid.size,0, squareGrid.squares[0,0].bottomLeft.position.z),

		};
		
		southMesh.triangles = new int[]{ 3, 2, 0, 2, 1, 0};
		southMesh.RecalculateNormals();
		southWall.mesh = southMesh;





		Mesh northMesh = new Mesh();
		northMesh.vertices = new Vector3[] {
			
			new Vector3(squareGrid.squares[0,0].topLeft.position.x,0, squareGrid.squares[0,squareGrid.squares.GetLength(1)-1].bottomLeft.position.z+squareGrid.size),
			new Vector3(squareGrid.squares[0,0].topLeft.position.x,-wallHeight, squareGrid.squares[0,squareGrid.squares.GetLength(1)-1].bottomLeft.position.z+squareGrid.size),
			
			new Vector3(squareGrid.squares[squareGrid.squares.GetLength(0)-1,squareGrid.squares.GetLength(1)-1].bottomLeft.position.x+squareGrid.size,-wallHeight, squareGrid.squares[0,squareGrid.squares.GetLength(1)-1].bottomLeft.position.z+squareGrid.size),
			new Vector3(squareGrid.squares[squareGrid.squares.GetLength(0)-1,squareGrid.squares.GetLength(1)-1].bottomLeft.position.x+squareGrid.size,0, squareGrid.squares[0,squareGrid.squares.GetLength(1)-1].bottomLeft.position.z+squareGrid.size),
			
		};
		
		northMesh.triangles = new int[]{ 0, 1, 2, 0, 2, 3};
		northMesh.RecalculateNormals();
		northWall.mesh = northMesh;





		wallMesh.vertices = wallVertices.ToArray ();
		wallMesh.triangles = wallTriangles.ToArray ();
		insideWalls.mesh = wallMesh;

	}

	void TriangulateSquare(Square square)
	{
		switch (square.configuration) {

		case 0:
			break;
		case 1:
			MeshFromPoints(square.left,square.bottom, square.bottomLeft);
			break;
		case 2:
			MeshFromPoints(square.bottomRight, square.bottom, square.right);
			break;
		case 4:
			MeshFromPoints(square.topRight, square.right, square.top);
			break;
		case 8:
			MeshFromPoints(square.topLeft, square.top, square.left);
			break;
		case 3:
			MeshFromPoints(square.right, square.bottomRight, square.bottomLeft, square.left);
			break;
		case 6:
			MeshFromPoints(square.top, square.topRight, square.bottomRight, square.bottom);
			break;
		case 9:
			MeshFromPoints(square.topLeft, square.top, square.bottom, square.bottomLeft);
			break;
		case 12:
			MeshFromPoints(square.topLeft, square.topRight, square.right, square.left);
			break;
		case 5:
			MeshFromPoints(square.top, square.topRight, square.right, square.bottom, square.bottomLeft, square.left);
			break;
		case 10:
			MeshFromPoints(square.topLeft, square.top, square.right, square.bottomRight, square.bottom, square.left);
			break;
		case 7:
			MeshFromPoints(square.top, square.topRight, square.bottomRight, square.bottomLeft, square.left);
			break;
		case 11:
			MeshFromPoints(square.topLeft, square.top, square.right, square.bottomRight, square.bottomLeft);
			break;
		case 13:
			MeshFromPoints(square.topLeft, square.topRight, square.right, square.bottom, square.bottomLeft);
			break;
		case 14:
			MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottom, square.left);
			break;
		case 15:
			MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
			checkedVertices.Add(square.topLeft.vertexIndex);
			checkedVertices.Add(square.topRight.vertexIndex);

			checkedVertices.Add(square.bottomRight.vertexIndex);

			checkedVertices.Add(square.bottomLeft.vertexIndex);

			break;
		
		}
	}

	void MeshFromPoints(params Node[] points)
	{
		AssignVertices (points);
		if (points.Length >= 3) {
			if (points.Length >= 3)
				CreateTriangle(points[0], points[1], points[2]);
			if (points.Length >= 4)
				CreateTriangle(points[0], points[2], points[3]);
			if (points.Length >= 5) 
				CreateTriangle(points[0], points[3], points[4]);
			if (points.Length >= 6)
				CreateTriangle(points[0], points[4], points[5]);
		}

	}

	void AssignVertices(Node[] points)
	{
		for (int i=0; i<points.Length; i++) {
			if(points[i].vertexIndex==-1)
			{
				points[i].vertexIndex=vertices.Count;
				vertices.Add(points[i].position);
			}
		}
	}


	
void CreateTriangle(Node a, Node b, Node c)
	{

		triangles.Add (a.vertexIndex);
		triangles.Add (b.vertexIndex);
		triangles.Add (c.vertexIndex);

		Triangle triangle = new Triangle (a.vertexIndex, b.vertexIndex, c.vertexIndex);
		AddTriangle (triangle.vertexIndexA, triangle);
		AddTriangle (triangle.vertexIndexB, triangle);
		AddTriangle (triangle.vertexIndexC, triangle);

	}

	void AddTriangle(int vertexIndex, Triangle triangle)
	{
		if (triangleDictionary.ContainsKey (vertexIndex)) {
			triangleDictionary [vertexIndex].Add (triangle);
		} else {
			List<Triangle> triangleList=new List<Triangle>();
			triangleList.Add(triangle);
			triangleDictionary.Add (vertexIndex, triangleList);
		}
	}

	void CalculateMeshOutlines()
	{
		for (int vertexIndex=0; vertexIndex<vertices.Count; vertexIndex++) {
			if(!checkedVertices.Contains(vertexIndex))
			{
				int newOutlineVertex=GetConnectedEdgeVertex(vertexIndex);
				if(newOutlineVertex!=-1)
				{
					checkedVertices.Add(vertexIndex); 

					List<int> newOutline=new List<int>();
					newOutline.Add(vertexIndex);
					edges.Add (newOutline);
					FollowOutline(newOutlineVertex,edges.Count-1);
					edges[edges.Count-1].Add(vertexIndex);
				}
			}
		}
}


	void FollowOutline(int vertexIndex, int outlineIndex) {
		edges [outlineIndex].Add (vertexIndex);
		checkedVertices.Add (vertexIndex);
		int nextVertexIndex = GetConnectedEdgeVertex (vertexIndex);
		
		if (nextVertexIndex != -1) {
			FollowOutline(nextVertexIndex, outlineIndex);
		}
	}

	int GetConnectedEdgeVertex(int vertexIndex)
	{
		List<Triangle> triangleContainingVertex = triangleDictionary [vertexIndex];
		for (int i=0; i<triangleContainingVertex.Count; i++) {
			Triangle triangle=triangleContainingVertex[i];
			for(int j=0;j<3;j++)
			{
				int vertexB=triangle[j];
				if(vertexB!=vertexIndex && !checkedVertices.Contains(vertexB))
				if(isEdge(vertexIndex,vertexB))
				{
					return vertexB;
				}
			}
		} 
		return -1;
	}

	bool isEdge(int vertexA, int vertexB)
	{
		List<Triangle> triangleA = triangleDictionary [vertexA]; //triangles that contain vertexA
		int sharedTriangles = 0;
		for (int i=0; i<triangleA.Count; i++) {
			if(triangleA[i].Contains(vertexB))
			{
				sharedTriangles++;
				if(sharedTriangles>1)
				{
					break;
				}
			}
		}
		return sharedTriangles == 1;
	}


	public class SquareGrid
	{
		public Square[,] squares;
		public float size;
		public SquareGrid(int[,] map, float squareSize)
		{
			size=squareSize;
			 int nodeCountX=map.GetLength(0);
			int nodeCountY=map.GetLength(1);

			float mapWidth=nodeCountX*squareSize;
			float mapHeight=nodeCountY*squareSize;

			ControlNode[,] controlNodes=new ControlNode[nodeCountX,nodeCountY];

			for(int i=0;i<nodeCountX;i++)
			{
				for(int j=0;j<nodeCountY;j++)
				{
					Vector3 pos=new Vector3(-mapWidth/2+i*squareSize +squareSize/2,0,-mapHeight/2+j*squareSize+ squareSize/2);
					//Vector3 pos=new Vector3(-mapWidth/2+i*squareSize ,0,-mapHeight/2+j*squareSize);
					controlNodes[i,j]=new ControlNode(pos,map[i,j]==1,squareSize);
				}
			}
		
			squares=new Square[nodeCountX-1,nodeCountY-1];
			for(int i=0;i<nodeCountX-1;i++)
			{
				for(int j=0;j<nodeCountY-1;j++)
				{
					squares[i,j]=new Square(controlNodes[i,j+1],controlNodes[i+1,j+1],controlNodes[i+1,j],controlNodes[i,j]);
				}
			}
		}
	}

	public class Square
	{
		public ControlNode topLeft, topRight, bottomLeft, bottomRight;
		public Node top, right, left, bottom;
		public int configuration;

		public Square(ControlNode topL,ControlNode topR,ControlNode botR,ControlNode botL)
		{
			topLeft=topL;
			topRight=topR;
			bottomLeft=botL;
			bottomRight=botR;
			top=topLeft.rightNode;
			bottom=bottomLeft.rightNode;
			left=bottomLeft.aboveNode;
			right=bottomRight.aboveNode;

			if(topLeft.isActive)
				configuration+=8;
			if(topRight.isActive)
				configuration+=4;
			if(bottomRight.isActive)
				configuration+=2;
			if(bottomLeft.isActive)
				configuration+=1;
		}
	}

	public class Node
	{
		public Vector3 position;
		public int vertexIndex = -1;
		public Node(Vector3 pos)
		{
			position=pos;
		}
	}

	public class ControlNode: Node
	{
		public bool isActive;
		public Node aboveNode, rightNode;

		public ControlNode(Vector3 pos, bool active, float squareSize):base(pos)
		{
			isActive=active;
			aboveNode=new Node(position+Vector3.forward*squareSize/2f);
			rightNode=new Node(position+Vector3.right*squareSize/2f);

		}
	}
public void MeshFloor()
{
	
}

}
