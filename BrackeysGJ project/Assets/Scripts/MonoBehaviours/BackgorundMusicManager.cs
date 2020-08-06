using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgorundMusicManager : MonoBehaviour
{

    private float _fadeTime;
    private float _tm;
    private AudioSource _source;
    private AudioClip _newClip;
    private AudioClip _current;

    private bool _changing = false;
    // Start is called before the first frame update
    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _tm = _fadeTime;
    }

    public void ChangeBackgroundMusic(AudioClip newClip,float time)
    {
        _fadeTime = time;
        _newClip = newClip;
        _changing = true;
        _tm = _fadeTime;
    }

    private void Update()
    {
        _current = _source.clip;
        if(!_changing) return;
        _tm -= Time.deltaTime;
        if (_tm > _fadeTime/2f)
        {
            _source.volume = Mathf.Max(0f, _tm / _fadeTime - _fadeTime/2);
        }
        else if(_tm > 0f)
        {
            if (_source.clip != _newClip)
            {
                _source.Stop();
                _source.clip = _newClip;
                _source.Play();
            }
            _source.volume = Mathf.Min(1f, 1f - (_tm / (_fadeTime / 2f)));
        }
        else
        {
            _source.volume = 1f;
            _changing = false;
        }
    }
}
