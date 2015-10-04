using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponSystemController : SystemController {
    [Command]
    public void CmdSpawnWeapon(GameObject weapon, Vector3 transformPosition, Quaternion transformRotation, float inheritVelocity)
    {
        GameObject newWeapon = (GameObject)Instantiate(weapon, transformPosition, transformRotation);
        if (newWeapon.GetComponent<Rigidbody>() != null && GetComponentInParent<Rigidbody>() != null)
            newWeapon.GetComponent<Rigidbody>().velocity = inheritVelocity * GetComponentInParent<Rigidbody>().velocity;
        NetworkServer.SpawnWithClientAuthority(newWeapon, gameObject);
    }
}
