using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{

    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private Animator _anim;
    private bool _alerted = false;
    [SerializeField]
    private float Distance;
    // Update is called once per frame
    void Update()
    {
        Distance = Vector3.Distance(_player.transform.position, transform.position);
        if (_alerted == false ) { 
        if(Distance < 5f) {
            _target = _player;
            _alerted = true;
        }
        } else
        {
            if(Distance > 1.5f)
            {
                _anim.SetBool("Walking_f", true);

            } else
            {
                _anim.SetBool("Walking_f", false);
                _anim.SetBool("Attack", true);
            }
            Vector3 lookVector = _target.transform.position - transform.position;
            lookVector.y = transform.position.y;
            Quaternion rot = Quaternion.LookRotation(lookVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
        }
    }
}
