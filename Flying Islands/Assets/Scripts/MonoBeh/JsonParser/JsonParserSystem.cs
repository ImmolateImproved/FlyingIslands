using JsonData;
using Newtonsoft.Json;
using UnityEngine;

public class JsonParserSystem : MonoBehaviour
{
    public CombatResult combatResult;
    public CombatResult combatResult2;

    private void Awake()
    {
        var test = JsonConvert.SerializeObject(combatResult);

        combatResult2 = JsonConvert.DeserializeObject<CombatResult>(test);
    }
}