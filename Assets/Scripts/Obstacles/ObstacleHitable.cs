using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHitable : MonoBehaviour, IHitable
{
    [SerializeField]
    private int _hp;
    [SerializeField]
    private Collider2D _col;
    [SerializeField]
    private Animator _anim;

    public void OnHit(GameObject hitOwner)
    {
        _hp--;

        if (_hp <= 0) 
        {
            _col.enabled = false;
            _anim.SetTrigger("Destroy");
            OnDestroyHitable();
        }

        Explosion explosion = hitOwner.GetComponent<Explosion>();
        if (explosion != null) 
        {
            explosion.DestroyExplosionInChain();
            return;
        }
    }

    public void OnDestroyHitable()
    {
        var guo = new GraphUpdateObject(_col.bounds);
        AstarPath.active.UpdateGraphs(guo);
        StageGridManager.instance.RemoveObstacleFromTilemap(transform.position);

        Destroy(gameObject, 0.5f);
    }

}
