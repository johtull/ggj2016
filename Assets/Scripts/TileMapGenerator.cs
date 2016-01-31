using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class TileMapGenerator : MonoBehaviour {

    public Transform tileObj;
    public Transform wallObj;
    public Transform wallObj2;
    public Transform sigil1;
    public Transform altarObj;
    public Transform gateObj;
    public Transform exitObj;
    public Transform audioTrig;
    public Transform player;
    public Transform braObj;
    public Transform endObj;
    public Transform solPanel;
    public Vector2 mapSize;
    public bool randomSeed;//Toggle seed being random



    [Range(0,1)]
    public float outlinePercent;

    [Range(0,1)]
    public float obstacleFill;

    List<Coord> allTileCoords;
    List<int> solution;
    Queue<Coord> shuffledCoords;
    List<Transform> altarList;

    public int seed = 10;
    public bool bordered = true;
    public bool displayTiles = false;
    public int sigilCount = 3;
    public int brazierCount = 3;
    public int diff = 0;
   


    Coord mapCenter;
    Coord mapEntrance;
    Coord mapExit;
   
    void Start(){
        if(randomSeed)
            seed =  UnityEngine.Random.Range(1,10000);
       // seed = (int)Time.time;
//        diff = GameObject.Find("Record").GetComponent<ProgressTracker>().level;
      //  if(GameObject.Find("Record") == null)
        //    diff = 1;

        bordered = true;
        displayTiles = false;

        //Level 1
        if(diff == 1){
        mapSize = new Vector2(11,11);
        sigilCount = 3;
        }
        //Level 2
        else if(diff == 2){
            mapSize = new Vector2(21,21);
            sigilCount = 5;
        }
        //Level 3
        else if(diff == 3){
            mapSize = new Vector2(31,31);
            sigilCount = 6;
        }
        //Level 4
        else if(diff == 4){
            mapSize = new Vector2(41,41);
            sigilCount = 8;
        }
        //Level 5
       else if(diff == 5){
            mapSize = new Vector2(51,51);
            sigilCount = 10;
        }



        GenerateMap();
    }


    public void GenerateMap(){

        allTileCoords = new List<Coord>();
        solution = new List<int>();
        for(int i = 0; i < sigilCount; i++){
            solution.Add(i);
        }
        solution = new List<int>(Utility.ShuffleArray(solution.ToArray(), seed));
        solPanel.gameObject.GetComponent<SolutionTileScript>().setSolution(solution);
       /* for(int i = 0; i < sigilCount; i++){
            
            print("solution: " + solution[i]);
        }*/

        for(int x = 0; x < mapSize.x; x++){
            for(int y = 0; y < mapSize.y; y++){
                allTileCoords.Add(new Coord(x,y));
            }
        }
        shuffledCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));
        mapCenter = new Coord((int)mapSize.x/2,(int)mapSize.y/2);
        mapEntrance = new Coord((int)mapSize.x/2,0);
        mapExit = new Coord((int)mapSize.x/2,(int)mapSize.y-1);
        player.transform.position = CoordToPosition(mapEntrance.x,mapEntrance.y-1);

        string holderName = "Generated Map";
        if(transform.FindChild(holderName)){
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }
            
        Transform mapHolder = new GameObject (holderName).transform;
        mapHolder.parent = transform;

        for(int x = 0; x < mapSize.x; x++){
            for(int y = 0; y < mapSize.y; y++){
                Vector3 tilePosition = CoordToPosition(x,y);
                if(displayTiles){
                Transform newTile = Instantiate(tileObj, tilePosition, Quaternion.identity) as Transform;
                newTile.localScale = Vector3.one *(1-outlinePercent);
                newTile.parent = mapHolder;
                }
            }
        }

        int obstacleCount = (int)(mapSize.x * mapSize.y * obstacleFill);
        bool[,] obstacleMap = new bool[(int)mapSize.x,(int)mapSize.y];
        bool[,] sigilMap = new bool[(int)mapSize.x,(int)mapSize.y];
        bool[,] objectMap = new bool[(int)mapSize.x,(int)mapSize.y];
        int currentObstacleCount = 0;
        int currentSigilCount = 0;

        for(int i = 0; i < obstacleCount; i++){
            Transform newObstacle;
            currentObstacleCount ++;
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;


            if(randomCoord != mapEntrance && randomCoord != mapExit && MapIsFullyAccessible(obstacleMap, currentObstacleCount)) {
                 Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
            

                 newObstacle = Instantiate(wallObj, obstaclePosition + Vector3.forward * .5f, Quaternion.identity) as Transform;      

                 newObstacle.parent = mapHolder;

                objectMap[randomCoord.x, randomCoord.y]=true;
               
            }
            else{
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount --;
               
            }    

        }
        //border
        //Create a border around the map
        if(bordered){
            for(int x = -2; x < mapSize.x+2; x ++){
              for(int y = -2; y < mapSize.y+2; y ++){
                  Vector3 obstaclePosition = CoordToPosition(x, y);
                    //First Border
                    if((x < 0 || x >= mapSize.x || y < 0 || y >= mapSize.y) && (x != mapExit.x)){
                       Transform newBorder = Instantiate(wallObj2, obstaclePosition + Vector3.forward * .5f, Quaternion.identity) as Transform;
                      newBorder.parent = mapHolder;
                    }
                    //Alcove
                    if(((x > mapEntrance.x-1 && x < mapEntrance.x +1) && (y == mapEntrance.y-2))){
                        Transform newBorder = Instantiate(wallObj2, obstaclePosition + Vector3.forward * .5f, Quaternion.identity) as Transform;
                        newBorder.parent = mapHolder;
                    }   
               }
            }

            //Create the back room
            for(int x = -2; x < mapSize.x+2; x ++){
                for(int y = (int)mapSize.y+2; y < (int)(mapSize.y + 12)+2; y ++){
                    Vector3 obstaclePosition = CoordToPosition(x, y);
                    //First Border
                    if((x < 0 || x >= mapSize.x || y < (mapSize.y+2) || y >= (int)(mapSize.y+ 10 + 2)) && (x != mapExit.x)){
                        Transform newBorder = Instantiate(wallObj2, obstaclePosition + Vector3.forward * .5f, Quaternion.identity) as Transform;
                        newBorder.parent = mapHolder;
                    }
                }
            }

        }
        //Add Objects
 
        for(int i = 0; i < sigilCount; i++){
            Transform newSigil;
            currentSigilCount ++;
            Coord randomCoord = GetRandomCoord();
            sigilMap[randomCoord.x, randomCoord.y] = true;

            print("Passing");
          //  if(randomCoord != mapEntrance && randomCoord != mapExit) {
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

                print("Placed");

                
                newSigil = Instantiate(sigil1, obstaclePosition + Vector3.forward * .5f, Quaternion.identity) as Transform;      
                newSigil.GetComponent<SigilVariables>().ID = i; //Set Sigil ID for its image
                newSigil.parent = mapHolder;

                objectMap[randomCoord.x, randomCoord.y]=true;

          //  }   

        }

        //Add the gate guarding the chamber
        Vector3 gatPos = CoordToPosition(mapExit.x, mapExit.y + 1);
        Transform gate = Instantiate(gateObj,  gatPos + Vector3.forward *.5f, Quaternion.identity) as Transform;
        gate.parent = mapHolder;


        //Add the gate guarding the exit
        Vector3 exPos = CoordToPosition(mapExit.x, (int)(mapExit.y +12+1));
        Transform ex = Instantiate(exitObj,  exPos + Vector3.forward *.5f, Quaternion.identity) as Transform;
        ex.parent = mapHolder;

        //Add the End
        Vector3 endPos = CoordToPosition(mapExit.x, (int)(mapExit.y +12+2));
        Transform endOb = Instantiate(endObj,  endPos + Vector3.forward *.5f, Quaternion.identity) as Transform;
        endOb.parent = mapHolder;

        //Add the audio changer for entering the chamber
        Vector3 audPos = CoordToPosition(mapExit.x, mapExit.y + 2);
        Transform audTrig = Instantiate(audioTrig,  audPos + Vector3.forward *.5f, Quaternion.identity) as Transform;
        audTrig.parent = mapHolder;

        //Add altars 
        for(int a = 0; a < sigilCount; a++){
            Vector3 altarPos = CoordToPosition(mapExit.x + (int)((3*a)-sigilCount), mapExit.y+5);
            Transform alt = Instantiate(altarObj, altarPos + Vector3.forward *.5f, Quaternion.identity) as Transform;
            alt.gameObject.GetComponent<AltarScript>().ID = solution[a];
            alt.parent = mapHolder;

        }

        //add braziers
        brazierCount = (int)((mapSize.x * mapSize.y)/75);
        for(int i = 0; i < brazierCount; i++){
            Transform newBra;
            currentSigilCount ++;
            Coord randomCoord = GetRandomCoord();
            sigilMap[randomCoord.x, randomCoord.y] = true;
            Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);



            newBra = Instantiate(braObj, obstaclePosition + Vector3.forward * .5f, Quaternion.identity) as Transform;      
            newBra.parent = mapHolder;

            objectMap[randomCoord.x, randomCoord.y]=true;

            //  }   

        }

    }


    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount){
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0),obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord> ();
       // queue.Enqueue (mapCenter);
       // mapFlags [mapCenter.x, mapCenter.y] = true;
      //  int accessibleTileCount = 1;

        queue.Enqueue (mapEntrance);
        mapFlags [mapEntrance.x, mapEntrance.y] = true;
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
