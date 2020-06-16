using minijson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = "{\"creator\":\"Maestro\",\"forceModifier1\":\"OFF\",\"forceModifier2\":\"OFF\",\"forceMissile1\":\"FALCON\",\"forceMissile2\":\"MISSILE POD\",\"forceWeapon1\":\"DRILLER\",\"forceWeapon2\":\"CRUSHER\",\"forceLoadout\":\"OFF\",\"powerupFilterBitmask\":65535,\"powerupBigSpawn\":\"LOW\",\"powerupInitial\":\"NORMAL\",\"turnSpeedLimit\":\"MEDIUM\",\"powerupSpawn\":\"HIGH\",\"friendlyFire\":false,\"matchMode\":\"ANARCHY\",\"maxPlayers\":2,\"showEnemyNames\":\"NORMAL\",\"timeLimit\":1200,\"scoreLimit\":0,\"respawnTimeSeconds\":2,\"respawnShieldTimeSeconds\":2,\"level\":\"SKYBOX V1.2\",\"joinInProgress\":true,\"rearViewAllowed\":false,\"teamCount\":2,\"players\":[\"MAESTRO\"],\"name\":\"Stats\",\"type\":\"StartGame\",\"start\":\"2019 - 12 - 15T17: 48:49.0157153Z\",\"time\":0}";

            Dictionary<string,object> list = (Dictionary<string, object>)MiniJson.Parse(json);

        }
    }
}
