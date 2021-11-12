using FF_Questionnaire.Models;
using FF_Questionnaire.Services.Interface;

namespace FF_Questionnaire.Services.Implementation
{
    public class AnswerService : IAnswerService
    {
        private readonly List<Answer> _answers;

        public AnswerService()
        {
            _answers = new List<Answer>();
        }
        
        public void StoreAnswer(Answer answer)
        {
            _answers.Add(answer);
        }

        public List<Answer> GetAnswers()
        {
            return _answers;
        }
    }
}
