using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistant
{
    public string Save();
    public void Load(string jsonString);
}
