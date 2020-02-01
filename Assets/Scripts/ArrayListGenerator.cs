using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class ArrayListGenerator
    {
        public ArrayList ListGenerate(String filepath)
        {
            var lines = System.IO.File.ReadAllLines(filepath);
            ArrayList wordList = new ArrayList();

            foreach (var line in lines)
            {
                wordList.Add(line);
            }
        return wordList;
        }

    }
