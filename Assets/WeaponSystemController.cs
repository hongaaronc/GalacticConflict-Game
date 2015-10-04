using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponSystemController : SystemController {
    [Command]
    public void CmdSpawnWeapon(GameObject weapon)
    {
        NetworkServer.SpawnWithClientAuthority(weapon, networkIdentity.connectionToClient);
    }

    [Command]
    public void CmdSpawnClientsideWeapons(GameObject[] myClientsideWeapons, GameObject spawnPoint, float inheritVelocity)
    {
        RpcSpawnClientsideWeapons(myClientsideWeapons, spawnPoint, inheritVelocity);
    }

    [ClientRpc]
    public void RpcSpawnClientsideWeapons(GameObject[] myClientsideWeapons, GameObject spawnPoint, float inheritVelocity)
    {
        foreach (GameObject weapon in myClientsideWeapons)
        {
            GameObject newWeapon = (GameObject)Instantiate(weapon, spawnPoint.transform.position, spawnPoint.transform.rotation);
            if (newWeapon.GetComponent<Rigidbody>() != null && GetComponentInParent<Rigidbody>() != null)
                newWeapon.GetComponent<Rigidbody>().velocity = inheritVelocity * GetComponentInParent<Rigidbody>().velocity;
        }
    }
}
