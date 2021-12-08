using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notice : MonoBehaviour
{
    [SerializeField] GameObject notice_content_window;
    [SerializeField] TextMeshProUGUI title_text;
    [SerializeField] TextMeshProUGUI content_text;

    string[] title_strings = { "[����] ���� ���� �ȳ�", "[����] ������ �� ��հ� ���� ���", "[ũ����] �����ڵ�" };
    string[] content_strings =
        {
        "������ �÷����ϴ� ���� ���װ� �߻����� ��, �Ʒ� ���Ϸ� �������ּ���.\nsdh20200219@sdh.hs.kr",
        "������ �� ĳ���Ϳ� �� ĳ������ �������� �κ��� �����ؼ� ���ø� ���� ����ְ� ������ �÷����Ͻ� �� �ֽ��ϴ�.",
        "��������ذ���б� �б���� ������Ʈ\n\n" +
            "���� ��ȹ, ���α׷��� �Ӵ�ȣ\n" +
            "���α׷��� ä����\n" +
            "�����̳� �����\n\n" +
            "Team.����ð�nullnull�ؿ�"
    };

    public void ShowWindow(int index)
    {
        notice_content_window.SetActive(true);
        title_text.text = title_strings[index];
        content_text.text = content_strings[index];
    }

    public void CloseWindow()
    {
        notice_content_window.SetActive(false);
    }
}
