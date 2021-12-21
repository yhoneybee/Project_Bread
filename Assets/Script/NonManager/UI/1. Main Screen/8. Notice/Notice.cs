using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notice : MonoBehaviour
{
    [SerializeField] GameObject notice_content_window;
    [SerializeField] TextMeshProUGUI title_text;
    [SerializeField] TextMeshProUGUI content_text;

    string[] title_strings = { "[공지] 버그 제보 안내", "[꿀팁] 게임을 더 재밌게 즐기는 방법", "[크레딧] 개발자들" };
    string[] content_strings =
        {
        "게임을 플레이하는 도중 버그가 발생했을 시, 아래 메일로 제보해주세요.\nsdh20200219@sdh.hs.kr",
        "각각의 빵 캐릭터와 적 캐릭터의 디테일한 부분을 집중해서 보시면 더욱 재미있게 게임을 플레이하실 수 있습니다.",
        "서울디지텍고등학교 학교기업 프로젝트\n\n" +
            "메인 기획, 프로그래밍 임대호\n" +
            "프로그래밍 채영훈\n" +
            "디자이너 장수진\n\n" +
            "Team.저희시간nullnull해요"
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
