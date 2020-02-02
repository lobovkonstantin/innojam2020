using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class WorldVariablesHandler : MonoBehaviour
{
    public int levelNumber;

    public static WorldVariablesHandler Instance;

    private float time;

    public LinkedList<string> nameList = new LinkedList<string>();

    ArrayListGenerator arrGen = new ArrayListGenerator();

    ArrayList predicateList = new ArrayList();

    ArrayList adjectiveList1 = new ArrayList();

    ArrayList adjectiveList2 = new ArrayList();

    public Dictionary<string, IPooledObject> itemDictionary = new Dictionary<string, IPooledObject>();

    void Start()
    {
        time = 20f;
        levelNumber = 1;
        Instance = this;
        predicateList = arrGen.ListGenerate(@"Assets/Resources/PredicateList.txt");
        adjectiveList1 = arrGen.ListGenerate(@"Assets/Resources/Adjective1List.txt");
        adjectiveList2 = arrGen.ListGenerate(@"Assets/Resources/Adjective2List.txt");
    }

    public void NextLevel()
    {
        levelNumber++;
    }

    void FixedUpdate()
    {
        if (time > 0)
        {
            time = time - Time.fixedDeltaTime;
        }
        else
        {
            time = 3;
            NextLevel();
        }
    }

    public ArrayList GetPredicateList()
    {
        return predicateList;
    }



    public ArrayList GetAdjectiveList1()
    {
        return adjectiveList1;
    }

    public ArrayList GetAdjectiveList2()
    {
        return adjectiveList2;
    }

    public int getLevel()
    {
        return levelNumber;
    }

    public Dictionary<string, IPooledObject> GetitemDicionary()
    {
        return itemDictionary;
    }
}
