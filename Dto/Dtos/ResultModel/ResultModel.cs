using System.ComponentModel.DataAnnotations;

namespace ApiGelirGider.DTOs.ResultModel
{
    public class ResultModel
    {
        
        public string? ErrorMessage { get; set; } = string.Empty;

        public string? ResultMessage { get; set; } = string.Empty;
    }
}