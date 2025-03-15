using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 100;

    private int _health;
    private int Health { get { return _health; } set { _health = value; _healthBar.SetValue(value); } }

    private UIPlayerHealthBar _healthBar;

    private PhotonView _photonView;

    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _healthBar = GetComponentInChildren<UIPlayerHealthBar>();
        _healthBar.Setup(_maxHealth);
        Health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _photonView.RPC("RemoteDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    private void RemoteDamage(int damage)
    {
        if (damage < 1)
            return;

        Health -= damage;
    }
}
