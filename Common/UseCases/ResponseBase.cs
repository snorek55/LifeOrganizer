namespace Common.UseCases
{
	public abstract class ResponseBase : IResponse
	{
		public bool HasError { get => !string.IsNullOrEmpty(Error); }
		public string Error { get; set; }

		protected ResponseBase()
		{
		}

		protected ResponseBase(string error)
		{
			Error = error;
		}
	}
}