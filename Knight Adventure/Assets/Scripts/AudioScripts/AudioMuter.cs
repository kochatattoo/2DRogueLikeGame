using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMuter : MonoBehaviour
{
    //��������� ���������
    public bool is_music = false;

    //��������� ���������
    private AudioSource _as; //Audio Source
    private float base_volume = 1F; //������� ���������

    void Start()
    {
        //������������� ���������� ��� ������ ����
        //�������� ��������� AS
        _as=this.gameObject.GetComponent<AudioSource>();
        //�������� ������� ��������� �� AS
        base_volume = _as.volume;
    }

    //������ ���� �� ��������� ��������� � ������������� ���������
    void Update()
    {
        //��� ������ ��������, ������ ��� ��� ���
         if(is_music)
        {
            _as.volume = (AudioManager.music) ? base_volume : 0F;
        }
        else
        {
            _as.volume = (AudioManager.sounds) ? base_volume : 0F;
        }
    }
}
