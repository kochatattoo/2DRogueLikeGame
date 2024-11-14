using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {  get; private set; } //��������� �������

    public static bool music = true; //�������� ����������� ������
    public static bool sounds = true; //�������� ����������� ������
    
    private void Start()
    {
        //������ ��������� ������������� ����������
        if(Instance == null)
        {
            //������ ������ �� ��������� �������
            Instance = this;
        }
        else if(Instance == this)
        {
            //������� ������
            Destroy(gameObject);
        }

        //������ ��� ����� �������, ��� �� ������ �� �����������
        //��� �������� �� ������ �����
        DontDestroyOnLoad(gameObject);

        //� ��������� �������������
        InitializeManager();
    }

    //����� ������������� ���������
    private void InitializeManager()
    {
        //����� �� ��������� � ������������ ��������� �� PlayerPrefs
        music = System.Convert.ToBoolean(PlayerPrefs.GetString("music", "true"));
        sounds = System.Convert.ToBoolean(PlayerPrefs.GetString("sounds", "true"));
    }
    public static void SaveSettings()
    {
        PlayerPrefs.SetString("music",music.ToString()); //��������� ��������� ������
        PlayerPrefs.SetString("sounds", sounds.ToString());//��������� ��������� ������
        PlayerPrefs.Save();//��������� ���������
    }
   
}
