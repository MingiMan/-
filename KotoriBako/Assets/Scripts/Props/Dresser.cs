using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dresser : Props
{
    [SerializeField] Animator dresser_Animor;
    public string break_Sound;
    bool flag;

    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        dresser_Animor.Rebind();
        flag = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !flag)
        {
            flag = true;
            theAudio.SoundPlay(break_Sound);
            dresser_Animor.SetTrigger("Break");
        }
    }

    public override void ShowText()
    {
        throw new System.NotImplementedException();
    }
}
