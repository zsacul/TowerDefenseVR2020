using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chunk : MonoBehaviour
{
    public ChunkType type;
    public PrefabsSet prefabs;
    public GameEvent obstacleBuiltOnPath;
    private bool canBeModified;
    [Tooltip("if true => can change type in runtime with ChangeType function")]
    public UnityEvent changeTypeEvent;
    [HideInInspector]
    public int x, y;
    [HideInInspector]
    public ChunkScene owner;
    public int choice;
    private GameObject currentObject;
    private GameObject upgradedTower;
    private float UpgradedTowerHeight;
    public bool ChangeType(ChunkType newType, int choice = 0)
    {
        if(/*canBeModified &&*/ ValidOperation(newType))
        {
            this.choice = choice;
            if (type != newType)
            {
                if (newType == ChunkType.playerObstacle && owner.oldPath[x, y])
                {
                    //Debug.Log("obstacleBuiltOnPath.Raise()");
                    obstacleBuiltOnPath.Raise();
                }
                type = newType;
                changeTypeEvent.Invoke();
                UpdateChunk();
                owner.UpdateChunks(true);
                return true;
            }
        }
        return false;
    }

    public void UpgradeTower(TowerType elementType)
    {
        if (type == ChunkType.tower)
        {
            UpgradeTower((int)elementType);
        }
    }

    public void UpgradeTower(int elementIndex)
    {
        if (type == ChunkType.tower)
        {
            if (currentObject != null)
            {
                UpgradedTowerHeight = currentObject.GetComponent<TowerHeight>().towerHeight;
                currentObject.GetComponentInChildren<ObjectVisibility>().StartDisappearing();
                Destroy(currentObject, 3f);
            }
            upgradedTower = Instantiate(prefabs.tower[elementIndex], transform);
            upgradedTower.transform.localScale = new Vector3(upgradedTower.transform.localScale.x, UpgradedTowerHeight, upgradedTower.transform.localScale.z);
        }
    }

    public void DestroyUpgradedTower()
    {
        Destroy(upgradedTower, 3f);
    }

    public bool ValidOperation(ChunkType newType, bool modifyPathInBFS = true)
    {
        owner.HideNewPath();
        if (type == ChunkType.none ||
            type == ChunkType.naturalObstacle ||
            type == ChunkType.border)
        {
            owner.HideNewPath();
            return false;
        }
        
        int xSize = owner.mapString.sizeX;
        int ySize = owner.mapString.sizeY;

        if(type == ChunkType.spawnPoint)
        {
            owner.HideNewPath();
            return false;
        }

        switch (newType)
        {
            case ChunkType.tower:
                for (int y = this.y - 1; y <= this.y + 1; y++)
                {
                    for (int x = this.x - 1; x <= this.x + 1; x++)
                    {
                        if (x >= 0 && y >= 0 && x < xSize && y < ySize && 
                            owner.chunkMap[x, y].type == ChunkType.tower)
                        {
                            owner.HideNewPath();
                            return false;
                        }
                    }
                }
                if (owner.path[this.x, this.y])
                {
                    return BFS(modifyPathInBFS);
                }
                return true;
                
            case ChunkType.playerObstacle:
                if (owner.path[this.x, this.y])
                {
                    return BFS(modifyPathInBFS);
                }
                return true;

            case ChunkType.empty:
                Debug.Log("Trying to change type to ChunkType.empty");
                return BFS(modifyPathInBFS, true);
                
            default:
                return false;
        }
    }
    public void UpdateChunk()
    {
        if(currentObject != null)
        {
            Destroy(currentObject, 3.0f);
        }
        switch(type)
        {
            case ChunkType.none:
                currentObject = Instantiate(prefabs.none[Random.Range(0, prefabs.none.Length)], transform);
                break;
            case ChunkType.empty:
                if (owner.path[x, y])
                {
                    if(currentObject != null)
                    {
                        currentObject.transform.Translate(Vector3.down*0.1f);
                    }
                    PrettyPath();
                }
                else
                {
                    currentObject = Instantiate(prefabs.empty[Random.Range(2, prefabs.empty.Length)], transform);
                }
                //Destroys upgraded tower if it exists
                if (upgradedTower)
                {
                    DestroyUpgradedTower();
                }
                break;
            case ChunkType.border:
                PrettyBorder();
                break;
            case ChunkType.naturalObstacle:
                currentObject = Instantiate(prefabs.naturalObstacle[Random.Range(0, prefabs.naturalObstacle.Length)], transform);
                break;
            case ChunkType.playerObstacle:
                currentObject = Instantiate(prefabs.playerObstacle[Random.Range(0, prefabs.playerObstacle.Length)], transform);
                break;
            case ChunkType.tower:
                currentObject = Instantiate(prefabs.tower[choice], transform);
                break;
            case ChunkType.spawnPoint:
                PrettySpawnPoint();
                break;
            case ChunkType.endPoint:
                PrettyEndPoint();
                break;
        }
    }

    public bool PathExist() {

        bool endPointReached = false;
        HashSet<(int, int)> wasVisited = new HashSet<(int, int)>();
        Queue<(int, int)> Q = new Queue<(int, int)>();
        (int, int) spawnPoint = owner.GetFirstChunkOfType(ChunkType.spawnPoint);
        (int, int) endPoint = owner.GetFirstChunkOfType(ChunkType.endPoint);
        Q.Enqueue(spawnPoint);
        wasVisited.Add(spawnPoint);


        while(Q.Count != 0 || !endPointReached) 
        {
            (int, int) element = Q.Dequeue();
            if (element == endPoint)
            {
                endPointReached = true;
                break;
            }
            
            wasVisited.Add(element);
            int xCord = element.Item1;
            int yCord = element.Item2;

            // (x, y) -> (x+1,y), (x-1,y), (x, y+1), (x, y-1)
            for(int y = yCord + 1; y >= yCord - 1; y -= 2) 
            {
                ChunkType chunkType = owner.GetChunkType(xCord, y);
                if(chunkType == ChunkType.empty && (xCord != this.x && y != this.y) && !wasVisited.Contains((xCord, y))) {
                    Q.Enqueue((xCord, y));
                }
            }
            for(int x = xCord + 1; x >= xCord - 1; y -= 2) 
            {
                ChunkType chunkType = owner.GetChunkType(xCord, y);
                if(chunkType == ChunkType.empty && (x != this.x && yCord != this.y) && !wasVisited.Contains((xCord, y))) {
                    Q.Enqueue((xCord, y));
                }
            }
        }

        return endPointReached;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="changeMap">Determines whether BFS should modify current path or is it called just to check if a path exist. Default value: true</param>
    /// <returns></returns>
    public bool BFS(bool changeMap = true, bool goThroughThisChunk = false)
    {
        int xSize = owner.mapString.sizeX;
        int ySize = owner.mapString.sizeY;
        int[,] value = new int[xSize, ySize];   //0 is not visited;

        bool endPointReached = false;
        Queue<(int, int)> Q = new Queue<(int, int)>();
        (int, int) spawnPoint = owner.GetFirstChunkOfType(ChunkType.spawnPoint);
        (int, int) endPoint = owner.GetFirstChunkOfType(ChunkType.endPoint);
        Q.Enqueue(spawnPoint);
        int step = 1;
        value[spawnPoint.Item1, spawnPoint.Item2] = step;

        while (Q.Count != 0 && !endPointReached)
        {
            (int, int) element = Q.Dequeue();
            if (element == endPoint)
            {
                endPointReached = true;
                break;
            }
            int xCord = element.Item1;
            int yCord = element.Item2;

            // (x, y) -> (x+1,y), (x-1,y), (x, y+1), (x, y-1)
            for (int y = yCord + 1; y >= yCord - 1; y -= 2)
            {
                if(y < ySize && y >= 0)
                {
                    ChunkType chunkType = owner.GetChunkType(xCord, y);
                    if ((((xCord != this.x || y != this.y)  && (chunkType == ChunkType.empty || 
                        chunkType == ChunkType.endPoint || chunkType == ChunkType.spawnPoint)) ||
                        (xCord == this.x && y == this.y && goThroughThisChunk)) && value[xCord, y] == 0) 
                    {
                        Q.Enqueue((xCord, y));
                        value[xCord, y] = value[xCord,yCord]+1;
                    }

                }
            }
            for (int xx = xCord + 1; xx >= xCord - 1; xx -= 2)
            {
                if(xx < xSize && xx >= 0)
                {
                    ChunkType chunkType = owner.GetChunkType(xx, yCord);
                    if ((((xx != this.x || yCord != this.y) && (chunkType == ChunkType.empty || 
                        chunkType == ChunkType.endPoint || chunkType == ChunkType.spawnPoint)) ||
                        (xx == this.x && yCord == this.y && goThroughThisChunk)) && value[xx, yCord] == 0)
                    {
                        Q.Enqueue((xx, yCord));
                        value[xx, yCord] = value[xCord, yCord] + 1;
                    }
                }
            }
        }
        if(endPointReached)
        {

            (int, int) point = endPoint;
            step = value[endPoint.Item1, endPoint.Item2];
            for(int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (changeMap) owner.path[x, y] = false; 
                }
            }
            if (changeMap) owner.path[endPoint.Item1, endPoint.Item2] = true;
            if (changeMap) owner.path[spawnPoint.Item1, spawnPoint.Item2] = true;
            owner.route.Clear();
            //back track
            while (step > 1)
            {
                owner.route.Add(point);
                step--;
                int x = point.Item1;
                int y = point.Item2;
                if (changeMap) owner.path[x, y] = true;
                if(x-1>=0 && step == value[x-1,y])
                {
                    point = (x-1, y);
                    continue;
                }
                if(x+1 < xSize && step == value[x+1, y])
                {
                    point = (x+1, y);
                    continue;
                }
                if (y-1 >= 0 && step == value[x, y-1])
                {
                    point = (x, y-1);
                    continue;
                }
                if (y + 1 < ySize && step == value[x, y+1])
                {
                    point = (x, y+1);
                    continue;
                }
                if (changeMap)
                {
                    Debug.LogError($"Chunks.cs/BFS failed while backtracking");
                    return false;
                }
            }
            owner.ShowNewPath();
            owner.Invoke("HideNewPath", 2);
            return true;
        }
        else
        {
            return endPointReached;
        }
    }
    private void PrettyPath()
    {
        int xSize = owner.mapString.sizeX;
        int ySize = owner.mapString.sizeY;
        //corner cases
        //
        //XX
        // X
        if (x-1 >= 0 && y-1 >= 0 &&
           owner.path[x,y-1] == true &&
           owner.path[x-1,y] == true)
        {
            currentObject = Instantiate(prefabs.empty[1], transform);
            return;
        }
        // X
        //XX
        // 
        if (x - 1 >= 0 && y + 1 < ySize &&
            owner.path[x-1, y] == true &&
           owner.path[x, y+1] == true)
        {
            currentObject = Instantiate(prefabs.empty[1], transform);
            currentObject.transform.Rotate(Vector3.up * 90);
            return;
        }
        // X
        // XX
        // 
        if (x + 1 < xSize && y + 1 < ySize &&
            owner.path[x, y+1] == true &&
           owner.path[x+1, y] == true)
        {
            currentObject = Instantiate(prefabs.empty[1], transform);
            currentObject.transform.Rotate(Vector3.up * 180);
            return;
        }
        //
        // XX
        // X
        if (x + 1 < xSize && y - 1 >= 0 &&
            owner.path[x+1, y] == true &&
           owner.path[x, y-1] == true)
        {
            currentObject = Instantiate(prefabs.empty[1], transform);
            currentObject.transform.Rotate(Vector3.up * 270);
            return;
        }
        //straight cases
        // X
        // X
        // X
        if (y + 1 < ySize && y - 1 >= 0 &&
            owner.path[x, y-1] == true &&
           owner.path[x, y+1] == true)
        {
            currentObject = Instantiate(prefabs.empty[0], transform);
            currentObject.transform.Rotate(Vector3.up * 90);
            return;
        }
        //
        //XXX
        // 
        if (x + 1 < xSize && x - 1 >= 0 &&
            owner.path[x-1, y] == true &&
           owner.path[x+1, y] == true)
        {
            currentObject = Instantiate(prefabs.empty[0], transform);
            return;
        }
    }
    private void PrettyBorder()
    {
        int xSize = owner.mapString.sizeX;
        int ySize = owner.mapString.sizeY;
        //corner cases
        //
        //XX
        // X
        if (x - 1 >= 0 && y - 1 >= 0 &&
           (owner.GetChunkType(x, y - 1) == ChunkType.border || owner.GetChunkType(x, y - 1) == ChunkType.spawnPoint) &&
           (owner.GetChunkType(x - 1, y) == ChunkType.border || owner.GetChunkType(x - 1, y) == ChunkType.spawnPoint))
        {
            if(owner.GetChunkType(x - 1, y - 1)==ChunkType.none)
            {
                //inner
                currentObject = Instantiate(prefabs.border[0], transform);
                currentObject.transform.Rotate(Vector3.up * 180);
            }
            else
            {
                //outer
                currentObject = Instantiate(prefabs.border[1], transform);
            }
            currentObject.transform.Rotate(Vector3.up * 90);
            return;
        }
        // X
        //XX
        // 
        if (x - 1 >= 0 && y + 1 < ySize &&
            (owner.GetChunkType(x, y + 1) == ChunkType.border || owner.GetChunkType(x, y + 1) == ChunkType.spawnPoint) &&
           (owner.GetChunkType(x - 1, y) == ChunkType.border || owner.GetChunkType(x - 1, y) == ChunkType.spawnPoint))
        {
            if (owner.GetChunkType(x - 1, y + 1) == ChunkType.none)
            {
                //inner
                currentObject = Instantiate(prefabs.border[0], transform);
                currentObject.transform.Rotate(Vector3.up * 180);
            }
            else
            {
                //outer
                currentObject = Instantiate(prefabs.border[1], transform);
            }
            currentObject.transform.Rotate(Vector3.up * 180);
            return;
        }
        // X
        // XX
        // 
        if (x + 1 < xSize && y + 1 < ySize &&
            (owner.GetChunkType(x, y + 1) == ChunkType.border || owner.GetChunkType(x, y + 1) == ChunkType.spawnPoint) &&
           (owner.GetChunkType(x + 1, y) == ChunkType.border || owner.GetChunkType(x + 1, y) == ChunkType.spawnPoint))
        {
            if (owner.GetChunkType(x + 1, y + 1) == ChunkType.none)
            {
                //inner
                currentObject = Instantiate(prefabs.border[0], transform);
                currentObject.transform.Rotate(Vector3.up * 180);
            }
            else
            {
                //outer
                currentObject = Instantiate(prefabs.border[1], transform);
            }
            currentObject.transform.Rotate(Vector3.up * 270);
            return;
        }
        //
        // XX
        // X
        if (x + 1 < xSize && y - 1 >= 0 &&
            (owner.GetChunkType(x, y - 1) == ChunkType.border || owner.GetChunkType(x, y - 1) == ChunkType.spawnPoint) &&
           (owner.GetChunkType(x + 1, y) == ChunkType.border || owner.GetChunkType(x + 1, y) == ChunkType.spawnPoint))
        {
            if (owner.GetChunkType(x + 1, y - 1) == ChunkType.none)
            {
                //inner
                currentObject = Instantiate(prefabs.border[0], transform);
                currentObject.transform.Rotate(Vector3.up * 180);
            }
            else
            {
                //outer
                currentObject = Instantiate(prefabs.border[1], transform);
            }
            return;
        }
        //straight cases
        // X
        // X
        // X
        if (y + 1 < ySize && y - 1 >= 0 &&
            (owner.GetChunkType(x, y - 1) == ChunkType.border || owner.GetChunkType(x, y - 1) == ChunkType.spawnPoint) &&
           (owner.GetChunkType(x, y + 1) == ChunkType.border || owner.GetChunkType(x, y + 1) == ChunkType.spawnPoint))
        {
            if (x+1 >= xSize || owner.GetChunkType(x + 1, y) == ChunkType.none)
            {
                //inner
                currentObject = Instantiate(prefabs.border[Random.Range(2, prefabs.border.Length)], transform);
                currentObject.transform.Rotate(Vector3.up * 180);
            }
            else
            {
                //outer
                currentObject = Instantiate(prefabs.border[Random.Range(2, prefabs.border.Length)], transform);
            }
            return;
        }
        //
        //XXX
        // 
        if (x + 1 < xSize && x - 1 >= 0 &&
            (owner.GetChunkType(x + 1, y) == ChunkType.border || owner.GetChunkType(x + 1, y) == ChunkType.spawnPoint) &&
           (owner.GetChunkType(x - 1, y) == ChunkType.border || owner.GetChunkType(x - 1, y) == ChunkType.spawnPoint))
        {
            if (y + 1 >= ySize || owner.GetChunkType(x, y + 1) == ChunkType.none)
            {
                //inner
                currentObject = Instantiate(prefabs.border[Random.Range(2, prefabs.border.Length)], transform);
            }
            else
            {
                //outer
                currentObject = Instantiate(prefabs.border[Random.Range(2, prefabs.border.Length)], transform);
                currentObject.transform.Rotate(Vector3.up * 180);

            }
            currentObject.transform.Rotate(Vector3.up * 90);
            return;
        }
    }
    private void PrettyEndPoint()
    {
        int xSize = owner.mapString.sizeX;
        int ySize = owner.mapString.sizeY;
        currentObject = Instantiate(prefabs.endPoint[Random.Range(0, prefabs.endPoint.Length)], transform);
        if (owner.path[x, y - 1] == true)
        {
            currentObject.transform.Rotate(Vector3.up * 90);
        }
        if (owner.path[x - 1, y] == true)
        {
            currentObject.transform.Rotate(Vector3.up * 180);
        }
        if (owner.path[x, y + 1] == true)
        {
            currentObject.transform.Rotate(Vector3.up * 270);
        }
        if (owner.path[x + 1, y] == true)
        {
            currentObject.transform.Rotate(Vector3.up);
        }
    }
    private void PrettySpawnPoint()
    {
        int xSize = owner.mapString.sizeX;
        int ySize = owner.mapString.sizeY;
        currentObject = Instantiate(prefabs.spawnPoint[Random.Range(0, prefabs.spawnPoint.Length)], transform);
        if (owner.path[x, y - 1] == true)
        {
            currentObject.transform.Rotate(Vector3.up * 90);
        }
        if (owner.path[x - 1, y] == true)
        {
            currentObject.transform.Rotate(Vector3.up * 180);
        }
        if (owner.path[x, y + 1] == true)
        {
            currentObject.transform.Rotate(Vector3.up * 270);
        }
        if (owner.path[x + 1, y] == true)
        {
            currentObject.transform.Rotate(Vector3.up);
        }
    }
}
