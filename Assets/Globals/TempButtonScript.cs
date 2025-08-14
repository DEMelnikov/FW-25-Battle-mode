using NUnit.Framework.Internal;
using UnityEngine;

public class TempButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject characterQQQ;
    
    public void test() 
    {
        SelectionManager.ClearSelections();
       
    }
}
