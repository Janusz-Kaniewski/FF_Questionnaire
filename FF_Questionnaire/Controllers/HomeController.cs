using FF_Questionnaire.Models;
using FF_Questionnaire.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FF_Questionnaire.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAnswerService _asnwerService;
        private readonly IEmailService _emailService;
        private List<Question> _questions;
        private int _questionCount;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IAnswerService answerService, IEmailService emailService)
        {
            _logger = logger;
            _configuration = configuration;
            _questions = _configuration.GetSection("Questions").Get<List<Question>>();
            _questionCount = _questions.Count;
            _asnwerService = answerService;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            ViewBag.QuestionnaireTitle = _configuration["QuestionnaireTitle"];
            ViewBag.QuestionsCount = _questionCount;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Question(int? index)
        {
            if (index == null || index > _questionCount || index < 0)
            {
                return View("SomethingWentWrong");
            }
            else
            {
                var question = _questions[index ?? 0];

                ViewBag.Index = index;
                ViewBag.QuestionNumber = index + 1;
                ViewBag.QuestionsCount = _questionCount;
                ViewBag.Question = question;

                var answers = _asnwerService.GetAnswers();
                var answer = answers.FirstOrDefault(x => x.QuestionIndex == index);

                ViewBag.Answer = answer;

                return View();
            }
        }

        public IActionResult StoreAnswerAndGoToNext(int index, string answerString, string? answerRadio)
        {
            var answer = new Answer
            {
                QuestionIndex = index,
                AnswerIndex = answerRadio == null ? -1 : int.Parse(answerRadio),
                AnswerText = answerString
            };

            _asnwerService.StoreAnswer(answer);
            
            if (index < _questionCount-1)
            {
                return RedirectToAction("Question", new { index = index + 1 });
            }
            else
            {
                return RedirectToAction("FinishQuiz");
            }
        }

        public IActionResult FinishQuiz()
        {
            var answers = _asnwerService.GetAnswers();
            string subject = $"{_configuration["QuestionnaireTitle"]} - Your answers";

            string emailBody = "Hi!\nBelow are your answers for questions in our questionnaire.\n\n\n";

            foreach (var answer in answers)
            {
                var questionIndex = answer.QuestionIndex;
                var question = _questions[questionIndex];

                emailBody += question.QuestionContent + "\n";

                string answerText = "";

                if (answer.AnswerIndex == -1)
                {
                    answerText = answer.AnswerText;
                }
                else
                {
                    answerText = question.PossibleAnswers[answer.AnswerIndex];
                }

                emailBody += answerText + "\n\n";
            }

            _emailService.SendEmail(subject, emailBody);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}