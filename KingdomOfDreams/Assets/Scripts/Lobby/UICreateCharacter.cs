using UnityEngine;
using UnityEngine.UI;

public class UICreateCharacter : MonoBehaviour
{

    public Text questionText; // 질문을 보여주는 UI Text
    //public Text[] answerText; // 대답을 보여주는 UI Text]
    public GameObject[] answerImage;
    public GameObject[] objects;    //
    private GameObject currentObj;  //현재 오브젝트
    public Button answerButton1; // 대답 버튼 1
    public Button answerButton2; // 대답 버튼 2

    private TreeNode currentNode; // 현재 노드

    public GameObject resultPanel;
    public GameObject[] characters;
    public GameObject nicknamePanel;

    // 이진 트리 노드 클래스
    private class TreeNode
    {
        public GameObject key; // 키 (질문 또는 캐릭터 종류)
        public string value; // 값 (질문 또는 null)
        public int characterNum;
        public TreeNode left; // 왼쪽 자식 노드
        public TreeNode right; // 오른쪽 자식 노드

    }


    void Start()
    {
        this.CreateTree();

        answerButton1.onClick.AddListener(() =>
        {
            OnClickAnswerButton1();
        });

        answerButton2.onClick.AddListener(() =>
        {
            OnClickAnswerButton2();
        });
    }

    private void Update()
    {
        if (this.resultPanel.activeSelf)
        {
            this.nicknamePopup();
        }
    }


    public void CreateTree()
    {
        // 이진 트리 구성
        TreeNode root = new TreeNode { key = null, value = "선택하세요", characterNum = -1 };
        TreeNode node1 = new TreeNode { key = objects[0], value = "선택하세요", characterNum = -1 };
        TreeNode node2 = new TreeNode { key = objects[1], value = "선택하세요", characterNum = -1 };
        TreeNode node3 = new TreeNode { key = objects[2], value = "선택하세요", characterNum = -1 };
        TreeNode node4 = new TreeNode { key = objects[3], value = "선택하세요", characterNum = -1 };
        TreeNode node5 = new TreeNode { key = objects[2], value = "선택하세요", characterNum = -1 };
        TreeNode node6 = new TreeNode { key = objects[3], value = "선택하세요", characterNum = -1 };
        TreeNode node7 = new TreeNode { key = objects[4], value = null, characterNum = 00 };
        TreeNode node8 = new TreeNode { key = objects[5], value = null, characterNum = 01 };
        TreeNode node9 = new TreeNode { key = objects[4], value = null, characterNum = 02 };
        TreeNode node10 = new TreeNode { key = objects[5], value = null, characterNum = 03 };
        TreeNode node11 = new TreeNode { key = objects[4], value = null, characterNum = 04 };
        TreeNode node12 = new TreeNode { key = objects[5], value = null, characterNum = 05 };
        TreeNode node13 = new TreeNode { key = objects[4], value = null, characterNum = 06 };
        TreeNode node14 = new TreeNode { key = objects[5], value = null, characterNum = 07 };

        root.left = node1;
        root.right = node2;
        node1.left = node3;
        node1.right = node4;
        node2.left = node5;
        node2.right = node6;
        node3.left = node7;
        node3.right = node8;
        node4.left = node9;
        node4.right = node10;
        node5.left = node11;
        node5.right = node12;
        node6.left = node13;
        node6.right = node14;

        currentNode = root; // 현재 노드를 루트 노드로 초기화


        // 초기 질문 텍스트 설정
        questionText.text = currentNode.value;

        //this.currentObj = objects[0];


    }
    void OnClickAnswerButton1()
    {
        if (currentNode.left != null)
        {
            currentNode = currentNode.left;
            Debug.LogFormat("<color=yellow>{0}</color>", currentNode.key);
            UpdateQuestionText(); //질문 텍스트 업데이트
            UpdateAnswerImage(); // 대답 이미지 업데이트

            if (currentNode.value == null)
            {
                Debug.Log("캐릭터 종류: " + currentNode.characterNum);

                this.resultPanel.SetActive(true);
                this.characters[currentNode.characterNum].SetActive(true);

                Debug.Log(currentNode.characterNum);
                InfoManager.instance.PlayerInfo.nowCharacterId = currentNode.characterNum;
                InfoManager.instance.PlayerInfo.myCharacters[currentNode.characterNum] = 1;
                InfoManager.instance.SavePlayerInfo();

                //EventManager.instance.onCreateCharacter(currentNode.characterNum);


            }
        }

    }

    void OnClickAnswerButton2()
    {
        if (currentNode.right != null)
        {
            currentNode = currentNode.right;
            Debug.LogFormat("<color=cyan>{0}</color>", currentNode.key);
            UpdateQuestionText(); //질문 텍스트 업데이트
            UpdateAnswerImage(); // 대답 텍스트 업데이트

            if (currentNode.value == null)
            {
                Debug.Log("캐릭터 번호: " + currentNode.characterNum);

                this.resultPanel.SetActive(true);
                this.characters[currentNode.characterNum].SetActive(true);

                Debug.Log(currentNode.characterNum);
                InfoManager.instance.PlayerInfo.nowCharacterId = currentNode.characterNum;
                InfoManager.instance.PlayerInfo.myCharacters[currentNode.characterNum] = 1;
                InfoManager.instance.SavePlayerInfo();
                //EventManager.instance.onCreateCharacter(currentNode.characterNum);


            }

        }
    }


    // 질문 텍스트 업데이트 함수
    void UpdateQuestionText()
    {
        if (currentNode.value != null)
        {
            questionText.text = currentNode.value;
        }
    }
    // 대답 텍스트 업데이트 함수
    void UpdateAnswerImage()
    {
        //if (currentNode.key != null)
        //{
        //    if (currentNode.left != null || currentNode.right != null)
        //    {
        //        answerImage[0].SetActive(false);
        //        answerImage[0] = currentNode.left.key;
        //        answerImage[0].SetActive(true);

        //        answerImage[1].SetActive(false);
        //        answerImage[1] = currentNode.right.key;
        //        answerImage[1].SetActive(true);
        //        //answerText[0].text = currentNode.left.key;
        //        //answerText[1].text = currentNode.right.key;
        //    }
        //}

        if (currentNode.left != null && currentNode.right != null)
        {
            answerImage[0].SetActive(true);
            answerImage[0].GetComponent<Image>().sprite = currentNode.left.key.GetComponent<Image>().sprite;

            answerImage[1].SetActive(true);
            answerImage[1].GetComponent<Image>().sprite = currentNode.right.key.GetComponent<Image>().sprite;
        }
        else if (currentNode.left == null && currentNode.right != null)
        {
            answerImage[0].SetActive(false);

            answerImage[1].SetActive(true);
            answerImage[1].GetComponent<Image>().sprite = currentNode.right.key.GetComponent<Image>().sprite;
        }
        else if (currentNode.left != null && currentNode.right == null)
        {
            answerImage[0].SetActive(true);
            answerImage[0].GetComponent<Image>().sprite = currentNode.left.key.GetComponent<Image>().sprite;

            answerImage[1].SetActive(false);
        }
    }

    public void nicknamePopup()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //this.nicknamePanel.SetActive(true);
            this.characters[currentNode.characterNum].SetActive(false);

            EventManager.instance.onTouched();


        }

    }

}
