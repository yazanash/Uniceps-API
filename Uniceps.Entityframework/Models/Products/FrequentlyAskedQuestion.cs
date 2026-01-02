using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.Products
{
    public class FrequentlyAskedQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; } = "";
        public string QuestionAr { get; set; } = "";
        public string Answer { get; set; } = "";
        public string AnswerAr { get; set; } = "";
        public int ProductId { get; set; }
    }
}
