namespace FF_Questionnaire.Models
{
    public class Question
    {
        public string QuestionContent { get; set; }
        public string InputType { get; set; }
        public List<string> PossibleAnswers { get; set; }
    }
}
