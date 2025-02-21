using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WoasFormsApp.Data
{
    public class Response
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public WoasFormsAppUser? Respondent { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UnixEpoch;

        public Template? Template { get; set; }

        public List<ResponseAnswer> Answers {  get; set; } = new List<ResponseAnswer>();
    }

    public class ResponseAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public TemplateField Field { get; set; }

        public string? AnswerSingleLine { get; set; }
        public string? AnswerMultiLine { get; set; }
        public int? AnswerPositiveInt { get; set; } 
        public bool? AnswerCheckedBox { get; set; }
    }
}