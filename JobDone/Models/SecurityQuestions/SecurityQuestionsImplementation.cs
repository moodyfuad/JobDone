
using JobDone.Data;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.SecurityQuestions
{
    public class SecurityQuestionsImplementation : ISecurityQuestion
    {
        private readonly DbSet<SecurityQuestionModel> _questions;

        public SecurityQuestionsImplementation(JobDoneContext context)
        {
            _questions = context.SecurityQuestionModels;
        }

        public List<SecurityQuestionModel> GetQuestions()
        {
            return _questions.ToList();
        }
    }
}
