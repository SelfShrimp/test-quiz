using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TMP_Text questionText;
    public Image backgroundImage;
    public Button[] answerButtons;
    public GameObject resultDialogue;
    public GameObject resultsScreen;

    private int currentQuestionIndex = 0;
    private int correctAnswers = 0;
    private QuestionData questionData;

    void Start()
    {
        LoadQuestions();
        ShowQuestion();
    }

    void LoadQuestions()
    {
        string json = Resources.Load<TextAsset>("questions").text;
        questionData = JsonUtility.FromJson<QuestionData>(json);
    }

    void ShowQuestion()
    {
        Question currentQuestion = questionData.questions[currentQuestionIndex];
        questionText.text = currentQuestion.question;
        backgroundImage.sprite = Resources.Load<Sprite>(currentQuestion.background);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = currentQuestion.answers[i].text;
            bool isCorrect = currentQuestion.answers[i].correct;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(isCorrect));
        }
    }

    void CheckAnswer(bool isCorrect)
    {
        if (isCorrect)
        {
            correctAnswers++;
            StartCoroutine(ShowCorrectScreen());
        }
        else
        {
            StartCoroutine(ShowWrongScreen());
        }
    }

    IEnumerator ShowCorrectScreen()
    {
        resultDialogue.GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.0f, 0.4f);
        resultDialogue.GetComponentInChildren<TMP_Text>().text = "Верно";
        resultDialogue.SetActive(true);
        yield return new WaitForSeconds(2f);
        resultDialogue.SetActive(false);
        NextQuestion();
    }

    IEnumerator ShowWrongScreen()
    {
        resultDialogue.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 0.4f);
        resultDialogue.GetComponentInChildren<TMP_Text>().text = "Не верно";
        resultDialogue.SetActive(true);
        yield return new WaitForSeconds(2f);
        resultDialogue.SetActive(false);
        NextQuestion();
    }

    void NextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex < questionData.questions.Length)
        {
            ShowQuestion();
        }
        else
        {
            ShowResults();
        }
    }

    void ShowResults()
    {
        resultsScreen.SetActive(true);
        resultsScreen.GetComponentInChildren<TMP_Text>().text =
            "Правильных ответов: " + correctAnswers + "/" + questionData.questions.Length;
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("StartScene");
    }
}
