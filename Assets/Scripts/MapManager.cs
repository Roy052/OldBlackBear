using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    int stageNum = 0;
    
    int[,] map;
    int[] stagePerMapSize = new int[3] { 7, 8, 9 };
    int[] stageSum = new int[3] { 12, 14, 16 };

    int[] tempMapArray;

    int[,] mapContents; // 0 : Empty, 1 : Not contain, 2 : Start, 3 : Shop,
                        // 4 : Bonfire, 5 : Random Event, 6 : Chest, 7 : Enemy
                        // 8 : Mid Boss, 9 : Boss
    
    // Start is called before the first frame update
    void Start()
    {
        map = new int[stagePerMapSize[stageNum], 3];
        MapBuilding();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MapBuilding()
    {
        //Initialize
        System.Array.Clear(map, 0, map.Length);

        //Temp Map Array
        tempMapArray = new int[stagePerMapSize[stageNum] - 2];
        int tempMapSum = 0;
        int beforeMapNum = 0; // Before Random Num
        float meanSum = stageSum[stageNum] / (float) stagePerMapSize[stageNum];
        Debug.Log(meanSum);

        //First Value In
        tempMapArray[0] = Random.Range(0, 3) + 1;
        tempMapSum = tempMapArray[0];
        beforeMapNum = tempMapArray[0];

        //After Value In
        for (int i = 1; i < tempMapArray.Length; i++){
            int tempValue = Random.Range(0,3) + 1;
            
            //Current sum is more than mean sum
            if( (tempMapSum + tempValue) > meanSum * (i + 1) )
            {
                if (tempValue != 1)
                    tempValue -= 1;
            }
            else
            {
                if (tempValue != 3)
                    tempValue += 1;
            }

            tempMapArray[i] = tempValue;
            beforeMapNum = tempValue;
            tempMapSum += tempValue;
        }

        //tempMapArray fit in stageSum
        if (stageSum[stageNum] > tempMapSum)
        {
            for (int i = 0; i < stageSum[stageNum] - tempMapSum; i++)
            {
                for(int j = 0; j < tempMapArray.Length ; j++)
                {
                    if(tempMapArray[j] == 3) continue;
                    else
                    {
                        tempMapArray[j]++;
                        break;
                    }
                }
            }

        }
        else if(stageSum[stageNum] < tempMapSum)
        {
            for (int i = 0; i < tempMapSum - stageSum[stageNum] ; i++)
            {
                for (int j = tempMapArray.Length - 1; j > 0; j--)
                {
                    if (tempMapArray[j] == 1) continue;
                    else
                    {
                        tempMapArray[j]--;
                        break;
                    }
                }
            } 
        }

        //Number To Map Apperance
        int[,] mapApperance_two = { { 0, 1 }, { 1, 2 }};

        int tempMapApperance = -1;
        for (int i = 0; i < tempMapArray.Length; i++)
        {
            //Full
            if (tempMapArray[i] == 3)
            {
                map[i + 1, 0] = 1; map[i + 1, 1] = 1; map[i + 1, 2] = 1;
            }
            else if (tempMapArray[i] == 2)
            {
                tempMapApperance = Random.Range(0, 2);
                for (int j = 0; j < 2; j++)
                {
                    map[i + 1, mapApperance_two[tempMapApperance, j]] = 1;
                }
            }
            else
            {
                map[i + 1, 1] = 1;
            }
        }

        string ForDebug = "";
        for (int i = 0; i < stagePerMapSize[stageNum]; i++)
        {
            ForDebug += map[i, 0] + ", " + map[i, 1] + ", " + map[i, 2] + "\n";
        }

        Debug.Log(ForDebug);

        //Map Apperance To Map Content
        map[0, 1] = 2; // Start
        map[stagePerMapSize[stageNum] - 1, 1] = 9; // Boss
        map[stagePerMapSize[stageNum] - 2, 1] = 4; // Bonfire
    }
}