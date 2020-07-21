using System;
using System.Collections.Generic;

[Serializable]
public class StageObjectsModel
{
    public List<StageObject> stageObjects = new List<StageObject>();

    [Serializable]
    public class StageObject
    {
        public String item;
        public float coordinate;
        public bool hasLife;
    }
}
