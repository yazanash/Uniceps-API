using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.DTOs.ProductDtos.FAQDtos
{
    public class FrequentlyAskedQuestionDto
    {
        public FrequentlyAskedQuestionDto(FrequentlyAskedQuestion data)
        {

            Id = data.Id;
            Question = data.Question;
            Answer = data.Answer;
            ProductId = data.ProductId;
            QuestionAr = data.QuestionAr;
            AnswerAr = data.AnswerAr;
        }
        public int Id { get; set; }
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";
        public string QuestionAr { get; set; } = "";
        public string AnswerAr { get; set; } = "";
        public int ProductId { get; set; }
    }
}
