namespace Portfolio.Finance.Services.DTO
{
    public class OperationResult
    {
        public string Message { get; set; }

        public bool IsSuccess { get; set; }
    }
    
    public class OperationResult<TResult>
    {
        public string Message { get; set; }

        public bool IsSuccess { get; set; }
        
        public TResult Result { get; set; }
    }
}
