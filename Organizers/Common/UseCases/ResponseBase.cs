namespace Organizers.Common.UseCases
{
	public class ResponseBase : IResponse
	{
		public bool HasError { get => !string.IsNullOrEmpty(Error); }
		public string Error { get; internal set; }

		public ResponseBase()
		{
		}

		public ResponseBase(string error)
		{
			Error = error;
		}
	}
}