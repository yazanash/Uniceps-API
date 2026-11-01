using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.DTOs.ProductDtos.FAQDtos
{
    public class FrequentlyAskedQuestionCreationDto
    {
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";
        public string QuestionAr { get; set; } = "";
        public string AnswerAr { get; set; } = "";
        public int ProductId { get; set; }
        public FrequentlyAskedQuestion ToModel()
        {
            return new FrequentlyAskedQuestion
            {
                Question = Question,
                Answer = Answer,
                QuestionAr = QuestionAr,
                AnswerAr = AnswerAr,
                ProductId = ProductId
            };
        }

    }
}
