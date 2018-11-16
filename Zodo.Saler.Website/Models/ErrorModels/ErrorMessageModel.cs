namespace Zodo.Saler.Website.Models
{
    public class ErrorMessageModel
    {
        public ErrorMessageModel()
        { }

        public ErrorMessageModel(string title, string message)
        {
            Title = title;
            Message = message;
        }

        /// <summary>
        /// 错误提醒
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 错误详情
        /// </summary>
        public string Message { get; set; }
    }
}
