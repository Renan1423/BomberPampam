using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Explosion _chainedExplosion;
    private bool _checkedWall = false;
    [HideInInspector]
    public bool ExplosionUnlocked = false;
    [SerializeField]
    private Collider2D col;
    [SerializeField]
    private SpriteRenderer gfx;

    private void Awake()
    {
        StartCoroutine(CheckWallCoroutine());
    }

    private IEnumerator CheckWallCoroutine() 
    {
        _checkedWall = false;
        gfx.gameObject.SetActive(false);

        yield return new WaitUntil(() => ExplosionUnlocked == true);

        yield return new WaitForSeconds(0.05f);

        _checkedWall = true;
        if(_chainedExplosion != null)
            _chainedExplosion.ExplosionUnlocked = true;
        gfx.gameObject.SetActive(true);
        col.enabled = false;
        col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            DestroyExplosionInChain();
            return;
        }

        if (collision.gameObject.tag == "Hitable" && ExplosionUnlocked)
        {
            IHitable hitable = collision.GetComponent<IHitable>();

            if (!_checkedWall && collision.gameObject.GetComponent<ObstacleHitable>() != null) 
            {
                //Blocks the chain continuity if the explosion hits an obstacle
                hitable.OnHit(this.gameObject);
                return;
            }

            hitable.OnHit(this.gameObject);
        }
    }

    public void DestroyExplosionGO() 
    {
        Destroy(this.gameObject);
    }

    public void ChainExplosion(Explosion explosion) 
    {
        _chainedExplosion = explosion;
    }

    public void DestroyExplosionInChain() 
    {
        if(_chainedExplosion != null)
            _chainedExplosion.DestroyExplosionInChain();
        Destroy(this.gameObject);
    }
}
