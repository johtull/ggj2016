using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMapGenerator : MonoBehaviour {

    public Transform tileObj;
    public Transform wallObj;
    public Transform wallObj2;
    public Vector2 mapSize;

    [Range(0,1)]
    public float outlinePercent;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledCoords;

    public int seed = 10;
    public int obstacleCount = 10;
    public bool bordered = true;


    Coord mapCenter;
    public bool blue = false;

    void Start(){
        GenerateMap();
    }


    public void GenerateMap(){

        allTileCoords = new List<Coord>();

        for(int x = 0; x < mapSize.x; x++){
            for(int y = 0; y < mapSize.y; y++){
                allTileCoords.Add(new Coord(x,y));
            }
        }
        shuffledCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));
        mapCenter = new Coord((int)mapSize.x/2,(int)mapSize.y/2);

        string holderName = "Generated Map";
        if(transform.FindChild(holderName)){
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }
            
        Transform mapHolder = new GameObject (holderName).transform;
        mapHolder.parent = transform;

        for(int x = 0; x < mapSize.x; x++){
            for(int y = 0; y < mapSize.y; y++){
                Vector3 tilePosition = CoordToPosition(x,y);
                Transform newTile = Instantiate(tileObj, tilePosition, Quaternion.identity) as Transform;
                newTile.localScale = Vector3.one *(1-outlinePercent);
                newTile.parent = mapHolder;
            }
        }


        bool[,] obstacleMap = new bool[(int)mapSize.x,(int)mapSize.y];
        int currentObstacleCount = 0;

        for(int i = 0; i < obstacleCount; i++){
            Transform newObstacle;
            currentObstacleCount ++;
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;

            if(randomCoord != mapCenter && MapIsFullyAccessible(obstacleMap, currentObstacleCount)) {
            Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
        //    int ran = Random.Range(1,3);
        //    if(!blue)
         //       ran = 1;
         //   if(ran == 1)
            newObstacle = Instantiate(wallObj, obstaclePosition + Vector3.forward * .5f, Quaternion.identity) as Transform;      
          //  else
          //  newObstacle = Instantiate(wallObj2, obstaclePosition + Vector3.forward * .5f, Quaternion.identity) as Transform;
            newObstacle.parent = mapHolder;
            }
            else{
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount --;
            }    

        }
        //border
        //Create a border around the map
        if(bordered){
            for(int x = -1; x < mapSize.x+1; x ++){
              for(int y = -1; y < mapSize.y+1; y ++){
                  Vector3 obstaclePosition = CoordToPosition(x, y);

                    if(x < 0 || x >= mapSize.x || y < 0 || y >= mapSize.y){
                       Transform newBorder = Instantiate(wallObj2, obstaclePosition + Vector3.forward * .5f, Quaternion.identity) as Transform;
                      newBorder.parent = mapHolder;
                    }          
               }
            }
        }

    }


    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount){
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0),obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord> ();
        queue.Enqueue (mapCenter);
        mapFlags [mapCenter.x, mapCenter.y] = true;
        int accessibleTileCount = 1;

        while (queue.Count > 0) {
            Coord tile = queue.Dequeue();

            for (int x = -1; x <= 1; x ++) {
                for (int y = -1; y <= 1; y ++) {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0) {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1)) {
                            if (!mapFlags[neighbourX,neighbourY] && !obstacleMap[neighbourX,neighbourY]) {
                                mapFlags[neighbourX,neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX,neighbourY));
                                accessibleTileCount ++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    Vector3 CoordToPosition(int x, int y){
        return new Vector3 (-mapSize.x/2 + .5f + x,-mapSize.y/2 + 0.5f + y,0);
    }

    public Coord GetRandomCoord(){
        Coord randomCoord = shuffledCoords.Dequeue();
        shuffledCoords.Enqueue(randomCoord);
        return randomCoord;
    }
	
    public struct Coord {
        public int x;
        public int y;

        public Coord(int _x, int _y) {
            x = _x;
            y = _y;
        }

        public static bool operator ==(Coord c1, Coord c2) {
            return c1.x == c2.x && c1.y == c2.y;
        }

        public static bool operator !=(Coord c1, Coord c2) {
            return !(c1 == c2);
        }

    }
}
