using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	[Range(40,60),SerializeField]
	public int fillPercentage;
	[SerializeField]
	private int x=50;
	[SerializeField]
	private int y=50;
	[SerializeField]
	public MultiDimensionalInt map;
	[SerializeField]
	public int smooth=1;

	public int wallThresholdSize = 30;
	public int roomThresholdSize = 30;

	void Start()
	{
		//x = 50;
		//y = 50;

		map=new MultiDimensionalInt(new int[x,y]);
		Generate ();
	}

	void Update()
	{


	}

	void Generate()
	{

		FillMap ();

		for (int i=0; i<smooth; i++) {
			SmoothMap();
		};
		ProcessMap ();
		int borderSize = 1;
		int[,] borderMap = new int[x + borderSize * 2, y + borderSize * 2];



		for (int i=0; i<borderMap.GetLength(0); i++) {
			for (int j=0; j<borderMap.GetLength(1); j++) {
				if(i>=borderSize && i<x+borderSize && j>=borderSize && j<y+borderSize)
				{
					borderMap[i,j] = map.array[i-borderSize,j-borderSize];
				}
				else
				{
					borderMap[i,j]=1;
				}
			}
		}
		MeshGenerator meshGenerator = GetComponent<MeshGenerator> ();
		map=new MultiDimensionalInt(borderMap);
		meshGenerator.GenerateMesh (borderMap, 1);
		//meshGenerator.GenerateMesh (map.array, 1);

	}

	void ProcessMap() {
		List<List<Coord>> wallRegions = GetRegions (1);

		
		foreach (List<Coord> wallRegion in wallRegions) {
			if (wallRegion.Count < wallThresholdSize) {
				foreach (Coord tile in wallRegion) {
					map.array[tile.tileX,tile.tileY] = 0;
				}
			}
		}
		
		List<List<Coord>> roomRegions = GetRegions (0);

		
		foreach (List<Coord> roomRegion in roomRegions) {
			if (roomRegion.Count < roomThresholdSize) {
				foreach (Coord tile in roomRegion) {
					map.array[tile.tileX,tile.tileY] = 1;
				}
			}
		}
	}
	
	List<List<Coord>> GetRegions(int tileType) {
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] mapFlags = new int[x,y];
		
		for (int xPos = 0; xPos < x; xPos ++) {
			for (int yPos = 0; yPos < y; yPos ++) {
				if (mapFlags[xPos,yPos] == 0 && map.array[xPos,yPos] == tileType) {
					List<Coord> newRegion = GetRegionTiles(xPos,yPos);
					regions.Add(newRegion);
					
					foreach (Coord tile in newRegion) {
						mapFlags[tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}
		
		return regions;
	}
	
	List<Coord> GetRegionTiles(int startX,int startY)
	{
		List<Coord> tiles = new List<Coord> ();
		int[,] flagsMap = new int[x, y];
		int tileType = map.array [startX, startY];//0,1

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord (startX, startY));
		flagsMap [startX, startY] = 1;
		while (queue.Count>0) {
			Coord tile = queue.Dequeue ();
			tiles.Add (tile);
			for (int xPos = tile.tileX - 1; xPos <= tile.tileX + 1; xPos++) {
				for (int yPos = tile.tileY - 1; yPos <= tile.tileY + 1; yPos++) {
					if (IsInMapRange (xPos, yPos) && (yPos == tile.tileY || xPos == tile.tileX)) {
						if (flagsMap [xPos, yPos] == 0 && map.array [xPos, yPos] == tileType) {
							flagsMap [xPos, yPos] = 1;
							queue.Enqueue (new Coord (xPos, yPos));
						}
					}
				}
			}
		}
		return tiles;
	}


	bool IsInMapRange(int xPos, int yPos) {
		return xPos >= 0 && xPos < x && yPos >= 0 && yPos < y;
	}
	
	void FillMap()
		{
			map.array = new int[x, y];
			RandomFillPercentage ();
			for (int i=0; i<x; i++) {
				for(int j=0;j<y;j++)
			{
				if(i==0 || j==0 || i==x-1 || j==y-1)
				{
					map.array[i,j]=1;
					continue;
				}
				int chance=Random.Range(40,60);
				//int chance=100;

				if(chance<=fillPercentage)
				{
					map.array[i,j]=1;//filled
				}
				else
				{
					map.array[i,j]=0;
				}
			}
		}

	}

	void RandomFillPercentage()
	{
		if (fillPercentage == 0) {
			fillPercentage=Random.Range(0,100);
		}
	}

	/*
	void OnDrawGizmos()
	{
		if (map.array != null) {

			for(int i=0;i<map.array.GetLength(0);i++)
			{
				for(int j=0;j<map.array.GetLength(1);j++)

				{
				Gizmos.color=(map.array[i,j]==0)?Color.white:Color.black;
					Vector3 position=new Vector3(-x/2.0f+i+0.5f,0.0f,-y/2.0f+j+0.5f);
					if(map.array[i,j]==1)
					Gizmos.DrawCube(position,Vector3.one);
				}
			}
		}
	}
*/
	void SmoothMap()
	{
	//	Debug.Log ("Smooth");
		if (map.array != null) {
			for (int i=0; i<map.array.GetLength(0); i++) {
				for (int j=0; j<map.array.GetLength(1); j++) {
					int n=GetSurroundingWallCount(i,j);

					if(n>5)
					{
						map.array[i,j]=1;
					}
					else if(n<3)
					{
						map.array[i,j]=0;

					}
				}

			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (neighbourX >= 0 && neighbourX < map.array.GetLength(0) && neighbourY >= 0 && neighbourY < map.array.GetLength(1)) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map.array[neighbourX,neighbourY];
					}
				}
				else {
					wallCount ++;
				}
			}
		}
		
		return wallCount;
	}

	struct Coord
	{
		public int tileX;
		public int tileY;

		public Coord(int xPos, int yPos)
		{
			tileX=xPos;
			tileY=yPos;

		}
	}

}



[System.Serializable]
public class MultiDimensionalInt
{
	[SerializeField]
	public int[,] array;

	public MultiDimensionalInt(int[,] map)
	{
		array = map;
	}

}
