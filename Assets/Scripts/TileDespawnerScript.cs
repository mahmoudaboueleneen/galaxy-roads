using UnityEngine;

public class TileDespawnerScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("INNN");
        if (other.CompareTag("NormalTile") ||
            other.CompareTag("StickyTile") ||
            other.CompareTag("BurningTile") ||
            other.CompareTag("SuppliesTile") ||
            other.CompareTag("BoostTile") ||
            other.CompareTag("EmptyTile"))
        {
            TileManager.Instance.DestroyAndReplaceTile(other.gameObject);
        }
    }
}
