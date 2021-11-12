using FF_Questionnaire.Models;

namespace FF_Questionnaire.Services.Interface
{
    public interface IAnswerService
    {
        void StoreAnswer(Answer answer);
        List<Answer> GetAnswers();
    }
}
