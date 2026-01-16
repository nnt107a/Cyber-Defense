using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CryoNode : Turret
{
    private float activationTimer = 0f;
    private bool isActive = false;

    private List<Enemy> collidingEnemies = new List<Enemy>();

    protected override void Awake()
    {
        base.Awake();
        isActive = false;
    }

    protected override void Update()
    {
        if (!GameManager.Instance.isLevelOnGoing || laneIndex == -1)
        {
            return;
        }

        if (!isActive)
        {
            activationTimer += Time.deltaTime;
            if (activationTimer >= turretData.activationDelay)
            {
                Activate();
            }
        }
    }

    private void Activate()
    {
        isActive = true;
        animator.SetTrigger("activate");
        if (collidingEnemies.Count > 0)
        {
            Explode(collidingEnemies[0].gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() == null)
            return;
        collidingEnemies.Add(collision.gameObject.GetComponent<Enemy>());
        if (!isActive)
            return;

        Explode(collision.gameObject);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() == null)
            return;
        collidingEnemies.Remove(collision.gameObject.GetComponent<Enemy>());
    }

    private void Explode(GameObject triggeringEnemy)
    {
        Enemy enemy = triggeringEnemy.GetComponent<Enemy>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            turretData.radius * 1.5f
        );

        if (gridCell == null)
        {
            enemy.GetComponent<Enemy>()?.TakeDamage((int)turretData.attackDamage, true);
            enemy.GetComponent<Enemy>()?.effectController.ApplyEffect(turretData.specialEffect, this);
        }
        else
        {
            foreach (var hit in hits)
            {
                if (!hit.CompareTag("Enemy"))
                    continue;

                GridCell enemyCell = Utils.GetGridCellAt(hit.transform.position);

                if (enemyCell == null)
                    continue;

                if (Utils.IsWithinGridRadius(gridCell, enemyCell, (int)turretData.radius))
                {
                    hit.GetComponent<Enemy>()?.TakeDamage((int)turretData.attackDamage);
                    hit.GetComponent<Enemy>()?.effectController.ApplyEffect(turretData.specialEffect, this);
                    Debug.Log(
                        $"Explosion hit enemy {hit.name} at cell ({enemyCell.x},{enemyCell.y}), deal {turretData.attackDamage}"
                    );
                }
            }
        }

        Destroy(gameObject);
        gridCell.RemoveTurret();
    }
}