using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGen : MonoBehaviour {

	[Range(0,100)]
	public int randomFillPercent; //Odds of terrtain per square

	public int width;
	public int height;

	public string seed;//Seed
	public bool randomSeed;//Toggle seed being random
    public int tunnelSize = 1;//Tunnel radius

	int[,] map;

	void Start(){
		GenerateMap();
	}

	void Update(){
		if(Input.GetMouseButtonDown(0)){
			GenerateMap();
		}
	}

	//go through the steps of creating a map
	void GenerateMap(){
		map = new int[width,height];
		RandomFillMap();//randomize

		for(int i = 0; i < 5; i++){
			SmoothMap();//less random
		}

        FilterSpaces();
			
		int borderSize = 5;
		int[,] mapBorder = new int[width + borderSize * 2,height + borderSize * 2];
        //Create a border around the map
		for(int x = 0; x < mapBorder.GetLength(0); x ++){
			for(int y = 0; y < mapBorder.GetLength(1); y ++){
				if(x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize){
					mapBorder[x,y] = map[x-borderSize,y-borderSize];
				}
				else{
					mapBorder[x,y] = 1;
				}
			}
		}


		MeshGen meshGenr = GetComponent<MeshGen>();
		meshGenr.GenerateMesh(mapBorder,1);
	}

    //Filter random walls in open space, and random patches of space in walls
    void FilterSpaces(){
        List<List<Coord>> wallRegions = GetRegions(1);
        int wallThreshold = 50;
        foreach (List<Coord> wallRegion in wallRegions){
            if(wallRegion.Count < wallThreshold){
                foreach(Coord tile in wallRegion){
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }

        List<List<Coord>> gapRegions = GetRegions(0);
        int gapThreshold = 50;
        List<Room> remainingRooms = new List<Room>();

        foreach (List<Coord> gapRegion in gapRegions){
            if(gapRegion.Count < gapThreshold){
                foreach(Coord tile in gapRegion){
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
            else{
                remainingRooms.Add(new Room(gapRegion, map));
            }
        }
            remainingRooms.Sort();
            remainingRooms[0].isMainRoom = true;
            remainingRooms[0].isAccessibleFromMainRoom = true;

        
        ConnectClosestRooms(remainingRooms);
    }


    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false){

        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if(forceAccessibilityFromMainRoom){
            foreach(Room room in allRooms){
                if(room.isAccessibleFromMainRoom){
                    roomListB.Add(room);
                }
                else{
                    roomListA.Add(room);
                }
            }
        }
        else{
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDis = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach(Room roomA in roomListA){
            if(!forceAccessibilityFromMainRoom){
                 possibleConnectionFound = false;
                if(roomA.connectedRooms.Count > 0){
                    continue;
                }
            }

            foreach(Room roomB in roomListB){
                if(roomA == roomB || roomA.IsConnect(roomB)){
                    continue;
                }
               
                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++){
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++){
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX,2) + Mathf.Pow(tileA.tileY - tileB.tileY,2));
                    
                        if (distanceBetweenRooms < bestDis || !possibleConnectionFound){
                            bestDis = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }

            if(possibleConnectionFound && !forceAccessibilityFromMainRoom){
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }

        }


        if(possibleConnectionFound && forceAccessibilityFromMainRoom){
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms,true);
        }

        if (!forceAccessibilityFromMainRoom){
            ConnectClosestRooms(allRooms,true);
        }
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB){
        Room.ConnectRooms(roomA, roomB);
       // Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.red, 100);
    
        List<Coord> line = GetLine(tileA,tileB);
        foreach(Coord c in line){
            DrawCircle(c, tunnelSize);
        }
    }

    void DrawCircle(Coord c, int r){
        for(int x = -r; x <= r; x++){
            for(int y = -r; y <= r; y++){
                if(x*x + y*y <= r*r){
                    int drawX = c.tileX + x;
                    int drawY = c.tileY + y;
                    if(IsInMapRange(drawX,drawY)){
                        map[drawX,drawY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to){
        List<Coord> line = new List<Coord>();

        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;

        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if(longest < shortest){
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest/2;
        for(int i = 0; i < longest; i++){
            line.Add(new Coord(x,y));

            if(inverted){
                y+= step;
            }
            else{
                x += step;
            }

            gradientAccumulation += shortest;
            if(gradientAccumulation >= longest){
                if(inverted){
                    x += gradientStep;
                }
                else{
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }
        return line;
    }
        

    Vector3 CoordToWorldPoint(Coord tile){
        return new Vector3(-width/2 + .5f + tile.tileX, -height / 2 + .5f + tile.tileY);
    }


    List<List<Coord>> GetRegions(int tileType){
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width,height];

        for(int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                if(mapFlags[x,y] == 0 && map[x,y] == tileType){
                    List<Coord> newRegion = GetRegionTiles(x,y);
                    regions.Add(newRegion);

                    foreach(Coord tile in newRegion){
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }
        return regions;
    }

    //Paintbucket tool to see how wide an area has a certain type
    List<Coord> GetRegionTiles(int startX, int startY){
        List<Coord> tiles =  new List<Coord>();
        int[,] mapFlags = new int[width,height];
        int tileType = map[startX,startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX,startY));
        mapFlags[startX,startY] = 1;

        while(queue.Count > 0){
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++){
                for (int y = tile.tileY - 1; y <= tile.tileY +1; y++){
                    if(IsInMapRange(x,y)&&(y == tile.tileY || x == tile.tileX)){
                        if(mapFlags[x,y] == 0 && map[x,y] == tileType){
                            mapFlags[x,y] = 1;
                            queue.Enqueue(new Coord(x,y));
                        }
                    }
                }
            }
        }
        return tiles;
    }

    bool IsInMapRange(int x, int y){
        return x >= 0 && x < width && y >= 0 && y < height;
    }

	//Fill the map randomly based on some parameters
	void RandomFillMap(){
		if(randomSeed){
			seed = Time.time.ToString();//create a new seed based on time since game ran
	}
		System.Random rng = new System.Random(seed.GetHashCode());//convert string to int
	
		for(int x = 0; x < width; x ++){
			for(int y = 0; y < height; y ++){
				if(x == 0 || x == width-1 || y == 0 || y == height-1){
					map[x,y] = 1;//if border, ensure wall
				}
				else{
				map[x,y] = (rng.Next(0,100) < randomFillPercent)? 1: 0;//otherwise random
				}
			}
		}
	}

	//Make the map more coherent
	void SmoothMap(){
		for(int x = 0; x < width; x ++){
			for(int y = 0; y < height; y ++){
				int neighborWalls = FindSurroundingWalls(x,y);//number of tiles surrounding which are walls

				if (neighborWalls > 4)
					map[x,y] = 1;//if surrounded, set to wall, if not, empty, if exactly 4, leave as is
				else if (neighborWalls < 4)
					map[x,y] = 0;
			}
		}
	}

	//Check for walls surrounding a space
	int FindSurroundingWalls(int gridX, int gridY)
	{
		int wallCount = 0;
		for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++){
			for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++){
				if(neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height){
					if(neighborX != gridX || neighborY != gridY){
						wallCount += map[neighborX,neighborY];
					}
				}
				else{
					wallCount ++;
				}
			}
		}
		return wallCount;
	}

    struct Coord{
        public int tileX;
        public int tileY;

        public Coord(int x, int y){
            tileX = x;
            tileY = y;
        }
    }

    class Room: IComparable<Room>{
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;


        public Room(){
        }

        public Room(List<Coord> roomTiles, int[,] map){
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<Coord>();
            foreach (Coord tile in tiles){
                for (int x = tile.tileX-1; x <= tile.tileX+1; x++){
                    for(int y = tile.tileY-1; y <= tile.tileY+1; y++){
                        if(x == tile.tileX || y == tile.tileY){
                            if(map[x,y] == 1){
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainRoom(){
            if(!isAccessibleFromMainRoom){
                isAccessibleFromMainRoom = true;
                foreach(Room connectedRoom in connectedRooms){
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(Room roomA, Room roomB){
            if(roomA.isAccessibleFromMainRoom){
                roomB.SetAccessibleFromMainRoom();
            }
            else if (roomB.isAccessibleFromMainRoom){
                roomA.SetAccessibleFromMainRoom();
            }

            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool IsConnect(Room otherRoom){
            return connectedRooms.Contains(otherRoom);
        }


        public int CompareTo(Room otherRoom){
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }
}
