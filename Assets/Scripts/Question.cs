[System.Serializable]
public class QuestionData
{
    public Question[] questions;
}

[System.Serializable]
public class Question
{
    public string question;
    public Answer[] answers;
    public string background;
}

[System.Serializable]
public class Answer
{
    public string text;
    public bool correct;
}
